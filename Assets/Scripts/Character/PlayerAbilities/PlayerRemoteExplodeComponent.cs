using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Rendering;
using UnityEngine.InputSystem;

namespace TimelineIso
{
    public class PlayerRemoteExplodeComponent : PlayerAbilityComponent
    {
        public float ExplodeDuration = 1f;
        public float MinHold = .2f;
        public float ChargeTime = 3f;
        public float ExplosionSize = 3f;
        public MeshRenderer ChargeEffectPrefab;
        public MeshRenderer ExplodeEffectPrefab;
        public string Command = "Charge"; // TODO: remap

        private bool busy;
        private float chargeStart;
        private GameObject ChargeEffectInstance;
        private GameObject ExplodeEffectInstance;
        private float diam;
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
            if (!(input is CommandInput))
            {
                return InputHandledStatus.Deny;
            }
            var commandInput = (CommandInput)input;

            if (commandInput.targetAbility != this)
            {
                return InputHandledStatus.Deny;
            }

            if (commandInput.button.is_on() && !this.busy)
            {
                DoCharge(commandInput.button.button_name);
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
        }

        private void DoCharge(string buttonName)
        {
            this.busy = true;
            this.chargeStart = Time.time;
            var effect = Instantiate(this.ChargeEffectPrefab, this.transform);
            effect.transform.parent = null;
            effect.transform.localScale = new Vector3(ExplosionSize, ExplosionSize, ExplosionSize); // TODO: uhh
            effect.material.SetFloat("_StartTime", this.chargeStart);
            effect.material.SetFloat("_Duration", this.ChargeTime);
            this.ChargeEffectInstance = effect.gameObject;
            StartCoroutine(ChargeCoroutine(buttonName));
        }

        IEnumerator ChargeCoroutine(string buttonName)
        {
            var movement = this.GetComponent<PlayerMovement>();
            var entityId = this.GetComponent<EntityComponent>().identifier;
            var start = Time.time;
            movement.SpeedMultiplier = 0f;

            while (this.contiguousInput.ReadValue<float>(entityId, buttonName) > 0)
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
                //TODO
                this.diam = this.ChargeEffectInstance.GetComponent<Renderer>().bounds.size.x;
                Destroy(this.ChargeEffectInstance);
                StartCoroutine(ExplodeCoroutine(Time.time - start, position));
            }
        }

        IEnumerator ExplodeCoroutine(float chargeTime, Vector3 target)
        {
            // TODO: make ring unit scale and have a scale factor here on this command
            chargeTime = Math.Min(chargeTime, ChargeTime);
            var explodeScale = (chargeTime / ChargeTime) * ExplosionSize;
            var renderer = Instantiate(this.ExplodeEffectPrefab, target.XZPlane(), Quaternion.identity);
            this.ExplodeEffectInstance = renderer.gameObject;
            this.ExplodeEffectInstance.transform.localScale = new Vector3(explodeScale, explodeScale, explodeScale);
            this.ExplodeEffectInstance.GetComponent<ExplosionCollider>().DamageMultiplier = chargeTime / ChargeTime;
            var startTime = Time.time;

            renderer.material.SetFloat("_Height", .9f);
            renderer.material.SetFloat("_Thickness", .4f);
            while (Time.time <= startTime + ExplodeDuration)
            {
                // TODO: time calc can happen in shader rather than this loop;
                var deltaT = (Time.time - startTime) / ExplodeDuration;
                renderer.material.SetFloat("_Height", 1 - 2*deltaT);
                yield return null;
            }
            Finish();
        }
        private void ClearState()
        {
            this.StopAllCoroutines();
            if (this.ExplodeEffectInstance) { Destroy(this.ExplodeEffectInstance); }
            if (this.ChargeEffectInstance) { Destroy(this.ChargeEffectInstance); }
        }


    }
}
