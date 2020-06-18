using UnityEngine;
using System.Collections;

//namespace TimelineIso
//{
//    [CreateAssetMenu(menuName = "PlayerAbility/PlayerSwordSlash")]
//    public class PlayerSwordSlashTemplate : PlayerAbilityTemplate
//    {
//        public float SlashSpeed = 1f;
//        public float RunSpeed = 1f;
//        public Material MyMaterial;
//        public GameObject SwordComboPrefab;

//        public override PlayerAbility Initialize(GameObject go)
//        {
//            return new PlayerSwordSlash(this, go);
//        }

//        public class PlayerSwordSlash : PlayerAbility
//        {
//            // TODO: semi-stateless
//            private Animator animator;
//            private PlayerController pc;
//            private PlayerSwordSlashTemplate pt;

//            public PlayerSwordSlash(PlayerSwordSlashTemplate pt, GameObject go)
//            {
//                this.animator = go.GetComponent<Animator>();
//                this.pc = go.GetComponent<PlayerController>();
//                this.pt = pt;
//            }

//            public void Trigger(IInputEvent input)
//            {
//                this.animator.SetTrigger("Slash");
//                //this.animator.Play("SwordSlash", 0);
//            }
//            public void Update()
//            {
//                //this.animator.SetTrigger("Slash");

//            }

//            public void Cancel() { }

//            public void Finish() { }
//        }

//    }

//}

