using UnityEngine;
using System.Collections;
using UnityEditor;
using Unity.Mathematics;
using System;

namespace TimelineIso
{
    [RequireComponent(typeof(Rigidbody))]
    public class Impulse : MonoBehaviour
    {
        public Vector3 InTheTank;
        public float Multiplier = 10f;
        private Rigidbody rb;
        private bool kinematic;
        public bool Immediate;

        private void Start()
        {
            this.rb = this.GetComponent<Rigidbody>();
            this.kinematic = this.rb.isKinematic;
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
            //this.kinematic = false;
            var startTime = Time.time;
            var remainingDisplacement = vector;
            var totalDisplacement = Vector3.zero;
            var origPosition = this.transform.position;

            this.rb.isKinematic = false;
            for(int i = 0; i < 10; i++)
            {
                yield return new WaitForFixedUpdate();
                var v = vector / 10f;
                this.rb.MovePosition(this.transform.position + v);
                totalDisplacement += v;
                //Debug.Log(v);
                //Debug.Log(this.transform.position);
                //Debug.Log(this.transform.position.x);
            }
            yield return new WaitForFixedUpdate();
            this.rb.isKinematic = true;
            //this.kinematic = true;
            Debug.Log(vector.magnitude);
            Debug.Log(totalDisplacement.magnitude);
            Debug.Log((totalDisplacement - vector).magnitude);
            Debug.Log((this.transform.position - origPosition).magnitude);
            //while (totalDisplacement.magnitude < vector.magnitude)
            //{
            //    var increment = vector * Time.deltaTime / duration;
            //    var remaining = vector - totalDisplacement;
            //    if (remaining.magnitude < increment.magnitude)
            //    {
            //        this.transform.Translate(remaining);
            //        totalDisplacement += remaining;
            //        break;
            //    } else
            //    {
            //        this.transform.Translate(increment);
            //        totalDisplacement += increment;
            //    }
            //    yield return null;
            //    //var translate = (remaining.magnitude < increment.magnitude) ? remaining : increment;
            //    //yield return null;
            //}
            ////transform.Translate(vector - totalDisplacement);
            //Debug.Log(totalDisplacement.magnitude);
            //Debug.Log(vector.magnitude);

            //this.rb.isKinematic = false;
            //while (remainingDisplacement.magnitude >= 0)
            //{
            //    var increment = vector * Time.deltaTime / duration;
            //    var translate = (increment.magnitude > remainingDisplacement.magnitude) ? remainingDisplacement : increment;
            //    remainingDisplacement -= translate;
            //    this.transform.Translate(translate);

            //    //this.rb.MovePosition(this.rb.position + translate);
            //    yield return null;
            //}

            //this.rb.isKinematic = true;
        }

    }
}
