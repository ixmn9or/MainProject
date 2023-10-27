using System.Collections;
using System.Collections.Generic;
using HyperCasualPack.ScriptableObjects.Pools;
using UnityEngine;

namespace HyperCasualPack.Pickables
{
    public class PickableSourceSpawner : PickableCollectorBase, IUpgradable
    {
        [SerializeField] PickablePoolerSO _poolerSO;
        [SerializeField] Transform _stackingPoint;
        [SerializeField, Range(0.02f, 20f)] float _oreExtractionSec;
        [SerializeField] float _pickingTime = 0.15f;

        WaitForSeconds _waitForSeconds;
        float _currentTimer;
        int _workSpeedMultiplier;
        
        void Awake()
        {
            SpawnedPickables = new Stack<Pickable>();
            _waitForSeconds = new WaitForSeconds(_pickingTime);
        }

        void Update()
        {
            if (SpawnedPickables.Count < RowColumnHeight.HeightCount * RowColumnHeight.RowCount * RowColumnHeight.ColumnCount)
            {
                _currentTimer += Time.deltaTime;

                if (_currentTimer >= _oreExtractionSec / (1 + _workSpeedMultiplier))
                {
                    Spawn();
                    _currentTimer = 0f;
                }
            }
        }

        public void Upgrade()
        {
            _workSpeedMultiplier++;
        }
        
        protected override IEnumerator CollectFromPlayer(InventoryManager inventoryManager)
        {
            while (true)
            {
                if (!inventoryManager.CanTakePickable(GetPool()))
                {
                    yield return null;
                    continue;
                }
                    
                Pickable pickable = GetPickable();
                if (pickable)
                {
                    inventoryManager.AddPickable(pickable);
                }
                
                yield return _waitForSeconds;
            }
        }

        public override PickablePoolerSO GetPool()
        {
            return _poolerSO;
        }
        
        void Spawn()
        {
            Pickable pickable = _poolerSO.TakeFromPool();
            pickable.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
            
            JumpOrganized(pickable, _stackingPoint, SpawnedPickables.Count);
            SpawnedPickables.Push(pickable);
        }
    }
}