using UnityEngine;
using System.Collections;


namespace TimelineIso
{
    [RequireComponent(typeof(EntityComponent))]
    public class TimelineEntity : MonoBehaviour
    {
        public Timeline Timeline { get; private set; }
        private EntityComponent EntityComponent;

        // Use this for initialization
        private void Awake()
        {
            this.Timeline = GameObject.Find("Timeline").GetComponent<TimelineMono>().Timeline;
            this.EntityComponent = this.GetComponent<EntityComponent>();
        }

        void FixedUpdate()
        {
            var timelineId = this.EntityComponent.identifier;

            if (this.Timeline.IsReverse)
            {
                var pos = this.Timeline.Restore<Position>(timelineId);
                var rotation = this.Timeline.Restore<Rotation>(timelineId);
                this.transform.position = (pos is Position unpackedP) ? unpackedP.p : this.transform.position;
                this.transform.rotation = (rotation is Rotation unpackedR) ? unpackedR.r : this.transform.rotation;
            }
            else
            {
                this.Timeline.Replace(timelineId, new Position { p = this.transform.position });
                this.Timeline.Replace(timelineId, new Rotation { r = this.transform.rotation });
            }
        }

        public void Despawn()
        {
            this.Timeline.Replace(this.EntityComponent.identifier, new Despawn { obj = this.gameObject });
            this.gameObject.SetActive(false);
        }

        public T Spawn<T>(T prefab, Vector3 position, Quaternion rotation) where T: EntityComponent
        {
            var obj = Instantiate(prefab, position, rotation);
            obj.identifier = EntityIdentifier.GetNew();
            this.Timeline.Replace(obj.identifier, new Spawn { obj = obj.gameObject });
            return obj;
        }
    }
}
