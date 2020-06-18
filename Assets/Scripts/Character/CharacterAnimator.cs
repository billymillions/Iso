using UnityEngine;

namespace TimelineIso
{

    // THIS is temporarily good enough. does not support play-reverse-play-reverse as the first recording will be lost
    public class CharacterAnimator : MonoBehaviour
    {
        private Timeline timeline;
        private Animator animator;
        private bool forward = false;

        void Start()
        {
            // TODO
            this.timeline = GameObject.Find("Timeline").GetComponent<TimelineMono>().Timeline;
            this.animator = this.GetComponent<Animator>();
            //this.animator.StartRecording(10000);

        }

        private void Update()
        {
            if (this.forward == false)
            {
                this.animator.playbackTime -= Time.deltaTime;
            }

        }

        void FixedUpdate()
        {
            var animator = this.GetComponent<Animator>();
            var timelineId = this.GetComponent<EntityComponent>().identifier;

            if (this.timeline.IsReverse)
            {
                if (this.forward)
                {
                    this.forward = false;
                    animator.StopRecording();
                    animator.StartPlayback();
                    animator.speed = 0;
                    this.animator.playbackTime = this.animator.recorderStopTime;
                        //this.animator.recorderStopTime;
                }
                //this.animator.playbackTime += Time.deltaTime;
                    //animator.Play(unpacked.stateHash, unpacked.layer, unpacked.normalizedTime);
                //var ai = this.timeline.Restore<AnimationInfo>(timelineId);
                //if (ai is AnimationInfo unpacked) {
                //}
            }
            else 
            {
                if (!this.forward)
                {
                    this.forward = true;
                    //var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                    animator.speed = 1;
                    animator.StopPlayback();
                    animator.StartRecording(10000);
                }
                //this.timeline.Replace(
                //    timelineId,
                //    new AnimationInfo { stateHash = stateInfo.shortNameHash, layer = 0, normalizedTime = stateInfo.normalizedTime });
            }
        }
    }
}
