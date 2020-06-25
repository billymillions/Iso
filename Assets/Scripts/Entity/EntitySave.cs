using UnityEngine;
using System.Collections;
using System;

namespace TimelineIso
{

    [RequireComponent(typeof(EntityComponent))]
    public class EntitySave : MonoBehaviour
    {
        private Timeline timeline;
        private EntityComponent entityComponent;

        // Use this for initialization
        void Start()
        {
            // TODO
            this.timeline = GameObject.Find("Timeline").GetComponent<TimelineMono>().Timeline;
            this.entityComponent = this.GetComponent<EntityComponent>();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            var timelineId = this.entityComponent.identifier;

            // TODO
            if (this.timeline.IsReverse || this.timeline.IsSnap)
            {
                var pos = this.timeline.Restore<Position>(timelineId);
                var rotation = this.timeline.Restore<Rotation>(timelineId);
                this.transform.position = (pos is Position unpackedP) ? unpackedP.p : this.transform.position;
                this.transform.rotation = (rotation is Rotation unpackedR) ? unpackedR.r : this.transform.rotation;
                //this.transform.position = (pos is Velocity unpacked) ? new Vector3(unpacked.x, unpacked.y, unpacked.z) : this.transform.position;
                //this.transform.rotation = (rotation is Rotation unpackedR) ? new Quaternion(unpackedR.x, unpackedR.y, unpackedR.z, unpackedR.w) : this.transform.rotation;
            }
            else
            {
                this.timeline.Replace(timelineId, new Position { p = this.transform.position });
                this.timeline.Replace(timelineId, new Rotation { r = this.transform.rotation });
                //var pos = this.transform.position;
                //var rotation = this.transform.rotation;
                //this.timeline.Remember(timelineId, new Velocity { x = pos.x, y = pos.y, z = pos.z });
                //this.timeline.Remember(timelineId, new Rotation { x = rotation.x, y = rotation.y, z = rotation.z, w = rotation.w });
            }
        }


    }
}
