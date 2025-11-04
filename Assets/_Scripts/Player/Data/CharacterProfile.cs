using UnityEngine;

namespace _Scripts.Player.Data
{
    [CreateAssetMenu(fileName = "CharaterProfile", menuName = "Game Data/Character Profile", order = 0)]
    public class CharacterProfile : ScriptableObject
    {
        [Header("Data Modules")]
        public MovementData movementData;
        public CombatData combatData;
        public HealthData healthData;
    }
}