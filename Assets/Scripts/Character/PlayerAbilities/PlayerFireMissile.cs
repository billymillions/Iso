using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Rendering;
using UnityEngine.InputSystem;

namespace TimelineIso
{
    public class PlayerFireMissile: PlayerAbilityComponent
    {
        public Vector2 Deviation = new Vector2(2f, .1f);
        public float Handling = 30f;
        public float Speed = 30f;
        public float Falloff = 10f;
        public Missile MissilePrefab;
        public Transform SpawnLocation;

        public override void Initialize()
        {

        }

        private void Fire()
        {
            var lockon = this.GetComponent<PlayerLockon>();
            var deviation = (UnityEngine.Random.insideUnitCircle * Deviation);
            var target = (lockon.Locked != null) ? lockon.Locked : lockon.ClosestSighted(this.transform.forward);
            var direction = (this.transform.rotation * new Vector3(deviation.x, deviation.y, 0) + this.transform.forward).normalized;
            // TODO spawn location
            var missile = Instantiate(this.MissilePrefab, this.transform);
            missile.transform.parent = null;
            missile.Target = (target) ? target.gameObject : null;
            missile.Handling = Handling;
            missile.Speed = Speed;
            missile.Falloff = Falloff;
            missile.GetComponent<Rigidbody>().velocity = direction * missile.Speed;
        }

        public override PlayerAbilityStatus Status()
        {
            return PlayerAbilityStatus.Finished;
        }

        public override InputHandledStatus HandleInput(IInputEvent input)
        {
            if (input is CommandInput c && c.button.is_press && c.targetAbility == this)
            {
                Fire();
                return InputHandledStatus.Handled;
            }
            return InputHandledStatus.Deny;
        }

        void Start()
        {
        }


        public override void Finish()
        {
        }
    }
}
