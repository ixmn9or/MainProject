using System;
using HyperCasualPack.ScriptableObjects.Pools;

namespace HyperCasualPack.Pickables
{
    [Serializable]
    public struct TypeCountPair
    {
        public PickablePoolerSO PickablePooler;
        public int Count;
    }
}