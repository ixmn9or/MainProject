using HyperCasualPack.ScriptableObjects;
using HyperCasualPack.ScriptableObjects.Pools;
using UnityEngine;

namespace HyperCasualPack
{
    [CreateAssetMenu(menuName = "HyperCasualPack/Resource Monitor Visualizer", fileName = "Resource Monitor Visualizer", order = 0)]
	public class ResourceMonitorVisualizer : ScriptableObject
	{
		[SerializeField] MovableResourcePoolerSO _movableResourcePool;
		[SerializeField] RuntimeResourceUIVariable _resourceUIParentObject;
		[SerializeField] SaveableRuntimeIntVariable _resource;
		[SerializeField, Range(0f, 5f)] float _visualizationDuration;

		Camera _camMain;

		public void PlayVisualization(Vector3 pickedItemWorldPos, int resourceAmount)
		{
			if (!_camMain) _camMain = Camera.main;

			MovableResource scoreMovableObject = _movableResourcePool.TakeFromPool();
			scoreMovableObject.PlayMoveSequenceFromWorldToUI(_resourceUIParentObject, _camMain, pickedItemWorldPos,
				_visualizationDuration, _movableResourcePool, OnMoveSequenceEnded, resourceAmount);
		}

		void OnMoveSequenceEnded(int resourceAmount)
		{
			_resource.RuntimeValue += resourceAmount;
		}
	}
}
