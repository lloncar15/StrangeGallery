using UnityEditor;

[CustomEditor(typeof(PaintingObject))]
public class PaintingObjectEditor : Editor {
    private PaintingObject _paintingObject;
    private Editor _configEditor;

    private void OnEnable() {
        _paintingObject = (PaintingObject)target;
    }

    private void OnDisable() {
        DestroyImmediate(_configEditor);
    }

    public override void OnInspectorGUI() {
        // Draw default PaintingObject inspector
        DrawDefaultInspector();

        PaintingCameraConfig config = _paintingObject.CameraConfig;
        if (!config) {
            EditorGUILayout.HelpBox("Assign a Painting Camera Config asset to edit its values here.", MessageType.Info);
            return;
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Painting Camera Config", EditorStyles.boldLabel);

        // Reuse or recreate the embedded editor for the config asset
        CreateCachedEditor(config, null, ref _configEditor);

        EditorGUI.BeginChangeCheck();
        _configEditor.OnInspectorGUI();
        if (EditorGUI.EndChangeCheck()) {
            EditorUtility.SetDirty(config);
        }
    }
}