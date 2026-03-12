#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Conversations {
    [CustomEditor(typeof(YarnDialogueProvider))]
    public class YarnDialogueProviderEditor : Editor {
        private List<Type> _conditionTypes;
        private List<Type> _strategyTypes;

        private ReorderableList _conditionsList;
        private ReorderableList _strategiesList;

        private readonly Dictionary<string, bool> _foldouts = new();
        private readonly Dictionary<string, List<Type>> _typesByLabel = new();

        private void OnEnable() {
            _conditionTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsSubclassOf(typeof(YarnDialogueConditionBase)) && !t.IsAbstract)
                .ToList();

            _strategyTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsSubclassOf(typeof(YarnDialogueStrategyBase)) && !t.IsAbstract)
                .ToList();

            _foldouts["Conditions"] = true;
            _foldouts["Strategies"] = true;

            _typesByLabel["Conditions"] = _conditionTypes;
            _typesByLabel["Strategies"] = _strategyTypes;

            SerializedProperty conditionsProp = serializedObject.FindProperty("conditions");
            SerializedProperty strategiesProp = serializedObject.FindProperty("strategies");

            if (conditionsProp != null)
                _conditionsList = BuildReorderableList(conditionsProp, "Conditions", _conditionTypes);

            if (strategiesProp != null)
                _strategiesList = BuildReorderableList(strategiesProp, "Strategies", _strategyTypes);
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();

            DrawPropertiesExcluding(serializedObject, "conditions", "strategies");

            EditorGUILayout.Space();

            if (_conditionsList == null || _strategiesList == null) {
                EditorGUILayout.HelpBox(
                    "Could not find serialized properties. Ensure field names match.",
                    MessageType.Error
                );
                return;
            }

            DrawFoldableList(_conditionsList, "Conditions");
            EditorGUILayout.Space();
            DrawFoldableList(_strategiesList, "Strategies");

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawFoldableList(ReorderableList list, string label) {
            bool expanded = _foldouts[label];

            Rect headerRect = EditorGUILayout.GetControlRect();
            Rect countRect = new Rect(headerRect.xMax - 50, headerRect.y, 50, headerRect.height);
            Rect foldoutRect = new Rect(headerRect.x, headerRect.y, headerRect.width - 55, headerRect.height);

            EditorGUI.BeginChangeCheck();
            int newSize = EditorGUI.DelayedIntField(countRect, list.serializedProperty.arraySize);
            if (EditorGUI.EndChangeCheck()) {
                list.serializedProperty.arraySize = Mathf.Max(0, newSize);
                serializedObject.ApplyModifiedProperties();
            }

            bool newExpanded = EditorGUI.Foldout(foldoutRect, expanded, label, true, EditorStyles.foldoutHeader);
            if (newExpanded != expanded) {
                _foldouts[label] = newExpanded;
                Repaint();
            }

            if (!expanded)
                return;

            list.displayAdd = true;
            list.displayRemove = true;
            list.draggable = true;
            RestoreListCallbacks(list, label, list.serializedProperty);
            list.DoLayoutList();
        }

        private ReorderableList BuildReorderableList(SerializedProperty property, string label, List<Type> types) {
            ReorderableList list = new ReorderableList(
                serializedObject,
                property,
                draggable: true,
                displayHeader: false,
                displayAddButton: true,
                displayRemoveButton: true
            );

            list.headerHeight = 0f;
            RestoreListCallbacks(list, label, property);
            SetupAddDropdownCallback(list, property, types);

            return list;
        }

        private void RestoreListCallbacks(ReorderableList list, string label, SerializedProperty property) {
            list.elementHeightCallback = index => {
                SerializedProperty element = property.GetArrayElementAtIndex(index);

                if (element.managedReferenceValue == null)
                    return EditorGUIUtility.singleLineHeight + 4;

                if (!element.isExpanded)
                    return EditorGUIUtility.singleLineHeight + 4;

                return GetExpandedHeight(element) + 4;
            };

            list.drawElementCallback = (rect, index, isActive, isFocused) => {
                SerializedProperty element = property.GetArrayElementAtIndex(index);
                rect.yMin += 2;

                if (element.managedReferenceValue == null) {
                    Rect dropdownRect = new Rect(rect.x + 10, rect.y, rect.width - 10, EditorGUIUtility.singleLineHeight);

                    if (EditorGUI.DropdownButton(dropdownRect, new GUIContent("Select type..."), FocusType.Passive)) {
                        GenericMenu menu = new GenericMenu();
                        List<Type> types = _typesByLabel.GetValueOrDefault(label, new List<Type>());

                        if (types.Count == 0)
                            menu.AddDisabledItem(new GUIContent("No types found"));

                        foreach (Type type in types) {
                            Type captured = type;
                            int capturedIndex = index;
                            menu.AddItem(new GUIContent(captured.Name), false, () => {
                                serializedObject.Update();
                                SerializedProperty target = property.GetArrayElementAtIndex(capturedIndex);
                                target.managedReferenceValue = Activator.CreateInstance(captured);
                                target.isExpanded = true;
                                serializedObject.ApplyModifiedProperties();
                            });
                        }

                        menu.ShowAsContext();
                    }
                    return;
                }

                string typeName = element.managedReferenceValue.GetType().Name;

                Rect foldoutRect = new Rect(
                    rect.x + 10,
                    rect.y,
                    rect.width - 10,
                    EditorGUIUtility.singleLineHeight
                );
                element.isExpanded = EditorGUI.Foldout(foldoutRect, element.isExpanded, typeName, true);

                if (!element.isExpanded)
                    return;

                float yOffset = EditorGUIUtility.singleLineHeight + 2;
                EditorGUI.indentLevel++;

                SerializedProperty iterator = element.Copy();
                SerializedProperty end = element.GetEndProperty();
                bool enterChildren = true;

                while (iterator.NextVisible(enterChildren) && !SerializedProperty.EqualContents(iterator, end)) {
                    float height = EditorGUI.GetPropertyHeight(iterator, true);
                    Rect fieldRect = new Rect(rect.x, rect.y + yOffset, rect.width, height);
                    EditorGUI.PropertyField(fieldRect, iterator, true);
                    yOffset += height + 2;
                    enterChildren = false;
                }

                EditorGUI.indentLevel--;
            };

            list.drawElementBackgroundCallback = (rect, index, isActive, isFocused) => {
                if (index < 0)
                    return;

                if (isFocused)
                    EditorGUI.DrawRect(rect, new Color(0.24f, 0.49f, 0.91f, 0.3f));
                else if (index % 2 == 0)
                    EditorGUI.DrawRect(rect, new Color(0f, 0f, 0f, 0.05f));
            };
        }

        private void SetupAddDropdownCallback(ReorderableList list, SerializedProperty property, List<Type> types) {
            list.onAddDropdownCallback = (buttonRect, reorderableList) => {
                GenericMenu menu = new GenericMenu();

                if (types.Count == 0)
                    menu.AddDisabledItem(new GUIContent("No types found"));

                foreach (Type type in types) {
                    Type captured = type;
                    menu.AddItem(new GUIContent(captured.Name), false, () => {
                        serializedObject.Update();
                        property.arraySize++;
                        SerializedProperty newElement = property.GetArrayElementAtIndex(property.arraySize - 1);
                        newElement.managedReferenceValue = Activator.CreateInstance(captured);
                        newElement.isExpanded = true;
                        serializedObject.ApplyModifiedProperties();
                    });
                }

                menu.ShowAsContext();
            };
        }

        private float GetExpandedHeight(SerializedProperty element) {
            float height = EditorGUIUtility.singleLineHeight + 2;

            SerializedProperty iterator = element.Copy();
            SerializedProperty end = element.GetEndProperty();
            bool enterChildren = true;

            while (iterator.NextVisible(enterChildren) && !SerializedProperty.EqualContents(iterator, end)) {
                height += EditorGUI.GetPropertyHeight(iterator, true) + 2;
                enterChildren = false;
            }

            return height;
        }
    }
}
#endif