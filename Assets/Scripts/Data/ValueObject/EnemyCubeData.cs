using UnityEngine;

namespace Data.ValueObject
{
    [CreateAssetMenu(fileName = "CubeCrusher3D", menuName = "CubeCrusher3D/EnemyCubeData", order = 0)]
    public class EnemyCubeData : ScriptableObject
    {
        public int EnemyHealthValue;
    }
}