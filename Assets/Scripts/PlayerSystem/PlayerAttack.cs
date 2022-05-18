using System;
using UnityEngine;
using UnityEngine.InputSystem;
using WeaponSystem;

namespace PlayerSystem
{
    public class PlayerAttack : MonoBehaviour, IWeaponUser
    {
        [SerializeField] private InputAction selectWeapon, shoot;
        public bool PullTrigger { get; private set; }
        public event EventHandler<int> onSwitchWeapon;

        private void OnEnable()
        {
            shoot.Enable();
            selectWeapon.Enable();


            selectWeapon.performed += OnSelectWeaponPerformed;
        }

        private void OnDisable()
        {
            shoot.Disable();
            selectWeapon.Disable();

            selectWeapon.performed -= OnSelectWeaponPerformed;
        }

        private void Update()
        {
            PullTrigger = shoot.IsPressed();
        }

        private void OnSelectWeaponPerformed(InputAction.CallbackContext c)
        {
            onSwitchWeapon?.Invoke(this, (int) c.ReadValue<float>());
        }
    }
}