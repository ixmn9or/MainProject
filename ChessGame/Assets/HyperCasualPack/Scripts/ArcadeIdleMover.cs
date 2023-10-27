using HyperCasualPack.ScriptableObjects;
using HyperCasualPack.ScriptableObjects.Channels;
using UnityEngine;

namespace HyperCasualPack
{
    public class ArcadeIdleMover : MonoBehaviour, IInteractor
    {
        [SerializeField] Rigidbody _rbd;
        [SerializeField] MovementDataSO _movementDataSO;
        [SerializeField] InputChannelSO _inputChannelSo;

        Vector3 _currentInputVector;

        bool IInteractor.IsInteractable => _currentInputVector.sqrMagnitude < 0.3f;

        void OnEnable()
        {
            _inputChannelSo.JoystickUpdate += OnJoystickUpdate;
        }

        void OnDisable()
        {
            _inputChannelSo.JoystickUpdate -= OnJoystickUpdate;
        }

        void FixedUpdate()
        {
            _rbd.velocity = new Vector3(_currentInputVector.x, _rbd.velocity.y, _currentInputVector.z);
        }

        void OnJoystickUpdate(Vector2 newMoveDirection)
        {
            if (newMoveDirection.magnitude >= 1f)
            {
                newMoveDirection.Normalize();
            }

            _currentInputVector = new Vector3(newMoveDirection.x * _movementDataSO.SideMoveSpeed, 0f, newMoveDirection.y * _movementDataSO.ForwardMoveSpeed);
            Vector3 lookDir = new Vector3(_currentInputVector.x, 0f, _currentInputVector.z);
            if (lookDir != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(lookDir, Vector3.up);
            }
        }
    }
}