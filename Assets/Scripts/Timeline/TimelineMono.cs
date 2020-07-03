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
            if (Timeline.IsPrepare)
            {
                Timeline.IsPrepare = false;
                Timeline.IsSnap = true;
                Timeline.CurrentIndex = Timeline.StartIndex;
            }
            else if (Timeline.IsSnap)
            {
                Timeline.IsSnap = false;
                //Timeline.SnapBack();
            }
            else if (Timeline.IsReverse)
            {
                Timeline.FallBack();
            }
            else
            {
                Timeline.Advance();
            }
        }

        public static Timeline GetForScene()
        {
            return GameObject.Find("Timeline").GetComponent<TimelineMono>().Timeline;
        }
    }
}