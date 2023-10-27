using System;
using DG.Tweening;
using HyperCasualPack.Pickables;
using HyperCasualPack.ScriptableObjects.Pools;
using UnityEngine;

namespace HyperCasualPack
{
    [Serializable]
	public class InventoryVisible : InventoryBase
	{
		[SerializeField] RowColumnHeight _rowColumnHeight;

		protected override void MovePickable(Pickable pickable)
		{
			Vector3 targetPos = ArcadeIdleHelper.GetPoint(pickableIndex, _rowColumnHeight);
			Transform trans = pickable.transform;
			trans.DOKill();
			trans.SetParent(stackingPoint);
			trans.DOLocalRotate(Vector3.zero, 0.5f).SetRecyclable();
			trans.DOLocalMove(targetPos, 0.5f).SetEase(Ease.InBack, 2f).SetRecyclable();
		}
		
		protected override bool IsCapacityFull(PickablePoolerSO p)
		{
			return pickableIndex >= _rowColumnHeight.GetCapacity();
		}
	}
}
