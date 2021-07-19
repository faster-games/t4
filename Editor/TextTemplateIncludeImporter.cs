using System.IO;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace FasterGames.T4.Editor
{
    /// <summary>
    /// The TextTemplate include (.ttinclude) file importer for Unity3D
    /// </summary>
    [ScriptedImporter(1, "ttinclude")]
    public class TextTemplateIncludeImporter : ScriptedImporter
    {
        /// <inheritdoc />
        public override void OnImportAsset(AssetImportContext ctx)
        {
            // since this is just a supporting file, no generation is needed
            // however, we pull this in as text so it can show nicely in the editor
            var asset = new TextAsset(File.ReadAllText(ctx.assetPath));
            ctx.AddObjectToAsset("text", asset);
            ctx.SetMainObject(asset);
        }
    }
}