using UnityEngine;
using System.Collections;
using UnityEditor;
using Unity.Mathematics;
using System;

namespace TimelineIso
{
    //[RequireComponent(typeof(Rigidbody))]
    public class Impulse : MonoBehaviour
    {
        public Vector3 InTheTank;
        public float Multiplier = 10f;
        private Rigidbody rb;
        private EntityMove em;
        private bool kinematic;
        public bool Immediate;

        private void Start()
        {
            this.rb = this.GetComponent<Rigidbody>();
            this.em = this.GetComponent<EntityMove>();
        }


        public void Displace(Vector3 vector, float duration)
        {
            if (this.Immediate)
            {
                this.transform.Translate(vector);
            } else
            {
                StartCoroutine(RunDisplace(vector, duration));
            }
        }

        private IEnumerator RunDisplace(Vector3 vector, float duration)
        {
            var frames = Math.Max(1, (int)(duration / Time.fixedDeltaTime));
            var v = vector / frames;

            for(int i = 0; i < frames; i++)
            {
                this.em.MoveDelta(v);
                yield return new WaitForFixedUpdate();
            }
        }
    }
}
