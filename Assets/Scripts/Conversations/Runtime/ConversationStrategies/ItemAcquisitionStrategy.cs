using System;
using DG.Tweening;
using Inventory;
using Painting.Runtime;
using Sound;
using UnityEngine;
using Yarn.Unity;

namespace Conversations {
    [Serializable]
    public class ItemAcquisitionStrategy : YarnDialogueStrategyBase {
        [SerializeField] private ItemData item;
        [SerializeField] private PlayerSprite playerSprite;
        [SerializeField] private GameObject itemSpritePrefab;
        [SerializeField] private int spriteOrderInLayer = 100;
        [SerializeField] private AudioClip collectionClip;
        
        private const float OVERSHOOT = 4f;
        private const float SCALE_DURATION = 0.2f;
        private const float HOLD_DURATION = 0.3f;
        private const float BURST_SCALE = 2f;
        private const float BURST_DURATION = 0.12f;
        private const float BURST_FADE_DURATION = 0.08f;

        public override async YarnTask Execute() {
            // instantiate at player position with zero scale
            GameObject newItem = UnityEngine.Object.Instantiate(
                itemSpritePrefab,
                playerSprite.transform.position,
                Quaternion.identity
            );
            newItem.transform.localScale = Vector3.zero;

            // set up sprite, color and layer
            SpriteRenderer spriteRenderer = newItem.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = item.sprite;
            spriteRenderer.sortingOrder = spriteOrderInLayer;
            if (item is ColorData colorData)
                spriteRenderer.color = colorData.color;

            float targetScale = item.paintingScaleFactor;
            
            Tween appearTween = newItem.transform.DOScale(targetScale, SCALE_DURATION)
                .SetEase(Ease.OutBack, OVERSHOOT);
            
            Sequence burstSequence = DOTween.Sequence();
            burstSequence.Join(newItem.transform
                .DOScale(targetScale * BURST_SCALE, BURST_DURATION)
                .SetEase(Ease.OutQuad))
                .OnComplete(() => {
                    SoundController.Instance.PlayOneShotSfx(collectionClip);
                });
            burstSequence.Join(spriteRenderer
                .DOFade(0f, BURST_FADE_DURATION)
                .SetEase(Ease.InQuad)
                .SetDelay(BURST_DURATION - BURST_FADE_DURATION)); 
            
            // create a sequence of the sprite appearing, waiting a bit and then bursting
            Sequence sequence = DOTween.Sequence();
            sequence.Append(appearTween)
                .AppendInterval(HOLD_DURATION)
                .Append(burstSequence);
            
            await sequence.AsyncWaitForCompletion();
            
            InventoryController.Instance.AddItem(item);
            UnityEngine.Object.Destroy(newItem);
        }
    }
}