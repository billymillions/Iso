using UnityEngine;
using System.Collections;
using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;

namespace TimelineIso
{
    [System.Serializable]
    struct AssignedAbility
    {
        public string commandName;
        [SerializeReference]
        public PlayerAbilityComponent ability;
    }

    [RequireComponent(typeof(EntityComponent))]
    public class PlayerController : MonoBehaviour
    {
        public float scheduleExpiry = .5f;

        private InputBuffer inputBuffer;
        private ContiguousInput contiguousInput;

        public TimelineEntity TimelineEntity { get; private set; }

        [SerializeField]
        private AssignedAbility[] abilities;
        private Dictionary<string, PlayerAbility> abilityDict = new Dictionary<string, PlayerAbility>();
        private PlayerAbility currentAbility;
        public bool IsBusy { get => (currentAbility != null && currentAbility.Status() == PlayerAbilityStatus.Running); }
        private List<ScheduledInput> scheduledInput = new List<ScheduledInput>();

        void Start()
        {
            this.inputBuffer = GameObject.Find("GlobalInputCache").GetComponent<GlobalInputCache>().InputBuffer;
            this.contiguousInput = GameObject.Find("GlobalInputCache").GetComponent<GlobalInputCache>().ContiguousInput;
            this.TimelineEntity = GetComponent<TimelineEntity>();

            foreach (var aa in abilities)
            {
                this.abilityDict[aa.commandName] = aa.ability;
            }
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

        CommandInput LookUpCommand(ScheduledInput si)
        {
            if (si.input is ButtonInput b)
            {
                var ability = abilityDict[b.button_name];
                return new CommandInput { button = b, targetAbility = ability};
            }
            return new CommandInput{ };
        } 

        PlayerAbility LookUpAbility(ScheduledInput si)
        {
            if (si.input is ButtonInput b)
            {
                return abilityDict[b.button_name];
            }
            return null;
        } 

        // Update is called once per frame
        void FixedUpdate()
        {
            var entityId = this.GetComponent<EntityComponent>().identifier;

            if (this.currentAbility != null && this.currentAbility.Status() != PlayerAbilityStatus.Running)
            {
                this.currentAbility.Finish(); // uhh
                this.currentAbility = null;
            }

            // TODO: don't remake list every time
            var scheduled = this.GetInputs();
            this.scheduledInput = new List<ScheduledInput>();

            // send input to current ability
            foreach (var s in scheduled)
            {
                var action = s.input;
                var status = InputHandledStatus.Handled;
                var command = LookUpCommand(s);
                if (this.currentAbility == null)
                {
                    this.currentAbility = command.targetAbility;
                }
                if (this.currentAbility != null)
                {
                    status = this.currentAbility.HandleInput(command);
                }
                if (status != InputHandledStatus.Handled)
                {
                    // reschedule
                    this.scheduledInput.Add(s);
                }
            }
        }
    }
}
