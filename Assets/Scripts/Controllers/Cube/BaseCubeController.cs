using TMPro;
using UnityEngine;

namespace Controllers.Cube
{
    public class BaseCubeController : MonoBehaviour
    {
        [SerializeField] private TextMeshPro valueText;
        public int CubeValue { get; private set; }

        private void Start()
        {
            CubeValue = Random.Range(1, 5);
            valueText.text = CubeValue.ToString();
        }

        public void IncreaseCubeValue(int amount)
        {
            CubeValue += amount;
            valueText.text = CubeValue.ToString();
        }
    }
}

