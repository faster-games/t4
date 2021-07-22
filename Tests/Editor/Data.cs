using System.Collections.Generic;
using UnityEngine;

namespace FasterGames.T4.Editor.Tests
{
    [CreateAssetMenu(menuName = "T4/Test Data/Example Data")]
    public class Data : ScriptableObject
    {
        public List<string> Animals;
    }
}