using TMPro;
using UnityEngine;

namespace Controllers.Cube
{
    public class BaseCube : Cube
    {
        [SerializeField] private TextMeshPro valueText;
        public Vector2Int BaseCubeTilePosition;
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

