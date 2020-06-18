using UnityEngine;
using System.Collections;

namespace TimelineIso
{
    public class TimelineScrub : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void FixedUpdate()
        {
            var timeline = GameObject.Find("Timeline").GetComponent<TimelineMono>().Timeline;
            if (timeline.IsReverse)
            {
                timeline.FallBack();
            }
            else
            {
                timeline.Advance();
            }
        }
    }
}