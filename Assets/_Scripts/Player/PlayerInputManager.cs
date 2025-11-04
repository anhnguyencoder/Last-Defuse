using _Scripts.InputHandler.InputReader;
using UnityEngine;

namespace _Scripts.Player
{
    public class PlayerInputManager : MonoBehaviour
    {
        [SerializeField] private PlayerInputReader playerInputReader;

        public PlayerInputReader PlayerInput
        {
            get => playerInputReader;
            set => playerInputReader = value;
        }

        private void Awake()
        {
            PlayerInput = playerInputReader;
        }
        
        private void OnEnable()
        {
            if (PlayerInput != null)
            {
                PlayerInput.EnableActions();
                Debug.Log("Player Input Actions ENABLED");
            }
        }

        // THÊM HÀM OnDisable NÀY VÀO
        private void OnDisable()
        {
            if (PlayerInput != null)
            {
                PlayerInput.DisableActions();
                Debug.Log("Player Input Actions DISABLED");
            }
        }
    }
}