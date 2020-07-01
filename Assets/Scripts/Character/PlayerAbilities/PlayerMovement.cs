﻿using UnityEngine;
using System.Linq;

namespace TimelineIso
{
    public class PlayerMovement : MonoBehaviour
    {

        private System.Lazy<InputBuffer> _inputBuffer = new System.Lazy<InputBuffer>(
            () => GameObject.Find("GlobalInputCache").GetComponent<GlobalInputCache>().InputBuffer
        );
        private InputBuffer inputBuffer { get => _inputBuffer.Value; }

        private EntityComponent _entityComponent;
        private Vector3 velocity;
        public Vector3 Velocity { get => velocity; }

        public float MaxSpeed = 10f;
        [HideInInspector]
        public float SpeedMultiplier = 1f;
        public bool RBMove = false;
        public bool RBVel = false;
        public bool ChangeForward = false;

        private EntityIdentifier entityId
        {
            get
            {
                if (!_entityComponent)
                {
                    _entityComponent = this.GetComponent<EntityComponent>();

                }
                return _entityComponent.identifier;
            }
        }



        // Use this for initialization
        void Start()
        {
            this.SpeedMultiplier = 1f;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            //if (this.disable)
            //{
            //    return
            //}
            var moveInputs = this.inputBuffer.GetInputs(entityId).OfType<MoveInput>();

            foreach (var action in moveInputs)
            {
                this.velocity = new Vector3(action.move.x, 0, action.move.z) * MaxSpeed;
            }

            var vel = this.velocity * this.SpeedMultiplier;
            if (vel.magnitude >= 0.05 && this.ChangeForward)
            {
                this.transform.forward = this.velocity.normalized;
            }

            if (this.RBMove)
            {
                var rb = this.GetComponent<Rigidbody>();
                if (vel.magnitude >= 0.05)
                {
                    rb.rotation = (Quaternion.LookRotation(velocity));
                }
                //Debug.Log(transform.forward);
                //rb.MoveRotation(Quaternion.LookRotation(vel));
                //var vv = this.transform.InverseTransformVector(velocity);
                if (SpeedMultiplier > 0)
                {
                    this.GetComponent<Rigidbody>().MovePosition(vel * Time.fixedDeltaTime + rb.position);
                }
            } else if (this.RBVel)
            {
                this.GetComponent<Rigidbody>().velocity = this.velocity;
            } else
            {
                this.transform.Translate(this.velocity * Time.fixedDeltaTime, Space.World);
            }
            if (vel.magnitude >= 0.05) { 
                //{
                //this.GetComponent<Rigidbody>().velocity = this.velocity;
                this.GetComponent<Animator>().SetBool("Running", true);
            }
            else
            {
                this.GetComponent<Animator>().SetBool("Running", false);
            }
            var locked = GetComponent<PlayerLockon>();
            //if (locked!=null && locked.Locked != null)
            //{
            //    this.transform.forward = (locked.Locked.transform.position - this.transform.position).XZPlane().normalized;
            //} else if (vel.magnitude >= 0.05)
            //{
            //}
        }
        

        public void Nudge(float multiplier)
        {
            // TODO: consolidate with update.... and make sure move input is computed first
            var vel = this.velocity * multiplier;
            if (vel.magnitude >= 0.05)
            {
                this.GetComponent<Rigidbody>().velocity = this.velocity;
                this.transform.forward = this.velocity.normalized;
            }
        }

        //private void Move(Vector3 velocity)
        //{
        //    var moveInputs = this.inputBuffer.GetInputs(entityId).OfType<MoveInput>();

        //    foreach (var action in moveInputs)
        //    {
        //        this.velocity = new Vector3(action.move.x, 0, action.move.z) * MaxSpeed;
        //    }

        //    var vel = this.velocity * this.SpeedMultiplier;
        //    if (vel.magnitude >= 0.05)
        //    {
        //        this.GetComponent<Rigidbody>().velocity = this.velocity;
        //        this.transform.forward = this.velocity.normalized;
        //        this.GetComponent<Animator>().SetBool("Running", true);
        //    }
        //    else
        //    {
        //        this.GetComponent<Animator>().SetBool("Running", false);
        //    }

        //}

    }
}
