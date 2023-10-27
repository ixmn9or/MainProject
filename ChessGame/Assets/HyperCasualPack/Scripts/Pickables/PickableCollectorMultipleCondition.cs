using System;
using System.Collections;
using System.Collections.Generic;
using HyperCasualPack.ScriptableObjects.Pools;
using UnityEngine;

namespace HyperCasualPack.Pickables
{
    public class PickableCollectorMultipleCondition : PickableCollectorBase
    {
        public event Action<PickablePoolerSO, int> Collected;
        public event Action Produced;

        public PickableCollectorMultipleConditionRuleset Ruleset;
        
        [SerializeField] Transform _modifiedStockpilePoint;
        [SerializeField, Range(0f, 20f)] float _stockpileEveryXSec;
        [SerializeField, Range(0f, 20f)] float _jumpHeight;

        Dictionary<PickablePoolerSO, int> _neededResources;
        float _currentModifiedItemTimer;
        float _currentStockpilingTimer;
        int _workSpeedMultiplier;

        void Awake()
        {
            SpawnedPickables = new Stack<Pickable>();
            _neededResources = new Dictionary<PickablePoolerSO, int>();
            foreach (TypeCountPair rulesetTypeCountPair in Ruleset.TypeCountPairs)
            {
                _neededResources.Add(rulesetTypeCountPair.PickablePooler, rulesetTypeCountPair.Count);
            }
        }
        
        public override PickablePoolerSO GetPool()
        {
            return Ruleset.OutputPooler;
        }

        protected override IEnumerator CollectFromPlayer(InventoryManager inventoryManager)
        {
            while (true)
            {
                if (_currentStockpilingTimer >= _stockpileEveryXSec)
                {
                    if (GetNeededPickable(inventoryManager, out PickablePoolerSO neededPickablePool))
                    {
                        if (inventoryManager.TakePickable(neededPickablePool, out Pickable pickable))
                        {
                            pickable.JumpToDisappearIntoPool(transform, _jumpHeight, 1, JumpDuration);
                            _neededResources[pickable.Pool] -= 1;
                            Collected?.Invoke(neededPickablePool, _neededResources[pickable.Pool]);
                            if (_neededResources[pickable.Pool] <= 0)
                            {
                                ProduceOutput();
                            }

                            _currentStockpilingTimer = 0f;
                        }
                    }
                }
                else
                {
                    _currentStockpilingTimer += Time.deltaTime;
                }

                yield return null;
            }
        }

        void ResetNeededResources()
        {
            foreach (TypeCountPair rulesetTypeCountPair in Ruleset.TypeCountPairs)
            {
                _neededResources[rulesetTypeCountPair.PickablePooler] = rulesetTypeCountPair.Count;
            }
        }

        bool GetNeededPickable(InventoryManager inventoryBase, out PickablePoolerSO poolerSo)
        {
            foreach (var neededResource in _neededResources)
            {
                if (neededResource.Value > 0)
                {
                    if (inventoryBase.ContainsPickable(neededResource.Key))
                    {
                        poolerSo = neededResource.Key;
                        return true;
                    }
                }
            }

            poolerSo = null;
            return false;
        }

        void ProduceOutput()
        {
            foreach (var neededResource in _neededResources)
            {
                if (neededResource.Value > 0)
                {
                    return;
                }
            }

            Pickable p = Ruleset.OutputPooler.TakeFromPool();
            p.transform.position = transform.position;
            JumpOrganized(p, _modifiedStockpilePoint, SpawnedPickables.Count);
            SpawnedPickables.Push(p);
            ResetNeededResources();
            Produced?.Invoke();
        }
    }
}