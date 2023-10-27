using HyperCasualPack.ScriptableObjects.Pools;
using NaughtyAttributes;
using System.Collections;
using UnityEngine;

namespace HyperCasualPack.Pickables
{
	public abstract class PickableSellerBase : MonoBehaviour
	{
		[SerializeField, Tooltip("Selling jupm duration"), Range(0f, 10f)] float _jumpDuration;
		[SerializeField, Tooltip("Sold object jump height"), Range(0f, 10f)] float _jumpHeight;
		[SerializeField, Tooltip("Sell count per second"), Range(0f, 20f)] float _sellRate;

        [SerializeField] bool _sellRandom;
        [SerializeField, HideIf(nameof(_sellRandom))] PickablePoolerSO _pickableToSell;

		WaitForSeconds _waitForSeconds;
		Coroutine _cor;

		void Awake()
		{
			_waitForSeconds = new WaitForSeconds(1f / _sellRate);
		}

		void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out InventoryManager inventory))
			{
				_cor = StartCoroutine(CollectFromPlayer(inventory));
			}
		}

		void OnTriggerExit(Collider other)
		{
			if (other.TryGetComponent(out InventoryManager _) && _cor != null)
			{
				StopCoroutine(_cor);
			}
		}

		IEnumerator CollectFromPlayer(InventoryManager inventoryManager)
		{
			if (_sellRandom)
			{
                while (inventoryManager.TakeRandomPickable(out Pickable pickable))
                {
                    PlaySequence(pickable);
                    yield return _waitForSeconds;
                }
			}
			else
			{
                while (inventoryManager.TakePickable(_pickableToSell, out Pickable pickable))
                {
                    PlaySequence(pickable);
                    yield return _waitForSeconds;
                }
            }
		}
		
		void PlaySequence(Pickable pickableItem)
        {
            pickableItem.transform.Jump(transform, _jumpHeight, 1, _jumpDuration, () => OnJump(pickableItem));
        }

		protected abstract void OnJump(Pickable pickable);
	}

}
