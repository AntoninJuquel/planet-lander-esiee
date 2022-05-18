using UnityEngine;

namespace PlayerSystem
{
    [CreateAssetMenu]
    public class Player : ScriptableObject
    {
        public float maxHealth;
        public float force;
        public float torqueSpeed;
        public float maxLandAngle;
        public float maxLandVelocity;
        public float maxFuel;
        public float fuelBurnRate;
        public float minParticleRate;
        public float maxParticleRate;
    }
}