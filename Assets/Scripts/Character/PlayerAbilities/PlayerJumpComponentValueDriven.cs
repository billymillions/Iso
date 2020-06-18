using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Rendering;
using UnityEngine.InputSystem;

namespace TimelineIso
{
    public class PlayerJumpComponentValueDriven: MonoBehaviour, PlayerAbility
    {
        public float JumpDuration = 1f;
        public float JumpHeight = 5f;
        public float MinHold = .2f;
        public MeshRenderer ChargeEffectPrefab;
        public string Command = "Charge"; // TODO: remap

        private bool busy;
        private float chargeStart;
        private GameObject ChargeEffectInstance;
        private ContiguousInputSave contiguousInput;
        private PlayerInput playerInput;
        private GlobalInputCache globalInput;

        public void Initialize()
        {
            return;
        }

        public PlayerAbilityStatus Status()
        {
            return (this.busy) ? PlayerAbilityStatus.Running : PlayerAbilityStatus.Finished;
        }

        public InputHandledStatus HandleInput(IInputEvent input)
        {
            if (!(input is ButtonInput))
            {
                return InputHandledStatus.Deny;
            }
            var buttonInput = (ButtonInput)input;
            
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


        public void Finish()
        {
            ClearState();
            var movement = this.GetComponent<PlayerMovement>();
            movement.SpeedMultiplier = 1f;
            this.busy = false;
        }

        private void DoCharge()
        {
            this.busy = true;
            this.chargeStart = Time.time;
            var effect = Instantiate(this.ChargeEffectPrefab, this.transform);
            effect.transform.parent = null;
            effect.transform.localScale = new Vector3(3, 3, 3); // TODO: uhh
            effect.material.SetFloat("_StartTime", this.chargeStart);
            this.ChargeEffectInstance = effect.gameObject;
            StartCoroutine(ChargeCoroutine());
        }

        IEnumerator ChargeCoroutine()
        {
            var movement = this.GetComponent<PlayerMovement>();
            var entityId = this.GetComponent<EntityComponent>().identifier;
            var start = Time.time;
            movement.SpeedMultiplier = 0f;

            while (this.contiguousInput.ReadValue<float>(entityId, this.Command) > 0)
            {
                this.ChargeEffectInstance.transform.Translate(movement.Velocity * Time.deltaTime, Space.World);
                var facing = (ChargeEffectInstance.transform.position - this.transform.position).XZPlane().normalized;
                if (facing.magnitude > 0)
                {
                    this.transform.forward = facing;
                }
                yield return null;
            }

            if (Time.time - start <= this.MinHold)
            {
                Finish();
            }
            else
            {
                var position = this.ChargeEffectInstance.transform.position;
                Destroy(this.ChargeEffectInstance);
                StartCoroutine(JumpCoroutine(position));
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
