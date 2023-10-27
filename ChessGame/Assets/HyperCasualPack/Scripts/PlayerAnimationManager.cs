using HyperCasualPack.ScriptableObjects.Channels;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.HyperCasualPack.Scripts
{
    public class PlayerAnimationManager : MonoBehaviour
    {
        [SerializeField] InputChannelSO _inputChannelSO;
        [SerializeField] Animator _animator;

        int moveId = Animator.StringToHash("Move");

        void OnEnable()
        {
            _inputChannelSO.JoystickUpdate += InputChannelSO_JoystickUpdate;
        }

        void OnDisable()
        {
            _inputChannelSO.JoystickUpdate -= InputChannelSO_JoystickUpdate;
        }


        void InputChannelSO_JoystickUpdate(Vector2 moveVector)
        {
            _animator.SetFloat(moveId, moveVector.magnitude);
        }
    }
}