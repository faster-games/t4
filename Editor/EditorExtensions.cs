using System.IO;
using UnityEditor;
using UnityEngine;

namespace FasterGames.T4.Editor
{
    /// <summary>
    /// Editor extensions to add TT creation to right click menus
    /// </summary>
    public static class EditorExtensions
    {
        /// <summary>
        /// Asset menu to create TT (.tt) files
        /// </summary>
        [MenuItem("Assets/Create/T4 Template", priority = 100)]
        public static void CreateTTFile()
        {
            var assetName = CreateSafeName(Path.Combine(EditorUtils.GetCurrentAssetDirectory(), "textTemplate.tt"));
            File.Create(assetName).Dispose();
            
            AssetDatabase.ImportAsset(assetName);
        }

        /// <summary>
        /// Asset menu to create TTInclude (.ttinclude) files
        /// </summary>
        [MenuItem("Assets/Create/T4 Template Include", priority = 101)]
        public static void CreateTTIncludeFile()
        {
            var assetName = CreateSafeName(Path.Combine(EditorUtils.GetCurrentAssetDirectory(), "textTemplateInclude.ttinclude"));
            File.Create(assetName).Dispose();
            AssetDatabase.ImportAsset(assetName);
        }

        /// <summary>
        /// Creates a filename that isn't currently in use
        /// </summary>
        /// <param name="desiredName">the name you desire</param>
        /// <returns>the name that's available</returns>
        private static string CreateSafeName(string desiredName)
        {
            var name = desiredName;
            var i = 1;
            while (File.Exists(name))
            {
                name = Path.Combine(Path.GetDirectoryName(desiredName), $"{Path.GetFileNameWithoutExtension(desiredName)}{i}{Path.GetExtension(desiredName)}");
                i++;
            }

            return name;
        }
    }
}