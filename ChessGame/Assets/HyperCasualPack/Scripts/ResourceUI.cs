using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace HyperCasualPack
{
	public class ResourceUI : MonoBehaviour
	{
		public Image ResourceImageScalable;

		[SerializeField] RuntimeResourceUIVariable _runtimeResourceUIVariable;

		Tween _tween;

		void Awake()
		{
			_tween = ResourceImageScalable.transform.DOPunchScale(Vector3.one * 0.7f, 0.3f, 10, 0f).Pause().SetAutoKill(false);
		}

		void OnEnable()
		{
			_runtimeResourceUIVariable.RuntimeValue = this;
		}

		void OnDisable()
		{
			_runtimeResourceUIVariable.RuntimeValue = null;
		}

		public void Initialize(Sprite sprite)
		{
			ResourceImageScalable.sprite = sprite;
		}

		public void PlayResourceImageEffect()
		{
			_tween.Restart();
		}
	}
}
