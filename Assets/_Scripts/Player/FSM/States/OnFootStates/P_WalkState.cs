using UnityEngine;

namespace _Scripts.Player.FSM.States.OnFootStates
{
    public class P_WalkState : P_BaseState
    {
        private readonly P_OnFootState _onFootState;
        public P_WalkState(P_StateMachine stateMachine, P_OnFootState onFootState) : base(stateMachine)
        {
            _onFootState = onFootState;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            var input = StateMachine.InputManager.PlayerInput;
            input.JumpEvent += OnJump;
            input.CrouchEvent += OnCrouch;
            input.SprintEvent += OnSprint;
            
            StateMachine.PlayerEvents.TriggerWalk();
        }

        public override void OnExecute()
        {
            base.OnExecute();

            Vector3 moveDirection = StateMachine.GetMovementDirection();

            moveDirection.y = StateMachine.VerticalVelocity;

            float speed = StateMachine.Profile.movementData.walkSpeed;
            StateMachine.Controller.Move(moveDirection * speed * Time.deltaTime);

            if (StateMachine.MoveInput == Vector2.zero)
                _onFootState.ChangeSubState(_onFootState.IdleState);
            
        }

        public override void OnExit()
        {
            base.OnExit();
            var input = StateMachine.InputManager.PlayerInput;
            input.JumpEvent -= OnJump;
            input.CrouchEvent -= OnCrouch;
            input.SprintEvent -= OnSprint;
        }

        private void OnJump() => _onFootState.ChangeSubState(_onFootState.JumpState);
        private void OnCrouch() => _onFootState.ChangeSubState(_onFootState.CrouchState);

        private void OnSprint(bool isSprinting)
        {
            if(isSprinting)
                _onFootState.ChangeSubState(_onFootState.RunState);
        }
        
    }
}