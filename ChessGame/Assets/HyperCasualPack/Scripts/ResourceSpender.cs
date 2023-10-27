using System;
using System.Collections.Generic;
using HyperCasualPack.Pickables;
using HyperCasualPack.ScriptableObjects;
using HyperCasualPack.ScriptableObjects.Pools;
using UnityEngine;

namespace HyperCasualPack
{
    public class ResourceSpender : MonoBehaviour
	{
		const int VISUAL_FEEDBACK_SPAWN_RATE_MAX = 40;
		[SerializeField] ResourceSpenderData[] _resourceSpenderDatas;
		[SerializeField, Tooltip("controls how frequent resource object will be shown"), Range(1, VISUAL_FEEDBACK_SPAWN_RATE_MAX)] int _visualFeedbackSpawnRate;
		[SerializeField, Range(0f, 10f)] float _jumpHeight;
		[SerializeField, Range(0f, 3f)] float _jumpDuration;

		Dictionary<PickableResourcePoolerSO, SaveableRuntimeIntVariable> _spendableResources;
		int _spawnCount;

		void Awake()
		{
			_spendableResources = new Dictionary<PickableResourcePoolerSO, SaveableRuntimeIntVariable>();
			foreach (ResourceSpenderData resourceSpenderData in _resourceSpenderDatas)
			{
				_spendableResources.Add(resourceSpenderData.SpendingResourcePool, resourceSpenderData.ResourceVariable);
			}
		}

		public void Spend(PickableResourcePoolerSO pickableResource, int amount, Transform moneyTargetPoint)
		{
			_spendableResources[pickableResource].RuntimeValue -= amount;
			if (amount == 0)
			{
				return;
			}
			_spawnCount++;
			if (_spawnCount >= VISUAL_FEEDBACK_SPAWN_RATE_MAX + 1 - _visualFeedbackSpawnRate)
			{
				PickableResource nonPickableResource = pickableResource.TakeFromPool();
				nonPickableResource.transform.position = transform.position;
				nonPickableResource.transform.Jump(moneyTargetPoint, _jumpHeight, 1, _jumpDuration, () =>
				{
					pickableResource.PutBackToPool(nonPickableResource);
				});
				_spawnCount = 0;
			}
		}

		public int GetRuntimeIntValue(PickableResourcePoolerSO p)
		{
			return _spendableResources[p].RuntimeValue;
		}
	}

	[Serializable]
	struct ResourceSpenderData
	{
		public PickableResourcePoolerSO SpendingResourcePool;
		public SaveableRuntimeIntVariable ResourceVariable;
	}
}
