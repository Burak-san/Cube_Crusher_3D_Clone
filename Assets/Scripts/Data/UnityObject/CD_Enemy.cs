using Data.ValueObject;
using UnityEngine;

namespace Data.UnityObject
{
    [CreateAssetMenu(fileName = "CD_Enemy", menuName = "CubeCrusher/CD_Enemy", order = 0)]
    public class CD_Enemy : ScriptableObject
    {
        public EnemyData EnemyData;
    }
}