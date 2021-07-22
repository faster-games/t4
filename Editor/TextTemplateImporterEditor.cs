using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace FasterGames.T4.Editor
{
    /// <summary>
    /// Custom editor for <see cref="TextTemplateImporter"/>
    /// </summary>
    [CustomEditor(typeof(TextTemplateImporter))]
    [CanEditMultipleObjects]
    public class TextTemplateImporterEditor : ScriptedImporterEditor
    {
        /// <inheritdoc />
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            var tgt = ((TextTemplateImporter) target);

            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(TextTemplateImporter.importerVersion)));
            
            EditorGUILayout.PropertyField(
                serializedObject.FindProperty(nameof(TextTemplateImporter.removeIfEmptyGeneration)));

            // only the beta gets embeddedData support
            if (tgt.importerVersion == ImporterVersion.Beta)
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Embedded Data", EditorStyles.largeLabel);
                EditorGUILayout.HelpBox($"Allows you to access data from Unity within your template. The data will be exposed at `Host.{nameof(UnityDataHost<Object>.Data)}` inside your template. This is a {nameof(ImporterVersion.Beta)} feature.", MessageType.None);

                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.PropertyField(
                        serializedObject.FindProperty(nameof(TextTemplateImporter.additionalTypes)));
                    EditorGUILayout.HelpBox(
                        "If your template fails to compile after enabling embedded data, you may need to include additional types in the Template Engine context.",
                        MessageType.Warning);
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(TextTemplateImporter.embeddedData)));
                
                var embeddedData = tgt.embeddedData;

                var initEnabled = GUI.enabled;
                GUI.enabled = false;
                if (embeddedData != null)
                {
                    DrawPropertiesExcluding(new SerializedObject(embeddedData));
                }
                GUI.enabled = initEnabled;
            }

            serializedObject.ApplyModifiedProperties();
            ApplyRevertGUI();
        }
    }
}