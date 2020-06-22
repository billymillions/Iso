using UnityEngine;
using System.Collections;
using System;
using System.Xml.Serialization;
using System.Runtime.CompilerServices;

namespace TimelineIso
{
    public class PlayerSlashComponent : PlayerAbilityComponent
    {

        public float SlashSpeed = 1f;
        public float RunSpeed = 1f;
        public float ComboWindow = .1f;

        private string[] ComboClips = new string[] { "Slash1", "Slash2", "Slash3" };
        public GameObject [] SwordComboPrefabs;
        public GameObject SwordComboInstance;

        // semi-state
        private int comboCounter = 0;
        private GameObject go;
        private bool allowInterrupt = true;
        private Coroutine comboRoutine;
        private PlayerAbilityStatus _status;
        private bool slashTrigger;

        private T GetLazy<T>(T saved) {
            if (saved == null)
            {
                saved = this.GetComponent<T>();
            }
            return saved;
        }


        public override void Initialize()
        {
        }

        public override PlayerAbilityStatus Status()
        {
            return (this.allowInterrupt) ? PlayerAbilityStatus.Finished : PlayerAbilityStatus.Running;
        }

        public override InputHandledStatus HandleInput(IInputEvent input)
        {
            var animator = GetComponent<Animator>();
            if (this.allowInterrupt && input is CommandInput ci && ci.targetAbility == this)
            {
                this.ExecuteSlash();
                return InputHandledStatus.Handled;
            }
            else if (this.allowInterrupt)
            {
                animator.Play("SlashEnd", 0, 0);
                return InputHandledStatus.Permit;
                //TODO: specify the input is unhandled?
            }
            return InputHandledStatus.Deny;
            //if (input is AttackInput)
            //{
            //    this.slashTrigger = true;
            //}
        }

        public override void Finish()
        {
            if (this.SwordComboInstance != null)
            {
                Destroy(this.SwordComboInstance); 
            }
            this.GetComponent<PlayerMovement>().SpeedMultiplier = 1f;
            this.allowInterrupt = true;
            this._status = PlayerAbilityStatus.Finished;
        } 

        public void SlashNudge()
        {
            this.GetComponent<PlayerMovement>().Nudge(.5f);
        }

        public void BeginComboWindow()
        {
            if (this.comboRoutine != null)
            {
                this.StopCoroutine(this.comboRoutine);
            }
            this.comboRoutine = this.StartCoroutine(this.OpenComboWindow());
        }

        public void ExecuteSlash()
        {

            if (!this.GetComponent<CharacterStamina>().ConsumeStamina(30))
            {
                return;
            }

            this.GetComponent<PlayerMovement>().SpeedMultiplier = 0f;
            var animator = this.GetComponent<Animator>();
            this.allowInterrupt = false;
            if (this.comboRoutine != null)
            {
                StopCoroutine(this.comboRoutine);
                this.comboRoutine = null;
            }
            animator.Play(this.ComboClips[this.comboCounter], 0, 0);
        }

        public void TriggerCollider()
        {
            if (this.SwordComboInstance != null)
            {
                Destroy(this.SwordComboInstance);
            }
            this.SwordComboInstance = Instantiate(this.SwordComboPrefabs[this.comboCounter], this.gameObject.transform);
        }

        IEnumerator OpenComboWindow()
        {
            this.allowInterrupt = true;
            this.comboCounter = (this.comboCounter + 1) % this.ComboClips.Length;
            // TODO: control animation speed and this window with the same scale?
            yield return new WaitForSeconds(this.ComboWindow);
            this.comboCounter = 0;
        }

        // TODO: Rush??
        //void Rush()
        //{
        //    var enemyVector = (this.locked.transform.position - this.transform.position).XZPlane();
        //    var rigidbody = this.GetComponent<Rigidbody>(); 
        //    if (enemyVector.magnitude <= 5)
        //    {
        //        rigidbody.velocity = this.velocity;
        //        this.charging = false;
        //    } else
        //    {
        //        rigidbody.velocity = enemyVector.normalized * this.chargeSpeed;
        //    }
        //    this.transform.Translate(enemyVector.normalized * Time.deltaTime * this.chargeSpeed, Space.World);
        //}
    }
}

