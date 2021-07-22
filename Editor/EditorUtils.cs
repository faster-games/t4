using System;
using System.Collections.Generic;
using UnityEditor;
using Object = UnityEngine.Object;

namespace FasterGames.T4.Editor
{
    internal static class EditorUtils
    {
        /// <summary>
        /// <see cref="EditorApplication"/> task runner
        /// </summary>
        /// <remarks>
        /// This allows us to schedule work to be run once on Editor update
        /// </remarks>
        /// <param name="cb">task to run</param>
        public static void Once(Action cb)
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
        
        /// <summary>
        /// Get the current asset directory
        /// </summary>
        /// <remarks>
        /// From https://gist.github.com/allanolivei/9260107
        /// </remarks>
        /// <returns>directory</returns>
        public static string GetCurrentAssetDirectory()
        {
            foreach (var obj in Selection.GetFiltered<Object>(SelectionMode.Assets))
            {
                var path = AssetDatabase.GetAssetPath(obj);
                if (string.IsNullOrEmpty(path))
                    continue;

                if (System.IO.Directory.Exists(path))
                    return path;
                else if (System.IO.File.Exists(path))
                    return System.IO.Path.GetDirectoryName(path);
            }

            return "Assets";
        }
    }
}