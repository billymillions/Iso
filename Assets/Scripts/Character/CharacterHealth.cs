using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TimelineIso
{
    [System.Serializable]
    public struct HealthStat
    {
        public float current;
        public float max;
        public float shield;
    }

    [System.Serializable]
    public class DamageEvent: UnityEvent<int> { }

    public class CharacterHealth : MonoBehaviour
    {
        [SerializeField]
        public HealthStat Health = new HealthStat { current = 100, max = 100 };
        public UnityEvent<int> Damage;
        private Timeline timeline;
        private EntityComponent entityComponent;

        private void Awake()
        {
            this.Damage = new DamageEvent();
        }

        public void Start()
        {
            this.timeline = TimelineMono.GetForScene();
            this.entityComponent = this.GetComponent<EntityComponent>();
        }

        public void InflictDamage(float damage)
        {
            this.Health.current -= damage;
            this.Health.current = Math.Max(this.Health.current, 0);
            Damage.Invoke((int)damage);
        }

        public void FixedUpdate()
        {
            Save();
        }

        public void Save()
        {
            if (timeline.IsReverse || timeline.IsSnap)
            {
                timeline.RestoreIfValue(this.entityComponent.identifier, ref Health);
            }
            else
            {
                timeline.Remember(this.entityComponent.identifier, Health);
            }
        }
    }
}
