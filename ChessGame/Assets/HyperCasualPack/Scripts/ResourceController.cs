using System;
using HyperCasualPack.ScriptableObjects.Pools;
using UnityEngine;

namespace HyperCasualPack
{
    public class ResourceController : MonoBehaviour
    {
        [SerializeField] PickableResourceManager _pickableResourceManager;
        [SerializeField] InventoryManager _inventoryManager;

        void OnEnable()
        {
            _inventoryManager.PickableAdded += InventoryManager_PickableAdded;
            _inventoryManager.PickableRemoved += InventoryManager_PickableRemoved;
        }

        void OnDisable()
        {
            _inventoryManager.PickableAdded -= InventoryManager_PickableAdded;
            _inventoryManager.PickableRemoved -= InventoryManager_PickableRemoved;
        }

        void InventoryManager_PickableAdded(PickablePoolerSO obj)
        {
            _pickableResourceManager.AddScore(obj, 1);
        }

        void InventoryManager_PickableRemoved(PickablePoolerSO obj)
        {
            _pickableResourceManager.AddScore(obj, -1);
        }
    }
}