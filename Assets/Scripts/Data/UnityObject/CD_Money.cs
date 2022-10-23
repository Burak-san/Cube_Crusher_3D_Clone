using Data.ValueObject;
using UnityEngine;

namespace Data.UnityObject
{
    
    [CreateAssetMenu(fileName = "CD_Money", menuName = "CubeCrusher/CD_Money", order = 0)]
    public class CD_Money : ScriptableObject
    {
        public MoneyData MoneyData;
    }
}