using HyperCasualPack.ScriptableObjects;
using HyperCasualPack.ScriptableObjects.Pools;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace HyperCasualPack
{
    [CreateAssetMenu(menuName = "HyperCasualPack/Pickable Resource Manager", fileName = "Pickable Resource Manager", order = 0)]
    public class PickableResourceManager : ScriptableObject
    {
        [SerializeField] PoolIntVariablePair[] _poolIntVariablePair;
        readonly Dictionary<PickablePoolerSO, SaveableRuntimeIntVariable> _lookup = new Dictionary<PickablePoolerSO, SaveableRuntimeIntVariable>();

        void OnEnable()
        {
#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
            {
                return;
            }
#endif
            foreach (var item in _poolIntVariablePair)
            {
                _lookup.Add(item.Pool, item.CollectedAmountVariable);
            }
        }

        public void AddScore(PickablePoolerSO pool, int score)
        {
            if (_lookup.ContainsKey(pool))
            {
                _lookup[pool].RuntimeValue += score;
            }
        }
    }

    [Serializable]
    public struct PoolIntVariablePair
    {
        public PickablePoolerSO Pool;
        public SaveableRuntimeIntVariable CollectedAmountVariable;
    }
}