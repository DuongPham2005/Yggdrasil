using UnityEngine;
using NaughtyCharacter;

namespace Retro.ThirdPersonCharacter
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(Animator))]
    public class Combat : MonoBehaviour
    {
        private const string attackTriggerName = "Attack";
        private const string specialAttackTriggerName = "Ability";

        private Animator _animator;
        private PlayerInput _playerInput;

        public bool AttackInProgress {get; private set;} = false;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _playerInput = GetComponent<PlayerInput>();
        }

        private void Update()
        {
            if(_playerInput.AttackInput && !AttackInProgress)
            {
                Attack();
            }
            else if (_playerInput.SpecialAttackInput && !AttackInProgress)
            {
                SpecialAttack();
            }
        }

        public void SetAttackStart()
        {
            AttackInProgress = true;

            // Bật hitbox nếu cần
            var weapon = GetComponentInChildren<WeaponActivator>();
            if (weapon != null)
            {
                weapon.EnableHitbox();
            }
        }

        public void SetAttackEnd()
        {
            AttackInProgress = false;

            // Tắt hitbox
            var weapon = GetComponentInChildren<WeaponActivator>();
            if (weapon != null)
            {
                weapon.DisableHitbox();
            }
        }

        public void EnableHitbox()
        {
            var weapon = GetComponentInChildren<WeaponActivator>();
            if (weapon != null)
            {
                weapon.EnableHitbox();
            }
        }

        public void DisableHitbox()
        {
            var weapon = GetComponentInChildren<WeaponActivator>();
            if (weapon != null)
            {
                weapon.DisableHitbox();
            }
        }


        private void Attack()
        {
            _animator.SetTrigger(attackTriggerName);
        }

        private void SpecialAttack()
        {
            _animator.SetTrigger(specialAttackTriggerName);
        }
    }
}