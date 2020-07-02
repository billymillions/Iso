using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;

namespace TimelineIso
{

    [System.Serializable]
    public class CollideEvent : UnityEvent<Collider> { }

    public class EventColliderComponent : MonoBehaviour
    {
        public UnityEvent<Collider> OnCollide = new CollideEvent();
        public float Duration = 0f;
        private HashSet<GameObject> collisions = new HashSet<GameObject>();

        // Update is called once per frame
        private void OnTriggerEnter(Collider other)
        {
            if (!collisions.Contains(other.gameObject))
            {
                collisions.Add(other.gameObject);
                OnCollide.Invoke(other);
            }
        }

        private void Start()
        {
            if (Duration > 0)
            {
                StartCoroutine(Destruct());
            }
        }

        private IEnumerator Destruct()
        {
            yield return new WaitForSeconds(Duration);
            Destroy(this.gameObject);
        }
    }
}
