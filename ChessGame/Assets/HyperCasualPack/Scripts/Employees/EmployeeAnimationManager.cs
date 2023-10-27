using UnityEngine;

namespace HyperCasualPack.Employees
{
    [RequireComponent(typeof(EmployeeMover))]
    public class EmployeeAnimationManager : MonoBehaviour
    {
        [SerializeField] Animator _animator;

        EmployeeMover _employeeMover;
        
        int moveId = Animator.StringToHash("Move");
        
        void Awake()
        {
            _employeeMover = GetComponent<EmployeeMover>();
            _employeeMover.StartedMoving += EmployeeMover_StartedMoving;
            _employeeMover.StoppedMoving += EmployeeMover_StoppedMoving;
        }

        void EmployeeMover_StartedMoving()
        {
            UpdateLocomotion(1f);
        }

        void EmployeeMover_StoppedMoving()
        {
            UpdateLocomotion(0f);
        }

        void UpdateLocomotion(float value)
        {
            _animator.SetFloat(moveId, value);
        }
    }
}