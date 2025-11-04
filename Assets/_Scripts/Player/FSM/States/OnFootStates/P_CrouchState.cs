using UnityEngine;

namespace _Scripts.Player.FSM.States.OnFootStates
{
    public class P_CrouchState : P_BaseState
    {
        private readonly P_OnFootState _onFootState;

        private float _originalControlHeight;
        public P_CrouchState(P_StateMachine stateMachine, P_OnFootState onFootState) : base(stateMachine)
        {
            _onFootState = onFootState;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            var input = StateMachine.InputManager.PlayerInput;
            input.CrouchEvent += OnCrouch;
            
            var controller = StateMachine.Controller;
            
            _originalControlHeight = controller.height;
            controller.height = StateMachine.Profile.movementData.crouchHeight;
        }

        public override void OnExecute()
        {
            base.OnExecute();
            Vector3 moveDirection = StateMachine.GetMovementDirection();

            moveDirection.y = StateMachine.VerticalVelocity;
            
            float moveSpeed = StateMachine.Profile.movementData.crouchSpeed;
            StateMachine.Controller.Move(moveDirection * moveSpeed * Time.deltaTime);
        }


        public override void OnExit()
        {
            base.OnExit();
            StateMachine.InputManager.PlayerInput.CrouchEvent -= OnCrouch;
            StateMachine.PlayerEvents.TriggerCrouchStopped();
            StateMachine.Controller.height = _originalControlHeight;
        }


        private void OnCrouch()
        {
            if (StateMachine.MoveInput == Vector2.zero)
            {
                _onFootState.ChangeSubState(_onFootState.IdleState);
            }
            else
            {
                _onFootState.ChangeSubState(_onFootState.WalkState);
            }
        }
        
    }
}