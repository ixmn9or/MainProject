using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using HyperCasualPack.ScriptableObjects.Pools;
using UnityEngine;

namespace HyperCasualPack.Pickables
{
    public abstract class PickableCollectorBase : MonoBehaviour
    {
        [SerializeField] protected RowColumnHeight RowColumnHeight;
        [SerializeField, Range(0.01f, 2f)] protected float JumpDuration;

        protected Stack<Pickable> SpawnedPickables;
        
        Dictionary<InventoryManager, Coroutine> _coroutineDictionary = new Dictionary<InventoryManager, Coroutine>();
        
        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out InventoryManager inventory))
            {
                _coroutineDictionary.Add(inventory, StartCoroutine(CollectFromPlayer(inventory)));
            }
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out InventoryManager inventory))
            {
                StopCoroutine(_coroutineDictionary[inventory]);
                _coroutineDictionary.Remove(inventory);
            }
        }

        protected abstract IEnumerator CollectFromPlayer(InventoryManager inventoryManager);

        protected void JumpOrganized(Pickable pickableItem, Transform pivotPoint, int index)
        {
            JumpOrganizedData jumpOrganizedData = new JumpOrganizedData
            {
                Duration = JumpDuration,
                Index = index,
                Item = pickableItem.transform,
                RowColumnHeight = RowColumnHeight,
                PivotPoint = pivotPoint
            };
            ArcadeIdleHelper.JumpOrganized(jumpOrganizedData);
        }

        protected void JumpOrganized(Pickable pickableItem, Transform pivotPoint, int index, TweenCallback tweenCallback)
        {
            JumpOrganizedData jumpOrganizedData = new JumpOrganizedData
            {
                Duration = JumpDuration,
                Index = index,
                Item = pickableItem.transform,
                RowColumnHeight = RowColumnHeight,
                PivotPoint = pivotPoint
            };
            ArcadeIdleHelper.JumpOrganized(jumpOrganizedData, tweenCallback);
        }

        public abstract PickablePoolerSO GetPool();

        public Pickable GetPickable()
        {
            return SpawnedPickables.Count > 0 ? SpawnedPickables.Pop() : null;
        }
    }
}