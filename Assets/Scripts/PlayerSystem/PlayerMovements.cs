using ReferenceSharing;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerSystem
{
    public class PlayerMovements : MonoBehaviour
    {
        [SerializeField] private Player data;
        [SerializeField] private InputAction horizontal, vertical;
        [SerializeField] private Reference<float> horizontalInputRef, verticalInputRef, fuelRef;

        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void OnEnable()
        {
            horizontal.Enable();
            vertical.Enable();
        }

        private void OnDisable()
        {
            horizontal.Disable();
            vertical.Disable();
        }

        private void Update()
        {
            horizontalInputRef.Value = horizontal.ReadValue<float>();
            verticalInputRef.Value = vertical.ReadValue<float>();
        }

        private void FixedUpdate()
        {
            Move();
            Rotate();
            CounterGravity();
        }

        private void Move()
        {
            if (fuelRef.Value > 0)
            {
                _rb.AddForce(transform.up * (data.force * verticalInputRef.Value));
                _rb.drag = verticalInputRef.Value;
            }
        }

        private void Rotate()
        {
            if (fuelRef.Value > 0)
                _rb.angularVelocity = -horizontalInputRef.Value * data.torqueSpeed * (verticalInputRef.Value != 0 ? .5f : 1);
        }

        private void CounterGravity()
        {
            if (verticalInputRef != 0 && fuelRef.Value > 0)
                _rb.AddForce(Vector2.up * Mathf.Abs(Physics2D.gravity.y), ForceMode2D.Force);
        }
    }
}