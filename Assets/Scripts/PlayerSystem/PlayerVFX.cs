using ReferenceSharing;
using UnityEngine;

namespace PlayerSystem
{
    public class PlayerVFX : MonoBehaviour
    {
        [SerializeField] private Player data;
        [SerializeField] private Reference<float> verticalInputRef, fuelRef;
        private ParticleSystem.EmissionModule _emission;

        private void Awake()
        {
            _emission = GetComponent<ParticleSystem>().emission;
        }

        private void Update()
        {
            _emission.enabled = verticalInputRef.Value != 0 && fuelRef.Value > 0;
            _emission.rateOverDistanceMultiplier = data.minParticleRate + verticalInputRef.Value * (data.maxParticleRate - data.minParticleRate);
        }
    }
}