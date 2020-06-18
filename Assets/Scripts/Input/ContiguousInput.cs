using UnityEngine;
using System.Collections;
using System.Linq;
using System;
using UnityEngine.InputSystem;

namespace TimelineIso
{
    //public class ContiguousInput
    //{
    //    private Timeline
    //    public void 

    //}

    public class ContiguousInput
    {
        private PlayerInput playerInput;
        public ContiguousInput(PlayerInput playerInput) { this.playerInput = playerInput; }

        public T Read<T>(string name, EntityIdentifier id) where T: struct
        {
            // TODO: id
            return this.playerInput.currentActionMap[name].ReadValue<T>();
        }
    }
}
