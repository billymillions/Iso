using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TimelineIso
{
    public class PlayerInputHandler : MonoBehaviour
    {

        [SerializeField]
        public CharacterSelector CharSelector;
        Timeline timeline;
        InputBuffer inputBuffer;
        Vector3 cameraForward, cameraRight;
        Vector3 heading;

        // Start is called before the first frame update
        void Start()
        {
            cameraForward = Camera.main.transform.forward;
            cameraForward.y = 0;
            cameraForward = cameraForward.normalized;
            cameraRight = Camera.main.transform.right;
            cameraRight.y = 0;
            cameraRight = cameraRight.normalized;

            inputBuffer = GameObject.Find("GlobalInputCache").GetComponent<GlobalInputCache>().InputBuffer;
            timeline = GameObject.Find("Timeline").GetComponent<TimelineMono>().Timeline;
        }

        public void OnMove(InputValue v)
        {
            var value = v.Get<Vector2>();
            var headingRight = value.x * cameraRight;
            var headingForward = value.y * cameraForward;
            heading = (headingForward + headingRight).normalized * value.magnitude;
            SetMove(heading);
        }

        public Vector3 GetMove(Vector2 vec)
        {
            // TODO
            var headingRight = vec.x * cameraRight;
            var headingForward = vec.y * cameraForward;
            heading = (headingForward + headingRight).normalized * vec.magnitude;
            return heading;
        }

        public void OnLook(InputValue v)
        {
            var value = v.Get<Vector2>();
            var headingRight = value.x * cameraRight;
            var headingForward = value.y * cameraForward;
            heading = (headingForward + headingRight).normalized * value.magnitude;
            var look = new LookInput { look = heading };
            inputBuffer.AddInput(CharSelector.SelectedCharacter, look);
        }

        public void OnCycle(InputValue v)
        {
            //SetMove(new Vector3(0f, 0f, 0f));
            CharSelector.CycleCharacter();
            //SetMove(heading);
        }

        internal void SetMove(Vector3 heading)
        {
            var move = new MoveInput { move = heading };
            inputBuffer.AddInput(CharSelector.SelectedCharacter, move);

            //var gameObjEntity = this.CharSelector.SelectedCharacter.GetComponent<GameObjectEntity>();
            //var entityIdentifier = gameObjEntity.EntityManager.GetComponentData<EntityIdentifier>(gameObjEntity.Entity);
            //var move = new MoveAction { move = heading };
            //var assignedInput = new AssignedInput { identifier = entityIdentifier, input = move };
            //this.inputBuffer.AddInput(entityIdentifier, move);
        }

        public void OnCycleLeft(InputValue value)
        {
            CharSelector.CycleCharacter();
        }

        public void OnRewind(InputValue value)
        {
            if (value.isPressed)
            {
                timeline.IsReverse = true;
            }
            else
            {
                timeline.IsReverse = false;
            }
        }

        public void OnAttack(InputValue value)
        {
            var attack = new AttackInput { };
            inputBuffer.AddInput(CharSelector.SelectedCharacter, attack);
            var button = new ButtonInput
            {
                is_press = value.isPressed,
                is_release = !value.isPressed,
                button_name = "Attack",
                value = value.Get<float>()
            };
            inputBuffer.AddInput(CharSelector.SelectedCharacter, button);
        }

        // Update is called once per frame
        public void OnShoot(InputValue value)
        {
            var shoot = new ShootInput { };
            inputBuffer.AddInput(CharSelector.SelectedCharacter, shoot);
        }
        public void OnCharge(InputValue value)
        {
            var shoot = new ChargeInput { isRelease = !value.isPressed };
            inputBuffer.AddInput(CharSelector.SelectedCharacter, shoot);
            var button = new ButtonInput {
                is_press = value.isPressed,
                is_release = !value.isPressed,
                button_name = "Charge",
                value=value.Get<float>()
            };
            inputBuffer.AddInput(CharSelector.SelectedCharacter, button);
        }

        public void OnDash(InputValue value)
        {
            var dash = new DashInput { };
            var button = new ButtonInput
            {
                is_press = value.isPressed,
                is_release = !value.isPressed,
                button_name = "Dash",
                value = value.Get<float>()
            };
            inputBuffer.AddInput(CharSelector.SelectedCharacter, dash);
            inputBuffer.AddInput(CharSelector.SelectedCharacter, button);
        }

        public void OnSnap(InputValue value)
        {
            this.timeline.CurrentIndex = 0;
            this.timeline.IsSnap = true;
        }

    }
}