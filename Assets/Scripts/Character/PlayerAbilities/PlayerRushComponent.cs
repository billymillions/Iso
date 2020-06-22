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
        private GlobalInputCache globalInput;

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
            var movement = this.GetComponent<PlayerMovement>();
            var entityId = this.GetComponent<EntityComponent>().identifier;
            var start = Time.time;
            movement.SpeedMultiplier = 0f;
            var facing = this.transform.forward;

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
                yield return null;
            }

            if (Time.time - start <= this.MinHold)
            {
                Finish();
            }
            else
            {
                ClearState();
                StartCoroutine(RushCoroutine(Time.time - start, facing));
            }
        }

        IEnumerator RushCoroutine(float duration, Vector3 direction)
        {
            var startTime = Time.time;
            var rigidbody = this.GetComponent<Rigidbody>();
            duration = Math.Min(duration, ChargeDuration);
            var speed = MaxDistance / RushDuration;
            var coroutineDuration = (duration / ChargeDuration) * RushDuration;

            while (Time.time < startTime + coroutineDuration)
            {
                rigidbody.velocity = direction * speed;
                yield return null;
            }
            rigidbody.velocity = this.transform.forward * 0;
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
