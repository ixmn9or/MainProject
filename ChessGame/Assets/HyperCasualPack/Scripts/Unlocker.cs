using System.Collections;
using DG.Tweening;
using HyperCasualPack.ScriptableObjects.Pools;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HyperCasualPack
{
    public class Unlocker : MonoBehaviour
	{
		[SerializeField] UnityEvent _onUnlocked;
		[SerializeField] TextMeshProUGUI _resourceCountText;
		[SerializeField] Image _progressBar;
		[SerializeField] PickableResourcePoolerSO _neededResource;
		[SerializeField] Ease _spendingEase;
		[SerializeField, Range(0f, 100f)] float _spendingSpeed;
		[SerializeField, Min(0)] int _requiredResource;
		[SerializeField] bool _canWorkMultipleTimes;

		InventoryManager _inventoryManager;
		ResourceSpender _resourceSpender;
		Tween _spendingTween;
		Coroutine _cor;
		WaitForSeconds _waitForSeconds;
		int _previousResourceSpentAmount;
		int _collectedResource;
		
		void Awake() 
		{
			_waitForSeconds = new WaitForSeconds(0.1f);
		}
		
		void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out InventoryManager inventory) && other.TryGetComponent(out ResourceSpender resourceSpender))
			{
				_inventoryManager = inventory;
				_resourceSpender = resourceSpender;
				_cor = StartCoroutine(CheckInventory(inventory));
			}
		}

		void OnTriggerExit(Collider other)
		{
			if (other.TryGetComponent(out InventoryManager _) && other.TryGetComponent(out ResourceSpender resourceSpender))
			{
				StopSpending();
				_inventoryManager = null;
				_resourceSpender = null;
			}
		}

		void OnValidate()
		{
			_resourceCountText.text = _requiredResource.ToString();
		}
		
		public void SetRequiredResource(int requiredResource)
		{
			_collectedResource = 0;
			_previousResourceSpentAmount = 0;
			_progressBar.fillAmount = 0;
			_requiredResource = requiredResource;
			_resourceCountText.text = _requiredResource.ToString();
		}

		IEnumerator CheckInventory(InventoryManager inventoryManager)
		{
			while (true)
			{
				if (!inventoryManager.IsInteractable)
				{
					yield return _waitForSeconds;
					continue;
				}
			
				int resourceAmount = _resourceSpender.GetRuntimeIntValue(_neededResource);
				ArcadeIdleHelper.SpendResource(_requiredResource, _collectedResource, resourceAmount, out _spendingTween, _spendingSpeed, _spendingEase, SpendMoney);
				yield break;
			}
		}

		void SpendMoney(int x)
		{
			int decreasingAmountDelta = x - _previousResourceSpentAmount;
			_resourceSpender.Spend(_neededResource, decreasingAmountDelta, transform);
			_collectedResource += decreasingAmountDelta;
			_resourceCountText.text = (_requiredResource - _collectedResource).ToString();
			_previousResourceSpentAmount = x;
			_progressBar.fillAmount = (float)_collectedResource / _requiredResource;
			
			
			if (_collectedResource == _requiredResource)
			{
				_onUnlocked?.Invoke();
				StopSpending();

				if (_canWorkMultipleTimes)
				{
					_cor = StartCoroutine(CheckInventory(_inventoryManager));
				}
				else
				{
					gameObject.SetActive(false);
				}
			}
		}
		
		void StopSpending()
		{
			_spendingTween?.Kill();
			if (_cor != null)
			{
				StopCoroutine(_cor);				
			}
		}
	}
}