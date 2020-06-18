using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Rendering;
using UnityEditor.Experimental.GraphView;

namespace TimelineIso
{
    public class PlayerJumpComponentActionDriven : MonoBehaviour, PlayerAbility
    {
        public float JumpDuration = 1f;
        public float JumpHeight = 5f;
        public float MinHold = .2f;
        public MeshRenderer ChargeEffectPrefab;

        bool charging { get => this.chargeRoutine != null; }
        bool jumping  { get => this.jumpRoutine != null; }
        private bool busy { get => jumping || charging; }
        private float chargeStart;
        private GameObject ChargeEffectInstance;
        private Coroutine chargeRoutine;
        private Coroutine jumpRoutine;

        public void Initialize()
        {
            return;
        }

        public PlayerAbilityStatus Status()
        {
            return (charging || jumping) ? PlayerAbilityStatus.Running : PlayerAbilityStatus.Finished;
        }

        public InputHandledStatus HandleInput(IInputEvent input)
        {
            // TODO: queued handling of charge input
            // possibility A: if both a press and a release are in the queue, they should be removed,
            //   this prevents the case where you tap charge during a
            // possibility B: never queue more than one of a given type
            //   this prevents the above
            //   this also prevents having a double execute... like queueing two combos during part 1
            //   this changes input priorities, which might be bad. like attack, dash, attack
            // possibility C: add value handling for holds that doesn't resemble buttons


            // TODO: nice type handling?
            if (!(input is ChargeInput))
            {
                return InputHandledStatus.Deny;
            }
            var chargeInput = (ChargeInput)input;
            if (chargeInput.isRelease && this.charging)
            {
                DoJump();
                return InputHandledStatus.Handled;
            }
            if (!this.busy && !chargeInput.isRelease)
            {
                DoCharge();
                return InputHandledStatus.Handled;
            }
            if (!this.busy && chargeInput.isRelease)
            {
                // swallow any unhandled release 
                return InputHandledStatus.Handled;
            }
            return InputHandledStatus.Deny;
        }

        private void DoCharge()
        {
            this.chargeStart = Time.time;
            var effect = Instantiate(this.ChargeEffectPrefab, this.transform);
            effect.transform.parent = null;
            effect.transform.localScale = new Vector3(3, 3, 3); // TODO: uhh
            effect.material.SetFloat("_StartTime", this.chargeStart);

            this.ClearState();
            this.ChargeEffectInstance = effect.gameObject;
            this.chargeRoutine = this.StartCoroutine(this.ChargeCoroutine());
        }

        private void DoJump()
        {
            if ((Time.time - this.chargeStart) <= MinHold)
            {
                this.Finish();
                return;
            }
            var position = this.ChargeEffectInstance.transform.position;

            this.ClearState();
            this.jumpRoutine = StartCoroutine(JumpCoroutine(position));
        }


        public void ClearState()
        {
            if (this.ChargeEffectInstance != null)
            {
                Destroy(this.ChargeEffectInstance);
                this.ChargeEffectInstance = null;
            }
            if (this.chargeRoutine != null)
            {
                StopCoroutine(this.chargeRoutine);
                this.chargeRoutine = null;
            };
            if (this.jumpRoutine != null)
            {
                StopCoroutine(this.jumpRoutine);
                this.jumpRoutine = null;
            };
        }

        public void Finish()
        {
            ClearState();
            var movement = this.GetComponent<PlayerMovement>();
            movement.SpeedMultiplier = 1f;
        }

        IEnumerator ChargeCoroutine()
        {
            var movement = this.GetComponent<PlayerMovement>();
            movement.SpeedMultiplier = 0f;
            while (this.ChargeEffectInstance != null)
            {
                this.ChargeEffectInstance.transform.Translate(movement.Velocity * Time.deltaTime, Space.World);
                var facing = (ChargeEffectInstance.transform.position - this.transform.position).XZPlane().normalized;
                if (facing.magnitude > 0)
                {
                    this.transform.forward = facing;
                }
                yield return null;
            }
        }

        IEnumerator JumpCoroutine(Vector3 target)
        {
            var startTime = Time.time;
            var rigidbody = this.GetComponent<Rigidbody>();
            var halfDuration = 0.5f * this.JumpDuration;
            var v_y = 2f * this.JumpHeight / halfDuration;
            var v_xz = (target - this.transform.position).XZPlane() / JumpDuration;
            var v_0 = new Vector3(v_xz.x, v_y, v_xz.z);
            var a = new Vector3(0, -v_y / halfDuration, 0);
            var d_0 = this.transform.position;
            var deltaTime = 0f;

            rigidbody.isKinematic = true;
            while (Time.time < startTime + JumpDuration)
            {
                deltaTime = Time.time - startTime;

                var position = d_0 + v_0 * deltaTime + .5f * a * deltaTime * deltaTime;
                // hack
                if (position.y < d_0.y)
                {
                    break;
                }

                this.transform.position = position;
                yield return null;
                // todo collision / raycast
            }
            // hack
            this.transform.position = new Vector3(this.transform.position.x, d_0.y, this.transform.position.z);
            rigidbody.isKinematic = false;
            Finish();
        }
    }
}
