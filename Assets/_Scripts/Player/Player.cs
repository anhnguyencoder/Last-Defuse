using System;
using _Scripts.InputHandler.InputReader;
using _Scripts.Player.Data;
using _Scripts.Player.FSM;
using UnityEngine;

namespace _Scripts.Player
{
    public class Player : MonoBehaviour
    {
        public P_StateMachine StateMachine {get; private set;}
        
        [SerializeField]
        private CharacterProfile characterProfile;
        
        public CharacterProfile CharacterProfile  => characterProfile;

        private void Awake()
        {
            StateMachine = GetComponent<P_StateMachine>();
        }

        private void Update()
        {
            StateMachine.OnExecute();
        }
    }
}