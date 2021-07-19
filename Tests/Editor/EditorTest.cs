using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace FasterGames.T4.Editor.Tests
{
    public class EditorTest
    {
        // A Test behaves as an ordinary method
        [Test]
        public void EditorTestSimplePasses()
        {
            // Use the Assert class to test conditions
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator EditorTestWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}