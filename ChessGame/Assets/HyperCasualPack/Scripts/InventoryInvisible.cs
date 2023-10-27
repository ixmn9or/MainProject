using System;
using DG.Tweening;
using HyperCasualPack.Pickables;
using HyperCasualPack.ScriptableObjects.Pools;
using UnityEngine;

namespace HyperCasualPack
{
    [Serializable]
	public class InventoryInvisible : InventoryBase
	{
		[SerializeField] int _capacity;

		protected override void MovePickable(Pickable pickable)
		{
			Transform trans = pickable.transform;
			trans.DOKill();
			trans.SetParent(stackingPoint);
			trans.DOLocalRotate(Vector3.zero, 0.5f).SetRecyclable();
			trans.DOLocalMove(Vector3.zero, 0.5f).SetEase(Ease.InBack, 2f).SetRecyclable().OnComplete(() =>
			{
				pickable.gameObject.SetActive(false);
			});
		}
		
		protected override bool IsCapacityFull(PickablePoolerSO p)
		{
			return pickableIndex >= _capacity;
		}
	}
}
