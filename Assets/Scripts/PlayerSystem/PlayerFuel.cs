using ReferenceSharing;
using UnityEngine;

namespace PlayerSystem
{
    public class PlayerFuel : MonoBehaviour
    {
        [SerializeField] private Player data;
        [SerializeField] private Reference<float> verticalInputRef, fuelRef, maxFuelRef;

        private void Awake()
        {
            fuelRef.Value = maxFuelRef.Value = data.maxFuel;
        }

        private void Update()
        {
            if (verticalInputRef.Value != 0 && fuelRef.Value > 0)
            {
                fuelRef.Value -= Time.deltaTime * data.fuelBurnRate;
            }
        }
    }
}