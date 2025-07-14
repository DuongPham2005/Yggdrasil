using UnityEngine;
using UnityEngine.InputSystem;

namespace Retro.ThirdPersonCharacter
{
    public class PlayerInput : MonoBehaviour
    {
        private PlayerControls _controls;

        public bool AttackInput { get; private set; }
        public bool SpecialAttackInput { get; private set; }
        public Vector2 MovementInput { get; private set; }
        public bool JumpInput { get; private set; }
        public bool ChangeCameraModeInput { get; private set; }
        public bool IsRunning { get; private set; }

        private void Awake()
        {
            _controls = new PlayerControls();

            _controls.Gameplay.Move.performed += ctx => MovementInput = ctx.ReadValue<Vector2>();
            _controls.Gameplay.Move.canceled += ctx => MovementInput = Vector2.zero;

            _controls.Gameplay.Jump.performed += ctx => JumpInput = true;
            _controls.Gameplay.Jump.canceled += ctx => JumpInput = false;

            _controls.Gameplay.Attack.performed += ctx => AttackInput = true;
            _controls.Gameplay.Attack.canceled += ctx => AttackInput = false;

            _controls.Gameplay.SpecialAttack.performed += ctx => SpecialAttackInput = true;
            _controls.Gameplay.SpecialAttack.canceled += ctx => SpecialAttackInput = false;

            _controls.Gameplay.ChangeCameraMode.performed += ctx => ChangeCameraModeInput = true;

            _controls.Gameplay.Run.performed += ctx => IsRunning = true;
            _controls.Gameplay.Run.canceled += ctx => IsRunning = false;
        }

        private void OnEnable() => _controls?.Enable();
        private void OnDisable() => _controls?.Disable();

        private void LateUpdate()
        {
            AttackInput = false;
            SpecialAttackInput = false;
            ChangeCameraModeInput = false;
        }
    }
}
