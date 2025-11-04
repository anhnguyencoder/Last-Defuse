using System;
using UnityEngine;

namespace _Scripts.Player
{
    public class PlayerEvents : MonoBehaviour
    {
        public event Action OnIdle;
        public event Action OnWalk;
        public event Action OnCrouchStarted;
        public event Action OnCrouchStopped;
        public event Action OnSprintStarted;
        public event Action OnSprintStopped;
        public event Action OnJump;

        public event Action OnAttack;
        public event Action<float> OnDamaged;
        public event Action OnDeath; 
        
        public void TriggerIdle() => OnIdle?.Invoke();
        public void TriggerWalk() => OnWalk?.Invoke();
        public void TriggerCrouchStarted() => OnCrouchStarted?.Invoke();
        public void TriggerCrouchStopped() => OnCrouchStopped?.Invoke();
        public void TriggerSprintStarted() => OnSprintStarted?.Invoke();
        public void TriggerSprintStopped() => OnSprintStopped?.Invoke();
        public void TriggerJump() => OnJump?.Invoke();
        
        public void TriggerAttack() => OnAttack?.Invoke();
        public void TriggerDamaged(float damageAmount) => OnDamaged?.Invoke(damageAmount);
        public void TriggerDied() => OnDeath?.Invoke();

        
    }
}