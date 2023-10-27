using System.Collections.Generic;
using UnityEngine;

namespace HyperCasualPack.ScriptableObjects
{
    [CreateAssetMenu(menuName = "HyperCasualPack/Variables/Saveables List", fileName = "Saveables List", order = 0)]
    public class SaveablesList : ScriptableObject
    {
        [SerializeField, ContextMenuItem("Remove empty elements", nameof(RemoveEmptyElements))]
        List<SaveableSO> _saveables;

        public List<SaveableSO> Saveables => _saveables;

        void RemoveEmptyElements()
        {
            for (int i = _saveables.Count - 1; i > 0; i--)
            {
                SaveableSO item = _saveables[i];
                if (item == null)
                {
                    _saveables.Remove(item);
                }
            }
        }
    }
}