using UnityEngine;

namespace HyperCasualPack.Pickables
{
	public class PickableSellerFloatingImage : PickableSellerBase
	{
		[SerializeField] ResourceMonitorVisualizer _scoreMonitorVisualizer;
		
		protected override void OnJump(Pickable pickable)
		{
			pickable.ReleasePool();
			_scoreMonitorVisualizer.PlayVisualization(pickable.transform.position, pickable.GetSellValue());
		}
	}
}
