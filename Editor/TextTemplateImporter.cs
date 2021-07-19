using System.Collections.Generic;
using System;
using System.IO;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace FasterGames.T4.Editor
{
    /// <summary>
    /// The TextTemplate (.tt) file importer for Unity3D
    /// </summary>
    /// <remarks>
    /// This importer relies on mono/t4 to parse templates and generate csharp files
    /// </remarks>
    [ScriptedImporter(1, "tt")]
    public class TextTemplateImporter : ScriptedImporter
    {
        /// <summary>
        /// Flag; If set, any empty files that are generated will be ignored.
        /// </summary>
        [Tooltip("If set, any empty files that are generated will be ignored.")]
        public bool removeIfEmptyGeneration = true;

        /// <inheritdoc />
        public override void OnImportAsset(AssetImportContext ctx)
        {   
            var outputFilePath = Path.Combine(Path.GetDirectoryName(ctx.assetPath), Path.GetFileNameWithoutExtension(ctx.assetPath) + ".cs");
            var src = File.ReadAllText(ctx.assetPath);

            var generator = new Mono.TextTemplating.TemplateGenerator();
            if (generator.ProcessTemplate(ctx.assetPath, src, ref outputFilePath, out string dst))
            {
                if (!removeIfEmptyGeneration || dst.Length > 0)
                {
                    File.WriteAllText(outputFilePath, dst);
                }

                Once(() =>
                {
                    // TODO(bengreenier): is there a less expensive way to force it to reload scripts/appdomain?
                    AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
                });
            }
            else
            {
                foreach (var err in generator.Errors)
                {
                    Debug.LogError(err.ToString());
                }
            }

            var asset = new TextAsset(src);
            ctx.AddObjectToAsset("text", asset);
            ctx.SetMainObject(asset);
        }

        /// <summary>
        /// <see cref="EditorApplication"/> task runner
        /// </summary>
        /// <remarks>
        /// This allows us to schedule work to be run once on Editor update
        /// </remarks>
        /// <param name="cb">task to run</param>
        private static void Once(Action cb)
        {
            OnceTasks.Enqueue(cb);
            EditorApplication.update += OnceRunner;
        }

        /// <summary>
        /// Storage for tasks. See <see cref="Once"/>
        /// </summary>
        private static readonly Queue<Action> OnceTasks = new Queue<Action>();

        /// <summary>
        /// Worker job that runs on Editor update
        /// </summary>
        /// <remarks>
        /// This checks if any jobs need to be run, and if so, runs them.
        /// </remarks>
        private static void OnceRunner()
        {
            while (OnceTasks.Count > 0)
            {
                OnceTasks.Dequeue()();
            }

            EditorApplication.update -= OnceRunner;
        }
    }
}