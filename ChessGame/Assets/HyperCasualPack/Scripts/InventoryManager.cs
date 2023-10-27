using System;
using HyperCasualPack.Pickables;
using HyperCasualPack.ScriptableObjects.Pools;
using UnityEngine;
using Random = UnityEngine.Random;

namespace HyperCasualPack
{
	public class InventoryManager : MonoBehaviour
	{
		[SerializeField] InventoryInvisible _inventoryInvisible;
		[SerializeField] InventoryVisible _inventoryVisible;
		
		IInteractor _interactor;

		public event Action<PickablePoolerSO> PickableAdded;
		public event Action<PickablePoolerSO> PickableRemoved;

		public bool IsInteractable => _interactor.IsInteractable;

		void Awake()
		{
			_interactor = GetComponent<IInteractor>();
			if (_interactor == null)
			{
				_interactor = new InteractionAllower();
			}
		}

		void OnEnable()
		{
            _inventoryVisible.PickableAdded += OnPickableAdded;
            _inventoryVisible.PickableRemoved += OnPickableRemoved;
			_inventoryInvisible.PickableAdded += OnPickableAdded;
			_inventoryInvisible.PickableRemoved += OnPickableRemoved;
		}

        void OnDisable()
        {
            _inventoryVisible.PickableAdded -= OnPickableAdded;
            _inventoryVisible.PickableRemoved -= OnPickableRemoved;
            _inventoryInvisible.PickableAdded -= OnPickableAdded;
            _inventoryInvisible.PickableRemoved -= OnPickableRemoved;
        }

        void OnPickableRemoved(PickablePoolerSO pool)
        {
	        PickableRemoved?.Invoke(pool);
        }

        void OnPickableAdded(PickablePoolerSO pool)
        {
	        PickableAdded?.Invoke(pool);
        }

        public void AddPickable(Pickable pickable)
		{
			if (pickable.PickableData.IsVisible)
			{
				_inventoryVisible.AddPickable(pickable);
			}
			else
			{
				_inventoryInvisible.AddPickable(pickable);
			}
		}

		public bool CanTakePickable(PickablePoolerSO pool)
		{
			if (!_interactor.IsInteractable)
			{
				return false;
			}

			bool result = pool.PeekData().IsVisible ? _inventoryVisible.CanTakePickable(pool) : _inventoryInvisible.CanTakePickable(pool);
            return result;
		}

		public bool TakePickable(PickablePoolerSO p, out Pickable pickable)
		{
			if (p.PeekData().IsVisible)
			{
				return _inventoryVisible.TryTakePickable(p, out pickable);
			}
			
			return _inventoryInvisible.TryTakePickable(p, out pickable);
		}

		public bool ContainsPickable(PickablePoolerSO poolerSO)
		{
			return poolerSO.PeekData().IsVisible ? _inventoryVisible.ContainsPickable(poolerSO) : _inventoryInvisible.ContainsPickable(poolerSO);
		}

		bool TakeRandomInvisiblePickable(out Pickable pickable)
		{
			return _inventoryInvisible.TakeRandomPickable(out pickable);
		}

		bool TakeRandomVisiblePickable(out Pickable pickable)
		{
			return _inventoryVisible.TakeRandomPickable(out pickable);
		}

		public bool TakeRandomPickable(out Pickable pickable)
		{
			if (!_inventoryVisible.IsEmpty() && !_inventoryInvisible.IsEmpty())
			{
				if (Random.value > 0.5f)
				{
					return TakeRandomInvisiblePickable(out pickable);
				}
			
				return TakeRandomVisiblePickable(out pickable);
			}
			if (!_inventoryVisible.IsEmpty())
			{
				return TakeRandomVisiblePickable(out pickable);
			}
			if (!_inventoryInvisible.IsEmpty())
			{
				return TakeRandomInvisiblePickable(out pickable);
			}

			pickable = null;
			return false;
		}
	}
}
