using System;
using ReferenceSharing;
using UnityEngine;
using WeaponSystem;

namespace PlayerSystem
{
    public class PlayerHealth : MonoBehaviour, ITakeHit
    {
        [SerializeField] private Player data;
        [SerializeField] private Reference<float> maxHealth, health;

        public event EventHandler OnDie, OnTakeHit;

        private void Awake()
        {
            maxHealth.Value = health.Value = data.maxHealth;
        }

        private void Start()
        {
            
        }

        private void Die()
        {
            OnDie?.Invoke(this, null);
        }

        public void Hit(int amount)
        {
            health.Value -= amount;
            OnTakeHit?.Invoke(this, null);

            if (health.Value <= 0) Die();
        }
    }
}