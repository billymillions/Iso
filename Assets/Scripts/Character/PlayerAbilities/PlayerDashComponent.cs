using UnityEngine;
using System.Collections;


namespace TimelineIso
{
    public class PlayerDashComponent : PlayerAbilityComponent
    {
        private Coroutine routine;
        public float DashDuration = .2f;
        public float DashDistance = 5f;

        public override void Initialize()
        {
        }

        public override void Finish()
        {
            if (this.routine !=null)
            {
                StopCoroutine(this.routine);
                this.routine = null;
            }
            this.GetComponent<PlayerMovement>().SpeedMultiplier = 1f;
        }

        public override InputHandledStatus HandleInput(IInputEvent input)
        {
            if (input is CommandInput i && i.targetAbility == this)
            {
                DoDash();
                return InputHandledStatus.Handled;
            }
            return InputHandledStatus.Deny;
        }

        public override PlayerAbilityStatus Status()
        {
            return (this.routine == null) ? PlayerAbilityStatus.Finished : PlayerAbilityStatus.Running;
        }

        void DoDash()
        {
            if (!this.GetComponent<CharacterStamina>().ConsumeStamina(30))
            {
                return;
            }
            Finish();
            this.routine = StartCoroutine(DashCoroutine());
        }

        IEnumerator DashCoroutine()
        {
            var movement = this.GetComponent<PlayerMovement>();
            var direction = movement.Velocity.XZPlane().normalized;
            movement.SpeedMultiplier = 0f;
            var startTime = Time.time;
            var endTime = startTime + this.DashDuration;
            var pos = this.transform.position;
            var target = pos + direction * DashDistance;

            var rigidbody = this.GetComponent<Rigidbody>();
            rigidbody.isKinematic = true;
            while (Time.time < endTime)
            {
                var t = (Time.time - startTime) / DashDuration;
                t = Mathf.Pow(t, 0.5f);
                this.transform.position = Vector3.Lerp(pos, target, t);
                yield return null;
            }
            this.transform.position = target;
            rigidbody.isKinematic = false;
            this.Finish();
        }
    }
}
