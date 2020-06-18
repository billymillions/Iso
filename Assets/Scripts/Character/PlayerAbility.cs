using UnityEngine;
using System.Collections;

namespace TimelineIso
{

    public abstract class PlayerAbilityTemplate: ScriptableObject
    {
        public abstract PlayerAbility Initialize(GameObject go);
    }

    public enum PlayerAbilityStatus
    {
        Running,
        Cancelled,
        Finished
    }

    public enum InputHandledStatus
    {
        Handled,
        Permit,
        Deny,
    }

    public struct ScheduledInput
    {
        public float scheduledTime;
        public float expireTime;
        public IInputEvent input;
    }

    public interface PlayerAbility
    {
        // NOTE: this could instead be separated into the ability class and the execution class, with Initialize returning
        // an execution object
        // BUT this sucks hard for handling animation events and other things bound to component methods
        void Initialize();
        PlayerAbilityStatus Status();
        InputHandledStatus HandleInput(IInputEvent input);
        void Finish();
    }

    //public interface PlayerAbilityStatus
    //{


    //}
}
