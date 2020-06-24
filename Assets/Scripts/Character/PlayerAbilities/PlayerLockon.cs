using UnityEngine;
using System.Linq;

namespace TimelineIso
{
    public class PlayerLockon : MonoBehaviour
    {

        private System.Lazy<InputBuffer> _inputBuffer = new System.Lazy<InputBuffer>(
    () => GameObject.Find("GlobalInputCache").GetComponent<GlobalInputCache>().InputBuffer
);
        private InputBuffer inputBuffer { get => _inputBuffer.Value; }

        private EntityComponent _entityComponent;


        private Enemy locked;
        public Enemy Locked { get => locked; }

        private EntityIdentifier entityId
        {
            get
            {
                if (!_entityComponent)
                {
                    _entityComponent = this.GetComponent<EntityComponent>();

                }
                return _entityComponent.identifier;
            }
        }


        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void FixedUpdate()
        {
            var lookInputs = this.inputBuffer.GetInputs(entityId).OfType<LookInput>();
            foreach (var input in lookInputs)
            {
                LockOn(input.look);
            }
        }

        public Enemy ClosestSighted(Vector3 look)
        {
            var normalized = look.normalized;
            var min = -10000;
            Enemy newLock = null;

            foreach (var enemy in GameObject.FindObjectsOfType<Enemy>())
            {
                var enemyRay = (enemy.transform.position - this.transform.position).XZPlane().normalized;
                var dot = Vector3.Dot(normalized, enemyRay);
                if (dot > min)
                {
                    newLock = enemy;
                }
            }
            return newLock;
        }


        public void LockOn(Vector3 look)
        {
            if (look.magnitude < .8)
            {
                return;
            }

            var normalized = look.normalized;
            var min = .8;
            Enemy newLock = null;

            foreach (var enemy in GameObject.FindObjectsOfType<Enemy>())
            {
                var enemyRay = (enemy.transform.position - this.transform.position).XZPlane().normalized;
                var dot = Vector3.Dot(normalized, enemyRay);
                if (dot > min)
                {
                    newLock = enemy;
                }
            }
            this.locked = newLock;
            //if (newLock)
            //{
            //    this.GetComponent<Animator>().SetBool("Locked", true);
            //}
            //else
            //{
            //    this.GetComponent<Animator>().SetBool("Locked", false);
            //}

        }

    }
}
