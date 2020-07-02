using UnityEngine;
using System.Collections;
using UnityEditor;
using Boo.Lang;
using System;

namespace TimelineIso
{
    public class CameraFollow : MonoBehaviour
    {
        public List<Transform> Follows; // TODO: different weights
        public CharacterSelector characterSelector;
        public Vector3 characterFollowDistance = new Vector3(-10, 20, -10);
        public float distanceThreshold = .00001f;
        public float minspeed = 4f;
        public float maxspeed = 4f;
        public float speed = 4f;
        public bool forgetIt = false;
        private Vector3 m_LastTargetPosition;
        private Vector3 m_CurrentVelocity;
        private Vector3 m_LookAheadPos;
        public float damping;
        private float sizeSpd;
        public float maxSize = 20;
        public float minSize = 10;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (forgetIt)
            {
                return;
            }
            BetterUpdate();
        }

        void BetterUpdate()
        {

            var cameraDist = 40f;
            var positions = new List<Vector4>();

            var pos = characterSelector.SelectedCharacterTransform.position;
            positions.Add(new Vector4(pos.x, pos.y, pos.z, 5));
            // TODO
            //if (characterSelector.Selected.GetComponent<PlayerController>().locked)
            //{
            //    var p = characterSelector.Selected.GetComponent<PlayerController>().locked.transform.position;
            //    positions.Add(new Vector4(p.x, p.y, p.z, 1));
            //}

            var average = new Vector3();
            var denom = 0f;
            foreach(var p in positions)
            {
                average += new Vector3(p.x, p.y, p.z) * p.w;
                denom += p.w;
            }

            average /= denom;
            var camera = GetComponent<Camera>();
            var camPoint = camera.WorldToViewportPoint(average);
            var maxD = minSize;

            foreach (var p in positions)
            {
                var screen = camera.WorldToViewportPoint(new Vector3(p.x, p.y, p.z));
                maxD = Math.Max((Math.Abs(screen.x - camPoint.x) + .1f) * 2 * camera.orthographicSize, maxD);
                maxD = Math.Max((Math.Abs(screen.y - camPoint.y) + .1f) * 2 * camera.orthographicSize, maxD);
            }

            maxD = Math.Min(maxSize, maxD);

            var centerPoint = new Vector3(.5f, .5f, cameraDist);
            var lineDiff = camera.ViewportToWorldPoint(centerPoint);
            Debug.DrawLine(lineDiff, average);



            var intendedCameraPosition = this.transform.position + (average - lineDiff);
            //m_LookAheadPos = Vector3.MoveTowards(this.transform.position, intendedCameraPosition, Time.deltaTime * maxspeed);
            Vector3 newPos = Vector3.SmoothDamp(transform.position, intendedCameraPosition, ref m_CurrentVelocity, damping);
            var newSize = Mathf.SmoothDamp(camera.orthographicSize, maxD, ref sizeSpd, damping);
            //.smooth.SmoothDamp(transform.position, m_LookAheadPos, ref m_CurrentVelocity, damping);

            transform.position = newPos;
            camera.orthographicSize = newSize;


        }
    }
}
