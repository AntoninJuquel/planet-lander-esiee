using System;
using ReferenceSharing;
using UnityEngine;
using WeaponSystem;

namespace PlayerSystem
{
    public class PlayerHealth : MonoBehaviour, ITakeHit
    {
        private Player Data => PlayerManager.Data;
        [SerializeField] private Reference<int> maxHealth, health;

        public event EventHandler OnDie, OnTakeHit;

        private void Awake()
        {
            maxHealth.Value = health.Value = Data.maxHealth;
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