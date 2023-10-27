using HyperCasualPack.Pickables;
using NaughtyAttributes;
using UnityEngine;

namespace HyperCasualPack.Employees
{
    public class EmployeeAssigner : MonoBehaviour
    {
        [SerializeField] EmployeeManager _employeeManager;
        [SerializeField] Transform _instantiatingPoint;
        [SerializeField] Vector3 _loadPoint;
        [SerializeField] Vector3 _unloadTarget;
        [SerializeField] float _loadTime = 2f;
        [SerializeField] float _unloadTime = 2f;

        [Button]
        public void Assign()
        {
            _employeeManager.AssignEmployee(_loadPoint, _unloadTarget, _loadTime, _unloadTime, _instantiatingPoint.position);
        }
    }
}