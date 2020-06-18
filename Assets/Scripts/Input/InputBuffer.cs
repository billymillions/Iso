using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem;

namespace TimelineIso
{
    [System.Serializable]
    public class InputBuffer
    {

        // TODO: use queue or list ???
        public List<AssignedInput> inputs;
        public int whatever = 0;


        public InputBuffer()
        {
            inputs = new List<AssignedInput>();
        }

        public IEnumerable<AssignedInput> Inputs { get => inputs; }

        public void AddInput(AssignedInput input)
        {
            inputs.Add(input);
        }
        public void AddInput(EntityIdentifier id, IInputEvent ia)
        {
            inputs.Add(new AssignedInput { identifier = id, input = ia });
        }

        public void ClearInputs()
        {
            inputs.Clear();
        }

        public IEnumerable<IInputEvent> GetInputs(EntityIdentifier id)
        {
            return inputs.Where(x => x.identifier.Equals(id)).Select(x => x.input);
        }
    }
}