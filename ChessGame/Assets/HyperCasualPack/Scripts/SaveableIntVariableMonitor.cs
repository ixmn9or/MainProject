using HyperCasualPack.ScriptableObjects;
using TMPro;
using UnityEngine;

namespace HyperCasualPack
{
	public class SaveableIntVariableMonitor : MonoBehaviour
	{
		[SerializeField] SaveableRuntimeIntVariable _monitorVariable;
		[SerializeField] TextMeshProUGUI _monitorText;

		void OnEnable()
		{
			_monitorVariable.ValueChanged += MonitorVariableOnValueChanged;
		}
		
		void OnDisable()
		{
			_monitorVariable.ValueChanged -= MonitorVariableOnValueChanged;
		}
		
		public void Initialize(SaveableRuntimeIntVariable intVariable)
		{
			_monitorVariable = intVariable;
		}

		void MonitorVariableOnValueChanged(int obj)
		{
			_monitorText.text = obj.ToString();
		}
	}
}
