using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace TimelineIso
{
    public class TimeBar : MonoBehaviour
    {
        private Timeline timeline;

        // Start is called before the first frame update
        void Start()
        {


        }

        // Update is called once per frame
        void Update()
        {
            this.timeline = GameObject.Find("Timeline").GetComponent<TimelineMono>().Timeline;
            var rect = this.GetComponent<RectTransform>();
            var indexOffset = (timeline.CurrentIndex + timeline.TimelineSize - timeline.StartIndex) % timeline.TimelineSize;
            rect.localScale = new Vector3(((float)indexOffset) / timeline.TimelineSize, rect.localScale.y, rect.localScale.z);

        }
    }
}
