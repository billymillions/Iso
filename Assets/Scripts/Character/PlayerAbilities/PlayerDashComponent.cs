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
            var movement = this.GetComponent<PlayerMovement>();
            movement.SpeedMultiplier = 1f;
        }

        public override InputHandledStatus HandleInput(IInputEvent input)
        {
            if (input is CommandInput i && i.targetAbility == this && i.button.is_press)
            {
                if (this.Status() != PlayerAbilityStatus.Running)
                {
                    DoDash();
                    return InputHandledStatus.Handled;
                }
                return InputHandledStatus.Deny;
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
            var rigidbody = this.GetComponent<Rigidbody>();
            var em = this.GetComponent<EntityMove>();

            var direction = movement.MovementForward;
            var pos = this.transform.position;
            var target = pos + direction * DashDistance;
            var frames = (int)(this.DashDuration / Time.fixedDeltaTime);

            movement.enabled = false;
            for (int i = 0; i <= frames; i++)
            {
                var t = ((float)i) / frames;
                //rigidbody.MovePosition(Vector3.Lerp(pos, target, t));
                em.MovePosition(Vector3.Lerp(pos, target, t));
                yield return new WaitForFixedUpdate();
            }
            movement.enabled = true;
            this.Finish();
        }
    }
}
