using UnityEngine;

namespace HyperCasualPack
{
	[CreateAssetMenu(fileName = "Runtime Resource UI Variable", menuName = "HyperCasualPack/Runtime Resource UI Variable",
		order = 0)]
	public class RuntimeResourceUIVariable : ScriptableObject
	{
		[SerializeField] ResourceUI _initialValue;
		public ResourceUI RuntimeValue { get; set; }

		void OnEnable()
		{
			ResetState();
		}

		public void ResetState()
		{
			RuntimeValue = _initialValue;
		}
	}
}
