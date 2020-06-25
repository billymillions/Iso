using UnityEngine;
using System.Collections;
using UnityEngine.Events;

namespace TimelineIso
{

    [System.Serializable]
    public class CollideEvent : UnityEvent<Collider> { }

    public class EventColliderComponent : MonoBehaviour
    {
        public UnityEvent<Collider> OnCollide = new CollideEvent();
        public float Duration = 0f;

        // Update is called once per frame
        private void OnTriggerEnter(Collider other)
        {
            OnCollide.Invoke(other);
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
