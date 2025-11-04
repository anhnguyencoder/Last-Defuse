using System;
using UnityEngine;

namespace _Scripts.InputHandler.InputReader
{
    public class BaseInputReader : ScriptableObject
    {
        protected InputSystemActions InputControls;

        protected virtual void OnEnable()
        {
            if (InputControls == null)
            {
                InputControls = new InputSystemActions();
            }
        }

        public virtual void EnableActions()
        {
        }

        public virtual void DisableActions()
        {
            
        }
    }
}