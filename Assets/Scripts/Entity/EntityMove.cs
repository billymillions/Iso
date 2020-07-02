using UnityEngine;
using System.Collections;
using System.Xml;
using UnityEngine.InputSystem.EnhancedTouch;

namespace TimelineIso
{
    public enum MovementType
    {
        CharacterController,
        RigidBody,
        Translation,
        Velocity
    }

    public class EntityMove : MonoBehaviour
    {
        [SerializeField]
        MovementType MovementType = MovementType.CharacterController;
        private Rigidbody rb;
        private CharacterController cc;


        // Use this for initialization
        void Start()
        {
            this.rb = this.GetComponent<Rigidbody>();
            this.cc = this.GetComponent<CharacterController>();
        }

        // Update is called once per frame
        public void SetForward(Vector3 forward)
        {
            forward = forward.XZPlane();
            if (forward.magnitude <= 0)
            {
                return;
            }
            forward = forward.normalized;

            if (this.MovementType == MovementType.RigidBody)
            {
                this.rb.rotation = Quaternion.LookRotation(forward);
            }
            else if (this.MovementType == MovementType.Velocity)
            {
                // TODO?
                this.rb.rotation = Quaternion.LookRotation(forward);
            }
            {
                this.transform.forward = forward;
            }
        }

        public void MovePosition(Vector3 pos)
        {
            // TODO: maybe aggregate instead and move on fixedupdate
            if (this.MovementType == MovementType.CharacterController)
            {
                this.transform.position = pos;
            }
            else if (this.MovementType == MovementType.RigidBody)
            {
                this.rb.MovePosition(pos);
            }
            else if (this.MovementType == MovementType.Velocity)
            {
                this.rb.MovePosition(pos);
            }
            else if (this.MovementType == MovementType.Translation)
            {
                this.transform.position = pos;
            }
        }

        public void MoveDelta(Vector3 delta)
        {
            // TODO: maybe aggregate instead and move on fixedupdate
            if (this.MovementType == MovementType.CharacterController)
            {
                var coll = this.cc.Move(delta);
            }
            else if (this.MovementType == MovementType.RigidBody)
            {
                this.rb.MovePosition(this.rb.position + delta);
            }
            else if (this.MovementType == MovementType.Velocity)
            {
                // TODO?
                this.rb.MovePosition(this.rb.position + delta);
            }
            else if (this.MovementType == MovementType.Translation)
            {
                this.transform.Translate(delta, Space.World);
            }
        }
        public void MoveVelocity(Vector3 velocity)
        {
            // TODO?
            return;
        }
    }
}
