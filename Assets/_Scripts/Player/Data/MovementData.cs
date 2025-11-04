using UnityEngine;

namespace _Scripts.Player.Data
{
    [CreateAssetMenu(fileName = "MovementData", menuName = "Game Data/Data Modules/Movement Data", order = 0)]
    public class MovementData : ScriptableObject
    {
        public float walkSpeed = 5f;
        public float runSpeed = 8f;
        public float jumpForce = 10f;
        public float crouchSpeed = 3f;
        public float crouchHeight = 1.5f;
        public float crouchCenterY = 1.5f;
    }
}