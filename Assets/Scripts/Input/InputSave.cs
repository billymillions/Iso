using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using UnityEngine.InputSystem;

namespace TimelineIso
{

    public class InputSave : MonoBehaviour
    {
        private Timeline timeline;
        private InputBuffer inputBuffer;
        private ContiguousInputSave contiguousInput;

        // Use this for initialization
        void Start()
        {
            // TODO
            this.timeline = GameObject.Find("Timeline").GetComponent<TimelineMono>().Timeline;
            this.inputBuffer = GameObject.Find("GlobalInputCache").GetComponent<GlobalInputCache>().InputBuffer;
            this.contiguousInput = GameObject.Find("GlobalInputCache").GetComponent<ContiguousInputSave>();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (this.timeline.IsReverse)
            {
            }
            else
            {
                // TODO clearing / ordering

                foreach (var input in this.inputBuffer.Inputs)
                {
                    // clear any player timeline player inputs if we've received realtime input
                    timeline.ForgetTheFuture(input.identifier);
                }

                // TODO: double add
                foreach (var input in this.timeline.currentFrame.Select(x => x.item).OfType<AssignedInput>())
                {
                    // THEN add timeline inputs to the input buffer
                    this.inputBuffer.AddInput(input.identifier, input.input);
                    if (input.input is ButtonInput)
                    {
                        var b = (ButtonInput)input.input;
                        this.contiguousInput.SetValue(input.identifier, b.button_name, b.value);
                    }
                }

                foreach (var input in this.inputBuffer.Inputs)
                {
                    // THEN re-remember inputs
                    this.timeline.Remember(input.identifier, input);
                }

            }
        }

        //void RememberValues()
        //{
        //    var input = this.GetComponent<PlayerInput>();
        //    input.actions.

        //}
    }
}
