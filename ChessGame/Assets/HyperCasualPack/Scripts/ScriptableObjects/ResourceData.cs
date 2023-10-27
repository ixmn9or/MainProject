using HyperCasualPack.ScriptableObjects.Pools;
using UnityEngine;

namespace HyperCasualPack.ScriptableObjects
{
    [CreateAssetMenu(menuName = "HyperCasualPack/Resource Data", fileName = "Resource Data", order = 0)]
    public class ResourceData : ScriptableObject
    {
        public Sprite Sprite;
        public SaveableRuntimeIntVariable CollectedAmount;
    }
}