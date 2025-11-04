using UnityEngine;

namespace _Scripts.Player.Data
{
    [CreateAssetMenu(fileName = "HeathData", menuName = "Game Data/Data Modules/ Health Data", order = 0)]
    public class HealthData : ScriptableObject
    {
        public float health;
    }
}