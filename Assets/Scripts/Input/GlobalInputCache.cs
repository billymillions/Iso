using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

namespace TimelineIso
{
    public class GlobalInputCache : MonoBehaviour
    {
        [SerializeField]
        public InputBuffer InputBuffer;
        public ContiguousInput ContiguousInput;
        [SerializeField] private int cacheSize;

        // Use this for initialization
        void Awake()
        {
            InputBuffer = new InputBuffer();
            ContiguousInput = new ContiguousInput(this.GetComponent<PlayerInput>());
        }

        private void FixedUpdate()
        {
            // this clears inputs... should be the last system to run in the frame
            InputBuffer.ClearInputs();
        }
    }
}