using System;
using UnityEngine;

namespace _Scripts.DesignPattern.FiniteStateManchine
{
    public class BaseFSM : MonoBehaviour
    {
        public IBaseState CurrentState { get; set; }
        public virtual void OnExecute()
        {
            CurrentState?.OnExecute();
        }

        public void ChangeState(IBaseState newState)
        {
            CurrentState?.OnExit();
            CurrentState = newState;
            CurrentState?.OnEnter();
        }

        public void InitializeState(IBaseState initialState)
        {
            CurrentState = initialState;
            CurrentState?.OnEnter();
        }
    }
}