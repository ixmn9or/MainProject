using System.Collections;
using System.Collections.Generic;
using HyperCasualPack.ScriptableObjects.Pools;
using UnityEngine;

namespace HyperCasualPack.Pickables
{
    public class PickableCollectorStockpiler : PickableCollectorBase, IUpgradable
    {
        [SerializeField] PickableCollectorStockpilerRuleset _ruleset;
        [SerializeField] Transform _modifiedStockpilePoint;
        [SerializeField] Transform _unmodifiedStockpilePoint;
        [SerializeField, Tooltip("In seconds"), Range(0f, 10f)] float _modifyingTimePerItem;
        [SerializeField] float _stockpileEveryXSec;
        
        Stack<Pickable> _unmodifiedItems;
        float _currentModifiedItemTimer;
        float _currentStockpilingTimer;
        int _workSpeedMultiplier;

        bool IsUnmodifiedItemCapacityFull => _unmodifiedItems.Count >= RowColumnHeight.ColumnCount * RowColumnHeight.RowCount * RowColumnHeight.HeightCount;

        void Awake()
        {
            SpawnedPickables = new Stack<Pickable>();
            _unmodifiedItems = new Stack<Pickable>();
        }

        void Update()
        {
            ModifyItem();
        }

        void OnValidate()
        {
            JumpDuration = Mathf.Clamp(JumpDuration, 0f, Mathf.Min(_modifyingTimePerItem * 0.9f, _stockpileEveryXSec * 0.9f));
        }

        public void Upgrade()
        {
            _workSpeedMultiplier++;
        }
        
        protected override IEnumerator CollectFromPlayer(InventoryManager inventoryManager)
        {
            while (true)
            {
                if (IsUnmodifiedItemCapacityFull)
                {
                    yield return null;
                    continue;
                }

                if (_currentStockpilingTimer >= _stockpileEveryXSec)
                {
                    bool isFound = inventoryManager.ContainsPickable(_ruleset.InputPoolerSO);
                    if (isFound)
                    {
                        if (inventoryManager.TakePickable(_ruleset.InputPoolerSO, out Pickable pickableItem))
                        {
                            JumpOrganized(pickableItem, _unmodifiedStockpilePoint, _unmodifiedItems.Count);
                            _unmodifiedItems.Push(pickableItem);
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

        public override PickablePoolerSO GetPool()
        {
            return _ruleset.OutputPoolerSO;
        }

        Pickable ApplyModifying(Pickable previousPickable)
        {
            previousPickable.DisappearSlowlyToPool();
            Pickable p = _ruleset.OutputPoolerSO.TakeFromPool();
            p.transform.position = previousPickable.transform.position;
            return p;
        }

        void ModifyItem()
        {
            if (_unmodifiedItems.Count > 0)
            {
                if (_currentModifiedItemTimer >= _modifyingTimePerItem / (_workSpeedMultiplier + 1))
                {
                    Pickable pickableItemItem = _unmodifiedItems.Pop();
                    Pickable p = ApplyModifying(pickableItemItem);
                    JumpOrganized(p, _modifiedStockpilePoint, SpawnedPickables.Count);
                    SpawnedPickables.Push(p);
                    _currentModifiedItemTimer = 0f;
                }
                else
                {
                    _currentModifiedItemTimer += Time.deltaTime;
                }
            }
        }
    }
}