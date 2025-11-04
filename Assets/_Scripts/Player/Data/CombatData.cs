using UnityEngine;

namespace _Scripts.Player.Data
{
    [CreateAssetMenu(fileName = "CombatData", menuName = "Game Data/Data Modules/ Combat Data", order = 0)]
    public class CombatData : ScriptableObject
    {
        public float attackDamage = 15f;
        public float attackRate = 1f;
    }
}