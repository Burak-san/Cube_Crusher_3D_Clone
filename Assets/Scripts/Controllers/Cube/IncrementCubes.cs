using Managers;
using TMPro;
using UnityEngine;

namespace Controllers.Cube
{
    public class IncrementCubes : Cube
    {
        [SerializeField] private TextMeshPro valueText;
        public int CubeValue { get; private set; }
        public TetrisBlockManager parentTetrisBlock;
        public bool IsPlaceable;

        private void Awake()
        {
            parentTetrisBlock = GetComponentInParent<TetrisBlockManager>();
        }

        private void Start()
        {
            CubeValue = Random.Range(1, 5);
            valueText.text = CubeValue.ToString();
        }
    }
}