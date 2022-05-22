using System;
using System.Collections;
using UnityEngine;
using WeaponSystem;
using Random = UnityEngine.Random;

namespace EnemySystem
{
    public class TacticalEnemy : Enemy, IHandleWeapon
    {
        protected override void Awake()
        {
            base.Awake();
        }

        protected override IEnumerator AttackRoutine()
        {
            var fixedUpdate = new WaitForFixedUpdate();
            while (Target && InAttackRange)
            {
                var position = Target.position + (Vector3) Random.insideUnitCircle * range;
                var time = Vector2.Distance(position, transform.position) / speed;
                transform.up = (position - transform.position).normalized;

                while (time > 0 && InAttackRange)
                {
                    Rb.velocity = (position - transform.position).normalized * speed;
                    time -= Time.fixedDeltaTime;
                    yield return fixedUpdate;
                }

                time = 2f;
                while (time > 0 && InAttackRange)
                {
                    Rb.velocity = Vector2.zero;
                    LookTarget();
                    PullTrigger = true;
                    time -= Time.fixedDeltaTime;
                    yield return fixedUpdate;
                }
            }

            State = State.Chasing;
        }

        public bool PullTrigger { get; set; }
        public event EventHandler<int> OnSwitchWeapon;
    }
}