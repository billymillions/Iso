using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using UnityEngine.InputSystem;
using System.Collections.Generic;

namespace TimelineIso
{
    public class ContiguousInputSave : MonoBehaviour
    {
        private Timeline timeline;
        private InputBuffer inputBuffer;
        private PlayerInput playerInput;
        private PlayerInputHandler playerInputHandler;

        //private Tuple<string, string> [] commands = {
        //    Tuple.Create("one", "two"),
        //    Tuple.Create("one", "two"),
        //    Tuple.Create("one", "two"),
        //};

        private string[] commands = { "Charge", "Shoot", "Dash", "Attack" };
        private string[] vectorCommands = { "Move" };

        private Dictionary<string, object> commandDict = new Dictionary<string, object>();


        // Use this for initialization
        void Start()
        {
            // TODO
            this.timeline = GameObject.Find("Timeline").GetComponent<TimelineMono>().Timeline;
            this.inputBuffer = GameObject.Find("GlobalInputCache").GetComponent<GlobalInputCache>().InputBuffer;
            this.playerInput = GameObject.Find("GlobalInputCache").GetComponent<PlayerInput>();
            // TODO: i hate this
            this.playerInputHandler = GameObject.Find("GlobalInputCache").GetComponent<PlayerInputHandler>();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            var currentPlayer = this.playerInputHandler.CharSelector.Selected.identifier;
            if (this.timeline.IsReverse)
            {
            }
            else
            {
                foreach (var c in commands)
                {
                    var f = this.playerInput.currentActionMap[c].ReadValue<float>();
                    commandDict[c] = f;
                    if (f > 0)
                    {
                        inputBuffer.AddInput(currentPlayer, new ButtonInput { button_name = c, value = f });
                    }
                }
                foreach (var c in vectorCommands)
                {
                    var v = this.playerInput.currentActionMap[c].ReadValue<Vector2>();
                    var move = this.playerInputHandler.GetMove(v);
                    commandDict[c] = move;
                    if (move.magnitude > 0)
                    {
                        inputBuffer.AddInput(currentPlayer, new MoveInput { move = move });
                    }
                }
            }
        }

        public T ReadValue<T>(EntityIdentifier entityId, string command)
        {
            // TODO: optimize
            object result;
            commandDict.TryGetValue(command, out result);
            return (result == null) ? default(T) : (T)result;  // TODO
        }

        public void SetValue<T>(EntityIdentifier id, string command, T value) 
        {
            commandDict[command] = value;
        }


        //void RememberValues()
        //{
        //    var input = this.GetComponent<PlayerInput>();
        //    input.actions.

        //}
    }
}
