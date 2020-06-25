using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Events;

namespace TimelineIso
{
    public struct ChargeResult
    {
        public float progress;
        public bool complete;
        public ChargeEffect effect;
    }

    public class ChargeEvent: UnityEvent<ChargeResult> { }

    public class ChargeEffect : MonoBehaviour
    {
        // Start is called before the first frame update
        public UnityEvent<ChargeResult> OnCharged = new ChargeEvent();
        public EntityIdentifier Identifier;
        public string Command;
        public float Scale;
        public float Duration;

        private float startTime;
        private ContiguousInputSave input;

        private void Start()
        {
            this.input = GameObject.Find("GlobalInputCache").GetComponent<ContiguousInputSave>();
            var renderer = this.GetComponent<Renderer>();
            this.startTime = Time.time;
            renderer.material.SetFloat("_StartTime", this.startTime);
            renderer.material.SetFloat("_ChargeTime", this.Duration);
            this.transform.localScale = Vector3.one * Scale;
            this.startTime = Time.time;
        }

        public void Initialize(EntityIdentifier identifier, string command, float scale, float duration)
        {
            this.Identifier = identifier;
            this.Command = command;
            this.Scale = scale;
            this.Duration = duration;
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void FixedUpdate()
        {
            var duration = Time.time - startTime;
            if (this.input.ReadValue<float>(Identifier, Command) <= .2)
            {
                var progress = Mathf.Min(duration / Duration, 1f);
                OnCharged.Invoke(new ChargeResult { complete = true, progress = progress, effect = this});
                Destroy(this.gameObject);
            }
        }
    }
 }
