using UnityEngine;
using System.Collections;

namespace TimelineIso
{
    public class WaitForFixedSeconds : IEnumerator
    {
        private float startTimeFixed;
        private float endTimeFixed;
        private float durationFixed;
        private int count=0;
        private int num;

        public WaitForFixedSeconds(float duration)
        {
            //this.startTimeFixed = Time.fixedTime;

            this.num = (int)(duration / Time.fixedDeltaTime);
            //this.durationFixed = duration - Time.fixedDeltaTime;
            //this.endTimeFixed = this.startTimeFixed + this.durationFixed;
        }

        public void Reset() { count = 0; }

        private IEnumerator WaitBro()
        {
            for (; count < num; this.count += 1)
            {
                yield return new WaitForFixedUpdate();
            }
        }

        public object Current { get
            {
                count += 1;
                return new WaitForFixedUpdate();
            }
        }

        public bool MoveNext()
        {
            return count < num;
            //return (Time.fixedTime < endTimeFixed + Time.fixedDeltaTime);
        }
    }
}
