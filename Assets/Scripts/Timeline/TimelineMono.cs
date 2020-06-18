using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimelineIso
{
    public class TimelineMono : MonoBehaviour
    {
        public Timeline Timeline;

        void Awake()
        {
            Timeline = new Timeline();
        }

        private void FixedUpdate()
        {
            // TODO: this is awful
            Timeline.IsSnap = false;
        }

        public static Timeline GetForScene()
        {
            return GameObject.Find("Timeline").GetComponent<TimelineMono>().Timeline;
        }
    }
}