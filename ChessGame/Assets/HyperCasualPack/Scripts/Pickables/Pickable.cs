using HyperCasualPack.ScriptableObjects.Pools;
using UnityEngine;

namespace HyperCasualPack.Pickables
{
    public class Pickable : MonoBehaviour
    {
        [SerializeField] PickableData _pickableData;
        Vector3 _defaultLocalScale;
        public PickablePoolerSO Pool { get; set; }

        public PickableData PickableData => _pickableData;
        public Vector3 DefaultScale => _defaultLocalScale;

        void Awake()
        {
            _defaultLocalScale = transform.localScale;
        }

        public int GetSellValue()
        {
            return _pickableData.SellValue;
        }

        public void ReleasePool()
        {
            ResetState();
            Pool.PutBackToPool(this);
        }

        void ResetState()
        {
            transform.localScale = _defaultLocalScale;
            transform.Clear();
        }
    }
}