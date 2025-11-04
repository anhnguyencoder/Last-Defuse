using _Scripts.DesignPattern.FiniteStateManchine;
using UnityEngine;

namespace _Scripts.Player.FSM
{
    public abstract class P_BaseState : IBaseState
    {
        protected readonly P_StateMachine StateMachine;

        protected P_BaseState(P_StateMachine stateMachine)
        {
            StateMachine = stateMachine;
        }

        public virtual void OnEnter()
        {
            
        }

        public virtual void OnExecute()
        {
        }

        public virtual void OnExit()
        {
        }
    }
}