using System;
using UnityEngine;
using UnityEngine.InputSystem;
using WeaponSystem;

namespace SpaceshipSystem
{
    public class SpaceshipAttack : MonoBehaviour, IHandleWeapon
    {
        [SerializeField] private InputAction selectWeapon, shoot;
        public bool PullTrigger { get; private set; }
        public event EventHandler<int> OnSwitchWeapon;

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
            OnSwitchWeapon?.Invoke(this, (int) c.ReadValue<float>());
        }
    }
}