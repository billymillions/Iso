using Boo.Lang;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimelineIso
{
    public class ExplosionCollider : MonoBehaviour
    {
        public float BaseDamage = 100f;
        public float DamageMultiplier = 1f;
        private HashSet<Collider> collisions;
        private void Start()
        {
            collisions = new HashSet<Collider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy") && !collisions.Contains(other))
            {
                collisions.Add(other);
                other.GetComponent<CharacterHealth>().InflictDamage(BaseDamage * DamageMultiplier);
            }
        }
    }
}
