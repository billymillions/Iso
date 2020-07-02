using UnityEngine;
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
        private Vector3 movementForward;
        public Vector3 Velocity { get => velocity; }
        public Vector3 MovementForward { get => movementForward; }
        public Vector3 TransformForward { get => this.transform.forward; }

        public float MaxSpeed = 10f;
        [HideInInspector]
        public float SpeedMultiplier = 1f;
        private EntityMove em;

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
            this.em = this.GetComponent<EntityMove>();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            //if (this.disable)
            //{
            //    return
            //}
            var moveInputs = this.inputBuffer.GetInputs(entityId).OfType<MoveInput>();

            this.velocity = Vector3.zero;
            foreach (var action in moveInputs)
            {
                this.velocity = new Vector3(action.move.x, 0, action.move.z) * MaxSpeed;
            }

            var vel = (this.velocity * this.SpeedMultiplier).XZPlane();
            if (vel.magnitude > 0.05)
            {
                this.movementForward = vel.normalized;
            }
            this.em.MoveDelta(vel * Time.fixedDeltaTime);
            this.em.SetForward(vel);

            // TODO?
            var locked = GetComponent<PlayerLockon>();
            if (locked != null && locked.Locked != null)
            {
                this.em.SetForward(locked.Locked.transform.position - this.transform.position);
            }
        }
        

        public void Nudge(float multiplier)
        {
            // TODO: consolidate with update.... and make sure move input is computed first
            return;
            //var vel = this.velocity * multiplier;
            //if (vel.magnitude >= 0.05)
            //{
            //    this.GetComponent<Rigidbody>().velocity = this.velocity;
            //    this.transform.forward = this.velocity.normalized;
            //}
        }

    }
}
