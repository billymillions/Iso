﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

namespace TimelineIso
{
    public class Missile : MonoBehaviour
    {
        public Vector3 Direction; 
        public GameObject Target;
        public float Speed = 10f;
        public float Handling = 1f;
        public float Falloff = 10f;
        private Rigidbody rb;

        public void Spawn(Vector3 location, Vector3 direction, GameObject target)
        {
            var obj = Instantiate(this, location, Quaternion.LookRotation(direction));
            obj.Target = target;
        }

        // Start is called before the first frame update
        void Start()
        {
            this.rb = this.GetComponent<Rigidbody>();
            this.GetComponent<TrailRenderer>().material.SetFloat("_Seed", Time.time);
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (this.Target == null)
            {
                return;
            }

            var look = (this.Target.transform.position - this.transform.position);
            look = look.normalized / Mathf.Min(Falloff, look.magnitude);

            //this.rb.velocity = Vector3.RotateTowards(rb.velocity.normalized, look, Handling * Time.deltaTime, Speed);
            this.rb.velocity = this.rb.velocity.normalized + Handling * Time.deltaTime * look;
            this.rb.velocity = this.rb.velocity.normalized * Speed;

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                other.gameObject.GetComponent<CharacterHealth>().InflictDamage(10);
            }
            Destroy(this.gameObject);
        }
    }
}
