using System;
using ReferenceSharing;
using UnityEngine;

namespace PlayerSystem
{
    public class PlayerCollision : MonoBehaviour
    {
        public event EventHandler OnCrash;
        [SerializeField] private Player data;
        [SerializeField] private Reference<bool> landedRef;
        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            var angle = Vector2.Angle(transform.up, col.contacts[0].normal);
            var collisionSpeed = col.relativeVelocity.magnitude;

            if (angle < data.maxLandAngle && collisionSpeed < data.maxLandVelocity)
            {
                Land(col);
            }
            else
            {
                OnCrash?.Invoke(this, null);
                Debug.Log(angle);
                Debug.Log(collisionSpeed);
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            landedRef.Value = false;
        }

        private void Land(Collision2D col)
        {
            transform.up = col.contacts[0].normal;
            _rb.velocity = Vector2.zero;
            landedRef.Value = true;
        }
    }
}