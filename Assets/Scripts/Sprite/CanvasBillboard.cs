using UnityEngine;
using System.Collections;

namespace TimelineIso
{
    public class CanvasBillboard : MonoBehaviour
    {
        private Vector3 origPosition;

        // Use this for initialization
        void Start()
        {
            this.origPosition = this.transform.localPosition;
        }

        // Update is called once per frame
        void Update()
        {
            this.transform.forward = Camera.main.transform.forward;
            var forward = this.transform.parent.InverseTransformVector(Camera.main.transform.forward);
            this.transform.localPosition = this.origPosition - 2 * forward;
        }
    }
}
