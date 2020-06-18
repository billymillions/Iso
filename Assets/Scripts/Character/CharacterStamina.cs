using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.XR.Haptics;

namespace TimelineIso
{
    [System.Serializable]
    public struct StaminaStat
    {
        public float current;
        public float max;
    }

    public class CharacterStamina : MonoBehaviour
    {
        [SerializeField]
        public StaminaStat Stamina = new StaminaStat { current = 100, max = 100 };
        public float RegenRate = 10f;
        public float MinStamina = -50f;
        private Timeline timeline;
        private EntityComponent entityComponent;
        private PlayerController playerController;

        public bool ConsumeStamina(int stamina)
        {
            if (Stamina.current <= 0)
            {
                return false;
            }

            this.Stamina.current -= stamina;
            return true;
        }

        public void Start()
        {
            this.timeline = TimelineMono.GetForScene();
            this.entityComponent = this.GetComponent<EntityComponent>();
            this.playerController = this.GetComponent<PlayerController>();
        }

        public void FixedUpdate()
        {
            // maybe??
            if (!this.playerController.IsBusy)
            {
                Stamina.current += RegenRate * Time.deltaTime;
            }
            Stamina.current = Mathf.Clamp(Stamina.current, MinStamina, Stamina.max);
            Save();
        }

        public void Save()
        {
            if (timeline.IsReverse || timeline.IsSnap)
            {
                timeline.RestoreIfValue(this.entityComponent.identifier, ref Stamina);
            }
            else
            {
                timeline.Remember(this.entityComponent.identifier, Stamina);
            }
        }
    }
}
