using System.Collections;
using UnityEngine;

namespace HyperCasualPack.Pickables
{
    /// <summary>
    /// Gives Pickable to the InventoryManager OnTrigger, one InventoryManager at a time. Multiple InventoryManager can't collect simultaneously.
    /// </summary>
    public class PickableSourceCollector : MonoBehaviour
    {
        [SerializeField] PickableCollectorBase _pickableCollectorBase;
        [SerializeField, Range(0f, 2f)] float _pickTime;

        WaitForSeconds _waitForSeconds;
        InventoryManager _inventoryManager;
        Coroutine _coroutine;

        void Awake()
        {
            _waitForSeconds = new WaitForSeconds(_pickTime);
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out InventoryManager inventoryManager))
            {
                if (_coroutine == null)
                {
                    _coroutine = StartCoroutine(Co_Collect(inventoryManager));
                }
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out InventoryManager inventoryManager))
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }
        }

        IEnumerator Co_Collect(InventoryManager inventory)
        {
            while (true)
            {
                if (!inventory.CanTakePickable(_pickableCollectorBase.GetPool()))
                {
                    yield return null;
                    continue;
                }
                    
                Pickable pickable = _pickableCollectorBase.GetPickable();
                if (pickable)
                {
                    inventory.AddPickable(pickable);
                }
                
                yield return _waitForSeconds;
            }
        }
    }
}