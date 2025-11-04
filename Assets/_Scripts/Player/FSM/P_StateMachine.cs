using System;
using _Scripts.DesignPattern.FiniteStateManchine;
using _Scripts.InputHandler.InputReader;
using _Scripts.Player.Data;
using _Scripts.Player.FSM.States.OnFootStates;
using UnityEngine;

namespace _Scripts.Player.FSM
{
    [RequireComponent(typeof(PlayerEvents), typeof(PlayerInputManager))]
    public class P_StateMachine : BaseFSM
    {
        // [Header("Dependencies")] [SerializeField]
        // private PlayerInputReader onFootInputReader;
        // [SerializeField] private VehicleInputReader vehicleInputReader;
        
        public CharacterProfile Profile { get; private set; }
        public CharacterController Controller { get; private set; }
        public PlayerInputManager InputManager { get; private set; }
        public PlayerEvents PlayerEvents { get; private set; }

        // public IState CurrentState { get; private set; }

        public float VerticalVelocity { get; set; } 
        public float Gravity => Physics.gravity.y;

        public P_OnFootState OnFootState { get; private set; }
        // public P_VehicleState VehicleState { get; private set; }

        public Vector2 MoveInput => InputManager.PlayerInput?.MoveInput ?? Vector2.zero;

        private void Awake()
        {
            InputManager = GetComponent<PlayerInputManager>();
            PlayerEvents = GetComponent<PlayerEvents>();
            Controller = GetComponent<CharacterController>();
            Profile = GetComponent<Player>().CharacterProfile;
            OnFootState = new P_OnFootState(this, InputManager.PlayerInput);
        }

        private void Start()
        {
            InitializeState(OnFootState);
        }

        private void FixedUpdate()
        {
            HandleGravity();
        }

        private void LateUpdate()
        {
            
        }

        private void HandleGravity()
        {
            if (Controller.isGrounded && VerticalVelocity < 0f)
            {
                VerticalVelocity = -2f;
            }
            else
            {
                VerticalVelocity += Gravity * Time.deltaTime;
            }
        }
        
        public Vector3 GetMovementDirection()
        {
            return transform.right * MoveInput.x + transform.forward * MoveInput.y;
        }
    }
}