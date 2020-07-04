using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Rendering;
using UnityEngine.InputSystem;

namespace TimelineIso
{
    public class PlayerRushComponent : PlayerAbilityComponent
    {
        public float ChargeDuration = 1f;
        public float MaxDistance = 20f;
        public float RushDuration = .5f;
        public float MinHold = .2f;

        public MeshRenderer ChargeEffectPrefab;
        public string Command = "Charge"; // TODO: remap

        private bool busy;
        private float chargeStart;
        private GameObject ChargeEffectInstance;
        private ContiguousInputSave contiguousInput;
        private PlayerInput playerInput;
        private PlayerMovement movement;
        private GlobalInputCache globalInput;
        private EntityMove em;

        public override void Initialize()
        {
            return;
        }

        public override PlayerAbilityStatus Status()
        {
            return (this.busy) ? PlayerAbilityStatus.Running : PlayerAbilityStatus.Finished;
        }

        public override InputHandledStatus HandleInput(IInputEvent input)
        {
            if (input is DashInput)
            {
                Finish();
                return InputHandledStatus.Permit;
            }

            if (!(input is CommandInput))
            {
                return InputHandledStatus.Deny;
            }
            var buttonInput = ((CommandInput)input).button;

            if (buttonInput.button_name != this.Command)
            {
                return InputHandledStatus.Deny;
            }

            if (buttonInput.is_on() && !this.busy)
            {
                DoCharge();
            }
            return InputHandledStatus.Handled;
        }

        void Start()
        {
            this.contiguousInput = GameObject.Find("GlobalInputCache").GetComponent<ContiguousInputSave>();
            this.globalInput = GameObject.Find("GlobalInputCache").GetComponent<GlobalInputCache>();
            this.playerInput = GameObject.Find("GlobalInputCache").GetComponent<PlayerInput>();
            this.movement = this.GetComponent<PlayerMovement>();
            this.em = this.GetComponent<EntityMove>();
        }


        public override void Finish()
        {
            ClearState();
            var movement = this.GetComponent<PlayerMovement>();
            movement.SpeedMultiplier = 1f;
            this.busy = false;
            this.StopAllCoroutines();
        }

        private void DoCharge()
        {
            this.busy = true;
            this.chargeStart = Time.time;
            var effect = Instantiate(this.ChargeEffectPrefab, this.transform);
            effect.material.SetFloat("_StartTime", this.chargeStart);
            effect.material.SetFloat("_Duration", this.ChargeDuration);
            this.ChargeEffectInstance = effect.gameObject;
            StartCoroutine(ChargeCoroutine());
        }

        IEnumerator ChargeCoroutine()
        {
            //var movement = this.GetComponent<PlayerMovement>();
            var entityId = this.GetComponent<EntityComponent>().identifier;
            var start = Time.time;
            movement.SpeedMultiplier = 0f;
            var facing = this.transform.forward;
            var frames = 0;

            while (this.contiguousInput.ReadValue<float>(entityId, this.Command) > 0)
            {
                if (movement.Velocity.magnitude > 0)
                {
                    facing = movement.Velocity.normalized.XZPlane();
                    this.transform.forward = facing;
                } else
                {
                    facing = this.transform.forward.XZPlane();
                }
                yield return new WaitForFixedUpdate();
                frames += 1;
            }
            var time = frames * Time.fixedDeltaTime;

            if (time <= this.MinHold)
            {
                Finish();
            }
            else
            {
                ClearState();
                StartCoroutine(RushCoroutine(time, facing));
            }
        }

        IEnumerator RushCoroutine(float chargeDuration, Vector3 direction)
        {
            var startTime = Time.time;
            var rigidbody = this.GetComponent<Rigidbody>();
            var ratio = Math.Min(chargeDuration / ChargeDuration, 1f);
            var distance = MaxDistance * ratio;
            var coroutineDuration = RushDuration * ratio;
            var numFrames = Math.Max(1, (int) (coroutineDuration / Time.fixedDeltaTime));
            var delta = (direction.XZPlane().normalized * distance) / numFrames;

            for (int i = 0; i < numFrames; i ++)
            {
                var result = this.em.MoveDelta(delta);
                yield return new WaitForFixedUpdate();
                if ((result & CollisionFlags.Sides) > 0)
                {
                    // TODO
                    this.GetComponent<Impulse>().Displace(-2f * delta.normalized, .1f);
                    break;
                }
            }
            Finish();
        }
        private void ClearState()
        {
            if (this.ChargeEffectInstance != null)
            {
                Destroy(this.ChargeEffectInstance);
                this.ChargeEffectInstance = null;
            }
        }


    }
}
