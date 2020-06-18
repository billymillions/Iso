using UnityEngine;
using System.Collections;
using System.Linq;

namespace TimelineIso
{
    public struct Despawn
    {
        public GameObject obj;
    }
    
    public struct Spawn
    {
        public GameObject obj;
    }

    [RequireComponent(typeof(TimelineMono))]
    public class TimelineLifecycle : MonoBehaviour
    {
        private Timeline Timeline;

        // Use this for initialization
        void Start()
        {
            this.Timeline = this.GetComponent<TimelineMono>().Timeline;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (this.Timeline.IsReverse)
            {
                foreach(TimelineEvent te in this.Timeline.currentFrame.Where(x => x.item is Despawn))
                {
                    HandleDespawn(te.id, (Despawn)te.item);
                }
                
                foreach(TimelineEvent te in this.Timeline.currentFrame.Where(x => x.item is Spawn))
                {
                    HandleSpawn(te.id, (Spawn)te.item);
                }
            }
        }

        void HandleDespawn(EntityIdentifier id, Despawn ds)
        {
            ds.obj.SetActive(true);
        }

        void HandleSpawn(EntityIdentifier id, Spawn s)
        {
            Destroy(s.obj);
            this.Timeline.ForgetTheFuture(id);
        }
    }
}
