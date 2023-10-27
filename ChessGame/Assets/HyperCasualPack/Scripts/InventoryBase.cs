#define ENABLE_LOGS

using System;
using System.Collections.Generic;
using HyperCasualPack.Pickables;
using HyperCasualPack.ScriptableObjects.Pools;
using UnityEngine;

namespace HyperCasualPack
{
    [Serializable]
    public abstract class InventoryBase
    {
        [SerializeField] protected Transform stackingPoint;

        protected Dictionary<PickablePoolerSO, Stack<Pickable>> Pickables { get; set; } = new Dictionary<PickablePoolerSO, Stack<Pickable>>();
        protected int pickableIndex;

        public event Action<PickablePoolerSO> PickableAdded;
        public event Action<PickablePoolerSO> PickableRemoved;

        public bool IsEmpty()
        {
            return pickableIndex <= 0;
        }

        public bool TakeRandomPickable(out Pickable pickable)
        {
            foreach (var pickable1 in Pickables)
            {
                if (pickable1.Value.Count > 0)
                {
                    TakePickable(out pickable, pickable1.Value);
                    return true;
                }
            }

            pickable = null;
            return false;
        }

        public bool CanTakePickable(PickablePoolerSO pool)
        {
            AddKey(pool);
            bool canTake = !IsCapacityFull(pool);
            return canTake;
        }

        public bool TryTakePickable(PickablePoolerSO p, out Pickable pickable)
        {
            if (Pickables.ContainsKey(p) && Pickables[p].Count > 0)
            {
                TakePickable(out pickable, Pickables[p]);
                return true;
            }

            pickable = null;
            return false;
        }

        public bool ContainsPickable(PickablePoolerSO pool)
        {
            if (!Pickables.ContainsKey(pool))
            {
                return false;
            }
            
            if (Pickables[pool].Count > 0)
            {
                return true;
            }

            return false;
        }

        public void AddPickable(Pickable p)
        {
            MovePickable(p);
            Pickables[p.Pool].Push(p);
            pickableIndex++;
            PickableAdded?.Invoke(p.Pool);
        }

        protected abstract void MovePickable(Pickable pickable);
        protected abstract bool IsCapacityFull(PickablePoolerSO p);
        
        void AddKey(PickablePoolerSO p)
        {
            if (Pickables.ContainsKey(p))
            {
                return;
            }

            Pickables.Add(p, new Stack<Pickable>());
        }

        void TakePickable(out Pickable pickable, Stack<Pickable> pickableStack)
        {
            pickable = pickableStack.Pop();
            pickable.transform.SetParent(null);
            pickable.gameObject.SetActive(true);
            pickableIndex--;
            PickableRemoved?.Invoke(pickable.Pool);
        }
    }
}