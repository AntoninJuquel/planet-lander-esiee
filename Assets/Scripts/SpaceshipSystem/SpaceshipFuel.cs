﻿using ReferenceSharing;
using UnityEngine;

namespace SpaceshipSystem
{
    public class SpaceshipFuel : MonoBehaviour
    {
        private Spaceship Data => SpaceshipManager.Data;
        [SerializeField] private Reference<float> verticalInputRef, fuelRef, maxFuelRef;

        private void Awake()
        {
            fuelRef.Value = maxFuelRef.Value = Data.maxFuel;
        }

        private void Update()
        {
            if (verticalInputRef.Value != 0 && fuelRef.Value > 0)
            {
                fuelRef.Value -= Time.deltaTime * Data.fuelBurnRate;
            }
        }
    }
}