using System.Collections.Generic;
using System;
using System.IO;
using Mono.TextTemplating;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;
using Object = UnityEngine.Object;

namespace FasterGames.T4.Editor
{
    /// <summary>
    /// The TextTemplate (.tt) file importer for Unity3D
    /// </summary>
    /// <remarks>
    /// This importer relies on mono/t4 to parse templates and generate csharp files
    /// </remarks>
    [ScriptedImporter(2, "tt")]
    public class TextTemplateImporter : ScriptedImporter
    {
        /// <summary>
        /// Flag; determines which importer feature-set is supported
        /// </summary>
        [Tooltip("The importer version to use")]
        public ImporterVersion importerVersion;
        
        /// <summary>
        /// Flag; If set, any empty files that are generated will be ignored.
        /// </summary>
        [Tooltip("If set, any empty files that are generated will be ignored.")]
        public bool removeIfEmptyGeneration = true;

        /// <summary>
        /// Data to be embedded in the template host, so it can be accessed by the template
        /// </summary>
        [Tooltip("Allows data to be embedded and accessed by the template.")]
        public Object embeddedData;

        /// <summary>
        /// Additional types to import into the template context.
        /// </summary>
        /// <remarks>
        /// Use this if your <see cref="embeddedData"/> includes data types that are outside
        /// the default referenced types
        /// </remarks>
        /// <example>
        /// If <see cref="embeddedData"/> contained `MyCustomNameSpace.MyCustomType dataElement`
        /// then you'd add `MyCustomNameSpace.MyCustomType` here.
        /// </example>
        [Tooltip("Additional types to import into the template context.")]
        public List<string> additionalTypes;
        
        /// <inheritdoc />
        public override void OnImportAsset(AssetImportContext ctx)
        {
            if (ctx == null)
            {
                return;
            }
            
            var outputFilePath = Path.Combine(Path.GetDirectoryName(ctx.assetPath), Path.GetFileNameWithoutExtension(ctx.assetPath) + ".cs");
            var src = File.ReadAllText(ctx.assetPath);

            TemplateGenerator generator;

            switch (importerVersion)
            {
                case ImporterVersion.Stable:
                    generator = new TemplateGenerator();
                    break;
                case ImporterVersion.Beta:
                {
                    generator = UnityDataHost<Object>.CreateInstance(embeddedData, additionalTypes);
                    break;
                }
                default:
                    throw new InvalidOperationException();
            }
            
            if (generator.ProcessTemplate(ctx.assetPath, src, ref outputFilePath, out string dst))
            {
                if (!removeIfEmptyGeneration || dst.Length > 0)
                {
                    File.WriteAllText(outputFilePath, dst);
                }

                EditorUtils.Once(() =>
                {
                    // TODO(bengreenier): is there a less expensive way to force it to reload scripts/appdomain?
                    AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
                });
            }
            else
            {
                for (var i = 0; i < generator.Errors.Count; i++)
                {
                    var err = generator.Errors[i];
                    
                    Debug.LogError(err.ToString());
                    Debug.LogError(dst);
                }
            }

            // core asset import
            var asset = new TextAsset(src);
            ctx.AddObjectToAsset("text", asset);
            ctx.SetMainObject(asset);
            
            // setup data dependency
            if (embeddedData != null && importerVersion == ImporterVersion.Beta)
            {
                var dataPath = AssetDatabase.GetAssetPath(embeddedData);
                var dataGuid = AssetDatabase.GUIDFromAssetPath(dataPath);

                ctx.DependsOnArtifact(dataGuid);
                    
                // needed to enforce the dependency
                AssetDatabase.LoadAssetAtPath<Object>(dataPath);
            }
        }
    }
}