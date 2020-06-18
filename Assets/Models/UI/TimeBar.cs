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
            rect.localScale = new Vector3(1f * timeline.CurrentIndex / timeline.MaxIndex, rect.localScale.y, rect.localScale.z);

        }
    }
}
