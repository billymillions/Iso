using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Rendering;
using UnityEngine.InputSystem;
using System.Diagnostics.Tracing;

namespace TimelineIso
{
    public class PlayerPulseComponent : PlayerAbilityComponent
    {
        public MeshRenderer PulsePrefab;
        public float FireDuration = .2f;
        public float PulseDuration = .1f;
        public float Force = 5f;
        public float Falloff = 1f;
        private bool busy;

        public override void Initialize()
        {

        }

        private void Fire()
        {
            // TODO spawn location
            var pulse = Instantiate(this.PulsePrefab, this.transform);
            pulse.transform.parent = null;
            StartCoroutine(PulseRoutine(pulse.gameObject));
        }

        private IEnumerator PulseRoutine(GameObject pulse)
        {
            if (! Time.inFixedTimeStep)
            {
                Debug.Log("Wut");
            }
            pulse.GetComponent<Renderer>().material.SetFloat("_StartTime", Time.fixedTime);
            pulse.GetComponent<Renderer>().material.SetFloat("_Duration", PulseDuration * 1f);
            pulse.GetComponent<EventColliderComponent>().OnCollide.AddListener((Collider col) => this.OnCollision(pulse, col));
            pulse.GetComponent<EventColliderComponent>().Duration = PulseDuration;
            yield return new WaitForFixedSeconds(PulseDuration);
            if (! Time.inFixedTimeStep)
            {
                Debug.Log("More understandable, but wut");
            }
            Destroy(pulse);
        }

        private void OnCollision(GameObject pulse, Collider collider)
        {
            if (collider.GetComponent<Enemy>() == null)
            {
                return;
            }
            //var direction = (collider.transform.position - pulse.transform.position).XZPlane();
            var direction = this.transform.forward.XZPlane();
            direction = direction.normalized;
            collider.GetComponent<Impulse>().Displace(direction * Force, .2f);
            return;
        }


        public override PlayerAbilityStatus Status()
        {
            return (busy) ? PlayerAbilityStatus.Running : PlayerAbilityStatus.Finished;
        }

        public override InputHandledStatus HandleInput(IInputEvent input)
        {
            if (input is CommandInput c && c.button.is_press && c.targetAbility == this && !busy)
            {
                StartCoroutine(RunFire());
                return InputHandledStatus.Handled;
            }
            return InputHandledStatus.Deny;
        }

        public IEnumerator RunFire()
        {
            this.GetComponent<PlayerMovement>().SpeedMultiplier = 0;
            this.busy = true;
            yield return new WaitForFixedSeconds(FireDuration);
            this.Fire();
            yield return new WaitForFixedSeconds(FireDuration);
            this.busy = false;
            this.GetComponent<PlayerMovement>().SpeedMultiplier = 1f;
            yield return null;
        }

        void Start()
        {
        }


        public override void Finish()
        {
        }
    }
}
