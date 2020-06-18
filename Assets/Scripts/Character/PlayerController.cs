using UnityEngine;
using System.Collections;
using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;

namespace TimelineIso
{

    [RequireComponent(typeof(EntityComponent))]
    public class PlayerController : MonoBehaviour
    {
        public float scheduleExpiry = .5f;

        private InputBuffer inputBuffer;
        private ContiguousInput contiguousInput;
        public Enemy locked;

        public TimelineEntity TimelineEntity { get; private set; }

        [SerializeField]
        private Vector3 lok;
        [SerializeField]
        public Transform ArrowSpawn;
        [SerializeField]
        public Arrow ArrowPrefab;

        [SerializeField]
        private float mag;


        private PlayerAbility playerSwordSlash;
        private PlayerAbility currentAbility;

        public bool IsBusy { get => (currentAbility != null && currentAbility.Status() == PlayerAbilityStatus.Running); }


        private List<ScheduledInput> scheduledInput = new List<ScheduledInput>();

        void Start()
        {
            this.inputBuffer = GameObject.Find("GlobalInputCache").GetComponent<GlobalInputCache>().InputBuffer;
            this.contiguousInput = GameObject.Find("GlobalInputCache").GetComponent<GlobalInputCache>().ContiguousInput;
            this.locked = null;
            this.TimelineEntity = GetComponent<TimelineEntity>();
            //this.playerSwordSlash = this.PSST.Initialize(this.gameObject);
        }

        private void Update()
        {

            
        }
        public void FinishAbility()
        {
            if (this.currentAbility != null)
            {
                this.currentAbility.Finish();
                this.currentAbility = null;
            }

        }

        private float ExpireTime(IInputEvent input, float time)
        {
            // TODO: classify non-event value inputs
            return (input is MoveInput || input is LookInput) ? time : time + scheduleExpiry;
        }

        IEnumerable<ScheduledInput> GetInputs()
        {
            // TODO: handle any continuous input
            // TODO: handle priority of commands
            // TODO: don't remake scheduled input each time
            var entityId = this.GetComponent<EntityComponent>().identifier;
            var time = Time.time;
            var first = this.scheduledInput.Where(x => x.expireTime > time);
            var next = this.inputBuffer.GetInputs(entityId).Select(
                (x) => new ScheduledInput { scheduledTime = time, expireTime = this.ExpireTime(x, time), input = x }
            );

            return first.Concat(next);
        }

        void PruneInputs()
        {
            // this is a terrible method.
            var containsRelease = this.scheduledInput.Any((x) => x.input is ChargeInput && ((ChargeInput)x.input).isRelease);
            if (containsRelease)
            {
                this.scheduledInput = this.scheduledInput.Where((x) => !(x.input is ChargeInput)).ToList();
            }
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            var entityId = this.GetComponent<EntityComponent>().identifier;
            var moveInputs = this.inputBuffer.GetInputs(entityId).OfType<MoveInput>();
            var lookInputs = this.inputBuffer.GetInputs(entityId).OfType<LookInput>();
            var attackInputs = this.inputBuffer.GetInputs(entityId).OfType<AttackInput>();
            var shootInputs = this.inputBuffer.GetInputs(entityId).OfType<ShootInput>();
            var chargeActions = this.inputBuffer.GetInputs(entityId).OfType<ChargeInput>().ToList();
            var dashInputs = this.inputBuffer.GetInputs(entityId).OfType<DashInput>();

            if (this.currentAbility != null && this.currentAbility.Status() != PlayerAbilityStatus.Running)
            {
                this.currentAbility.Finish(); // uhh
                this.currentAbility = null;
            }

            var scheduled = this.GetInputs();

            // TODO: don't remake list every time
            this.scheduledInput = new List<ScheduledInput>();

            // send trigger to current ability
            foreach (var s in scheduled)
            {
                var action = s.input;
                if (this.currentAbility == null)
                {
                    // TODO, action lookup
                    if (action is AttackInput)
                    {
                        this.currentAbility = this.GetComponent<PlayerSlashComponent>();
                    } else if (action is DashInput)
                    {
                        this.currentAbility = this.GetComponent<PlayerDashComponent>();
                    }
                     else if (action is ButtonInput)
                    {
                        this.currentAbility = this.GetComponent<PlayerJumpComponentValueDriven>();
                        //break;
                    } else
                    {
                        continue;
                    }
                    this.currentAbility.Initialize();
                }
                var result = this.currentAbility.HandleInput(action);

                // TODO permit
                if (result != InputHandledStatus.Handled)
                {
                    this.scheduledInput.Add(s);
                }
            }
            // PruneInputs(); // jesus

            var go = this.gameObject.transform.Find("Debug");
            var pi = GameObject.Find("GlobalInputCache").GetComponent<PlayerInput>();

            if (pi.currentActionMap["Charge"].ReadValue<float>() > 0)
            {
                go.gameObject.SetActive(true);
            } else {
                go.gameObject.SetActive(false);
            }
               
            //if (this.contiguousInput.Read<float>("Charge", entityId) > 0)
            //{
            //    go.gameObject.SetActive(true);
            //} else {
            //    go.gameObject.SetActive(false);
            //}
        }

        private void OnDrawGizmos()
        {
            var entityId = this.GetComponent<EntityComponent>().identifier;
            if (this.contiguousInput==null)
            {
                return;
            }
            if (this.contiguousInput.Read<float>("Charge", entityId) > 0)
            {
                Debug.Log("here");
                Gizmos.DrawIcon(new Vector3(0, 1, 0), "icon.png");
            };

        }

        void LockOn(Vector3 look)
        {
            if (look.magnitude < .8)
            {
                return;
            }

            var normalized = look.normalized;
            var min = .8;
            Enemy newLock = null;
            this.lok = look;

            foreach (var enemy in GameObject.FindObjectsOfType<Enemy>())
            {
                var enemyRay = (enemy.transform.position - this.transform.position).XZPlane().normalized;
                var dot = Vector3.Dot(look, enemyRay);
                this.mag = dot;
                if (dot > min)
                {
                    newLock = enemy;
                }
            }
            this.locked = newLock;
            if (newLock)
            {
                this.GetComponent<Animator>().SetBool("Locked", true);
            } else
            {
                this.GetComponent<Animator>().SetBool("Locked", false);
            }

        }


        void TriggerShoot()
        {
            this.GetComponent<Animator>().Play("PlayerArrow");
        }

        void Shoot()
        {
            if (this.TimelineEntity)
            {
                var ec = this.ArrowPrefab.GetComponent<EntityComponent>();
                var obj = this.TimelineEntity.Spawn(ec, this.ArrowSpawn.position, this.ArrowSpawn.rotation);
                obj.GetComponent<Arrow>().speed = 100f;
            }
            //var obj = Instantiate(this.ArrowPrefab, this.ArrowSpawn.position, this.ArrowSpawn.rotation);
            //obj.velocity = this.ArrowSpawn.forward.normalized * 100f;
            //StartCoroutine(ArrowCleanup(obj.gameObject));
        }

        IEnumerator ArrowCleanup(GameObject obj)
        {
            yield return new WaitForSeconds(1);
            Destroy(obj);
        }

    }
}
