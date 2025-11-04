using _Scripts.InputHandler.InputReader;


namespace _Scripts.Player.FSM.States.OnFootStates
{
    public class P_OnFootState : P_BaseState
    {
        private readonly PlayerInputReader _inputReader;
        private P_BaseState _currentSubState;
        
        public P_IdleState IdleState { get; }
        public P_WalkState WalkState { get; }
        public P_RunState RunState { get; }
        public P_CrouchState CrouchState { get; }
        public P_JumpState JumpState { get; }
        public P_OnFootState(P_StateMachine stateMachine, PlayerInputReader inputReader) : base(stateMachine)
        {
            _inputReader = inputReader;
            IdleState = new P_IdleState(stateMachine, this);
            WalkState = new P_WalkState(stateMachine, this);
            RunState = new P_RunState(stateMachine, this);
            CrouchState = new P_CrouchState(stateMachine, this);
            JumpState = new P_JumpState(stateMachine, this);
        }

        public override void OnEnter()
        {
            base.OnEnter();
            StateMachine.InputManager.PlayerInput = _inputReader;
            InitSubState(IdleState);
        }

        public void ChangeSubState(P_BaseState newSubState)
        {
            _currentSubState?.OnExit();
            _currentSubState = newSubState;
            _currentSubState?.OnEnter();
        }

        public override void OnExecute()
        {
            base.OnExecute();
            _currentSubState?.OnExecute();
        }

        public override void OnExit()
        {
            base.OnExit();
            _currentSubState?.OnExit();
        }

        public void InitSubState(P_BaseState newSubState)
        {
            _currentSubState = newSubState;
            _currentSubState?.OnEnter();
        }
    }
}