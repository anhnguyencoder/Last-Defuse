using _Scripts.InputHandler.InputReader;
using UnityEngine;

namespace _Scripts.Player.FSM.States.OnFootStates
{
    public class P_JumpState : P_BaseState
    {
        private readonly P_OnFootState _onFootState;
        public P_JumpState(P_StateMachine stateMachine, P_OnFootState onFootState) : base(stateMachine)
        {
            _onFootState = onFootState;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            StateMachine.PlayerEvents.TriggerJump();

            float jumpVelocity = Mathf.Sqrt(-2.0f * StateMachine.Gravity * StateMachine.Profile.movementData.jumpForce);
            StateMachine.VerticalVelocity = jumpVelocity;
        }

        public override void OnExecute()
        {
            base.OnExecute();
            Vector3 moveDirection = StateMachine.GetMovementDirection();

            moveDirection.y = StateMachine.VerticalVelocity;

            float airControlSpeed = StateMachine.Profile.movementData.walkSpeed * 0.8f;
            StateMachine.Controller.Move(moveDirection * airControlSpeed * Time.deltaTime);
        
            // Điều kiện thoát state: khi chạm đất
            if (StateMachine.Controller.isGrounded && StateMachine.VerticalVelocity < 0)
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
}