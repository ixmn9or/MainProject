using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

namespace HyperCasualPack.Employees
{
    /// <summary>
    /// Tracks PickableSourceCollectors, if there is pickable, goes there and waits for X second, picks pickable and carries them to the target place and waits in there for Y second.
    /// </summary>
    public class EmployeeMover : MonoBehaviour, IInteractor
    {
        [SerializeField] NavMeshAgent _navMeshAgent;
        
        Vector3 _loadPoint;
        Vector3 _unloadPoint;
        float _loadTime;
        float _unloadTime;

        bool _isCurrentDestinationLoad;

        public bool IsInteractable => !enabled;
        public event Action StartedMoving;
        public event Action StoppedMoving;

        void Update()
        {
            if (_navMeshAgent.remainingDistance < 0.1f)
            {
                if (_isCurrentDestinationLoad)
                {
                    DOVirtual.DelayedCall(_loadTime, Resume, false);
                }
                else
                {
                    DOVirtual.DelayedCall(_unloadTime, Resume, false);
                }
                _isCurrentDestinationLoad = !_isCurrentDestinationLoad;
                StoppedMoving?.Invoke();
                enabled = false;
            }
        }

        public void Initialize(Vector3 loadPoint, Vector3 unloadPoint, float loadTime, float unloadTime)
        {
            _loadPoint = loadPoint;
            _unloadPoint = unloadPoint;
            _loadTime = loadTime;
            _unloadTime = unloadTime;
            _isCurrentDestinationLoad = true;
            
            Resume();
        }

        public void Pause()
        {
            _navMeshAgent.ResetPath();
            StoppedMoving?.Invoke();
            enabled = false;
        }

        public void Resume()
        {
            enabled = true;
            if (_isCurrentDestinationLoad)
            {
                _navMeshAgent.SetDestination(_loadPoint);
            }
            else
            {
                _navMeshAgent.SetDestination(_unloadPoint);
            }
            StartedMoving?.Invoke();
        }
    }
}