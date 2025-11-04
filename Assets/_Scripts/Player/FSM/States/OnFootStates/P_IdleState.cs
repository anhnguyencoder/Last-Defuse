using _Scripts.InputHandler.InputReader;
using UnityEngine;

namespace _Scripts.Player.FSM.States.OnFootStates
{
    public class P_IdleState : P_BaseState
    {
        private readonly P_OnFootState _onFootState;
        public P_IdleState(P_StateMachine stateMachine, P_OnFootState onFootState) : base(stateMachine)
        {
            _onFootState = onFootState;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            StateMachine.InputManager.PlayerInput.JumpEvent += OnJump;
            StateMachine.InputManager.PlayerInput.CrouchEvent += OnCrouch;
            
            StateMachine.PlayerEvents.TriggerIdle();
        }

        public override void OnExecute()
        {
            base.OnExecute();
            Vector3 movement = new Vector3(0, StateMachine.VerticalVelocity, 0);
            StateMachine.Controller.Move(movement * Time.deltaTime);
            if (StateMachine.MoveInput != Vector2.zero)
            {
                _onFootState.ChangeSubState(_onFootState.WalkState);
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            StateMachine.InputManager.PlayerInput.JumpEvent -= OnJump;
            StateMachine.InputManager.PlayerInput.CrouchEvent -= OnCrouch;
        }

        private void OnJump() => _onFootState.ChangeSubState(_onFootState.JumpState);
        private void OnCrouch() => _onFootState.ChangeSubState(_onFootState.CrouchState);
    }
}