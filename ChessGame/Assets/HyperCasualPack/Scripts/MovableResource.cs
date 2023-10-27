using System;
using DG.Tweening;
using HyperCasualPack.ScriptableObjects;
using HyperCasualPack.ScriptableObjects.Pools;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace HyperCasualPack
{
    public class MovableResource : MonoBehaviour
	{
		[SerializeField] Image _resourceImage;
		[SerializeField] ResourceData _resourceData;


		public void PlayMoveSequenceFromWorldToUI(RuntimeResourceUIVariable scoreUIParentObject, Camera cam,
		                                          Vector3 startingWorldPos, float visualizationDuration, MovableResourcePoolerSO smPool,
		                                          Action<int> onSequenceEnded, int resourceAmount)
		{
			Transform trans = transform;
			trans.SetParent(scoreUIParentObject.RuntimeValue.transform);
			trans.position = cam.WorldToScreenPoint(startingWorldPos);
			trans.localScale = Vector3.zero;

			Sequence s = CreateAndGetScoreMovableSequence(scoreUIParentObject, visualizationDuration, smPool,
				onSequenceEnded, resourceAmount);
		}

		Sequence CreateAndGetScoreMovableSequence(RuntimeResourceUIVariable scoreUIObject, float visualizationDuration,
		                                          MovableResourcePoolerSO smPool, Action<int> onSequenceEnded, int resourceAmount)
		{
			Sequence sequence = DOTween.Sequence().SetRecyclable();
			sequence.Append(transform.DOMove(transform.position + (Vector3)Random.insideUnitCircle * 100,
				visualizationDuration / 2f));
			sequence.Join(transform.DOScale(Vector3.one, visualizationDuration / 2f));
			sequence.Append(transform.DOScale(Vector3.one / 2f, visualizationDuration).SetEase(Ease.InBack, 5f));
			sequence.Join(transform.DOLocalMove(Vector3.zero, visualizationDuration).SetEase(Ease.InQuart));
			sequence.AppendCallback(() =>
			{
				transform.localScale = Vector3.one;
				smPool.PutBackToPool(this);

				scoreUIObject.RuntimeValue.PlayResourceImageEffect();
				onSequenceEnded.Invoke(resourceAmount);
			});
			return sequence;
		}

		void Reset()
		{
			_resourceImage.sprite = _resourceData.Sprite;
		}
	}
}
