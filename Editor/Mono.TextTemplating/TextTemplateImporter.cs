using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEditor;
using UnityEditor.Experimental.AssetImporters;
using UnityEngine;

namespace FasterGames.T4.Editor
{
    /// <summary>
    /// Importer for '.tt' files
    /// </summary>
    [ScriptedImporter(1, "tt")]
    public class TextTemplateImporter : ScriptedImporter
    {
        public bool RemoveIfEmptyGeneration = false;

        public override void OnImportAsset(AssetImportContext ctx)
        {
            var outputFilePath = Path.Combine(Path.GetDirectoryName(ctx.assetPath), Path.GetFileNameWithoutExtension(ctx.assetPath) + ".cs");
            var src = File.ReadAllText(ctx.assetPath);

            var generator = new Mono.TextTemplating.TemplateGenerator();
            if (generator.ProcessTemplate(ctx.assetPath, src, ref outputFilePath, out string dst))
            {
                if (!RemoveIfEmptyGeneration || dst.Length > 0)
                {
                    File.WriteAllText(outputFilePath, dst);
                }

                Once(() =>
                {
                    // TODO(bengreenier): is there a less expensive way to force it to reload scripts/appdomain?
                    AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
                });
            }

            var asset = new TextAsset(src);
            ctx.AddObjectToAsset("text", asset);
            ctx.SetMainObject(asset);
        }

        private static void Once(Action cb)
        {
            onces.Enqueue(cb);
            EditorApplication.update += OnceRunner;
        }

        private static readonly Queue<Action> onces = new Queue<Action>();

        private static void OnceRunner()
        {
            while (onces.Count > 0)
            {
                onces.Dequeue()();
            }

            EditorApplication.update -= OnceRunner;
        }
    }
}