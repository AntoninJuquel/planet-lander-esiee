using System;
using ReferenceSharing;
using UnityEngine;
using WeaponSystem;

namespace SpaceshipSystem
{
    public class SpaceshipHealth : MonoBehaviour, ITakeHit
    {
        private Spaceship Data => SpaceshipManager.Data;
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