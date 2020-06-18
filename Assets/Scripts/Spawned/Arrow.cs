using UnityEngine;
using System.Collections;


namespace TimelineIso
{

    [RequireComponent(typeof(TimelineEntity))]
    [RequireComponent(typeof(Rigidbody))]
    public class Arrow : MonoBehaviour
    {
        public Transform target;
        public float speed;

        private TimelineEntity TimelineEntity;
        private Rigidbody RigidBody;

        public int End { get; private set; }

        // Use this for initialization
        void Start()
        {
            this.TimelineEntity = GetComponent<TimelineEntity>(); ;
            this.RigidBody = this.GetComponent<Rigidbody>();
            this.RigidBody.velocity = this.transform.forward.normalized * speed;
            this.End = this.TimelineEntity.Timeline.CurrentIndex + 50;  // TODO: frame independent
        }

        void FixedUpdate()
        {
            // TODO: frame independent
            // TODO: don't check every frame ??
            if (this.TimelineEntity.Timeline.CurrentIndex >= this.End)
            {
                this.TimelineEntity.Despawn();
            }
        }
    }
}
