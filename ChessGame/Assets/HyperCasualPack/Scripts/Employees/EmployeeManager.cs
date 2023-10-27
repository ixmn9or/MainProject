using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace HyperCasualPack.Employees
{
    public class EmployeeManager : MonoBehaviour
    {
        [SerializeField] EmployeeMover _employeeMoverPrefab;
        
        List<EmployeeMover> _activeEmployees = new List<EmployeeMover>();

        public void AssignEmployee(Vector3 loadPoint, Vector3 unloadPoint, float loadTime, float unloadTime, Vector3 instantiatingPoint)
        {
            var employee = Instantiate(_employeeMoverPrefab, instantiatingPoint, Quaternion.identity);
            employee.Initialize(loadPoint, unloadPoint, loadTime, unloadTime);
            _activeEmployees.Add(employee);
        }

        [Button]
        public void PauseAll()
        {
            foreach (EmployeeMover activeEmployee in _activeEmployees)
            {
                activeEmployee.Pause();
            }
        }

        [Button]
        public void ResumeAll()
        {
            foreach (EmployeeMover activeEmployee in _activeEmployees)
            {
                activeEmployee.Resume();
            }
        }
    }
}