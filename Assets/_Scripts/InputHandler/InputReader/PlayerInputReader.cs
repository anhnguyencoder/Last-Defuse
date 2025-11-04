using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts.InputHandler.InputReader
{
    [CreateAssetMenu(fileName = "PlayerInputReader", menuName = "InputReader/PlayerInputReader")]
    public class PlayerInputReader : BaseInputReader, InputSystemActions.IPlayerActions
    {
        public event Action<Vector2> MoveEvent;
        public event Action<Vector2> LookEvent;

        public event Action JumpEvent;
        public event Action InteractEvent;
        public event Action CrouchEvent;
        public event Action SwitchCameraEvent; 
        public event Action InventoryEvent;

        public event Action<float> SwitchWeaponEvent;
        
        public event Action<bool> SprintEvent;
        public event Action<bool> OnMouseEvent;
        public event Action<bool> AttackEvent; 
        public event Action<bool> SecondaryAttackEvent; 
        
        public Vector2 MoveInput {get; private set;}
        
        protected override void OnEnable()
        {
            base.OnEnable();
            InputControls?.Player.SetCallbacks(this);
        }

        public override void EnableActions()
        {
            base.EnableActions();
            InputControls?.Player.Enable();
        }

        public override void DisableActions()
        {
            base.DisableActions();
            InputControls?.Player.Disable();
        }


        public void OnMove(InputAction.CallbackContext context)
        {
            MoveInput = context.ReadValue<Vector2>();
            MoveEvent?.Invoke(MoveInput);
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            LookEvent?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    AttackEvent?.Invoke(true);
                    break;
                case InputActionPhase.Canceled:
                    AttackEvent?.Invoke(false);
                    break;
            }
        }

        public void OnSecondaryAttack(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    SecondaryAttackEvent?.Invoke(true);
                    break;
                case InputActionPhase.Canceled:
                    SecondaryAttackEvent?.Invoke(false);
                    break;
            }
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            InteractEvent?.Invoke();
        }

        public void OnCrouch(InputAction.CallbackContext context)
        {
            CrouchEvent?.Invoke();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                JumpEvent?.Invoke();
            }
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started: // Khi nút bắt đầu được nhấn
                    SprintEvent?.Invoke(true);
                    break;
                case InputActionPhase.Canceled: // Khi nút được nhả ra
                    SprintEvent?.Invoke(false);
                    break;
            }        
        }

        public void OnSwitchWeapon(InputAction.CallbackContext context)
        {
            SwitchWeaponEvent?.Invoke(context.ReadValue<float>());
        }

        public void OnInventory(InputAction.CallbackContext context)
        {
            InventoryEvent?.Invoke();
        }

        public void OnSwitchCamera(InputAction.CallbackContext context)
        {
            SwitchCameraEvent?.Invoke();
        }

        public void OnOnMouse(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    OnMouseEvent?.Invoke(true);
                    break;
                case InputActionPhase.Canceled:
                    OnMouseEvent?.Invoke(false);
                    break;
            }
        }
    }
}