﻿using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Rendering;
using UnityEngine.InputSystem;

namespace TimelineIso
{
    public class PlayerShoutComponent : PlayerAbilityComponent
    {
        public float ShoutDuration = 1f;
        public float MinHold = .2f;
        public float ChargeTime = .5f;
        public float ShoutSize = 3f;
        public ChargeEffect ChargeEffectPrefab;
        public EventColliderComponent ShoutColliderPrefab;

        private bool busy;

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
            if (input is CommandInput ci && ci.targetAbility == this && ci.button.is_press && !busy)
            {
                DoCharge(ci.button.button_name);
                return InputHandledStatus.Handled;
            }
            return InputHandledStatus.Deny;
        }


        public override void Finish()
        {
            this.busy = false;
        }

        private void DoCharge(string buttonName)
        {
            this.busy = true;
            var chargeEffect = Instantiate(ChargeEffectPrefab, this.transform);
            chargeEffect.Initialize(this.GetComponent<EntityComponent>().identifier, buttonName, this.ShoutSize, this.ChargeTime);
            chargeEffect.OnCharged.AddListener(this.DoShout);
        }

        private void DoShout(ChargeResult ce) {
            var collider = Instantiate(ShoutColliderPrefab, this.transform);
            collider.transform.localScale = Vector3.one * this.ShoutSize * ce.progress; // fudge
            var renderer = collider.GetComponent<Renderer>();
            renderer.material.SetFloat("_StartTime", Time.time);
            renderer.material.SetFloat("_Duration", ShoutDuration);
            collider.OnCollide.AddListener(this.OnCollision);
            StartCoroutine(Cooldown());

        }

        private IEnumerator Cooldown()
        {
            var movement = this.GetComponent<PlayerMovement>();
            movement.SpeedMultiplier = 0; 
            yield return new WaitForSeconds(ShoutDuration);
            movement.SpeedMultiplier = 1f; 
            this.busy = false;
        }

        private void OnCollision(Collider collider)
        {
            Debug.Log(collider);
        }

    }
}
