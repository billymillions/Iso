using System.Runtime.Remoting.Messaging;
using Unity.Mathematics;
using UnityEngine;

namespace TimelineIso
{

    public interface IInputEvent
    {
    }


    [System.Serializable]
    public struct AssignedInput
    {
        public EntityIdentifier identifier;
        [SerializeReference]
        public IInputEvent input;
    }

    [System.Serializable]
    public struct ButtonInput: IInputEvent
    {
        public string button_name; // or a hash or some shit
        public bool is_press;
        public bool is_release;
        public float value;
        public bool is_on()
        {
            return this.value >= .5;
        }
    }

    [System.Serializable]
    public struct MoveInput : IInputEvent
    {
        public float3 move;
    }

    [System.Serializable]
    public struct LookInput : IInputEvent
    {
        public float3 look;
    }

    [System.Serializable]
    public struct AttackInput : IInputEvent
    {
    }

    [System.Serializable]
    public struct ShootInput : IInputEvent
    {
    }

    [System.Serializable]
    public struct DashInput : IInputEvent
    {
    }

    [System.Serializable]
    public struct ChargeInput : IInputEvent
    {
        public bool isRelease;
    }

}
