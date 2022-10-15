using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Controllers
{
    public class IncrementCubes : MonoBehaviour
    {
        [SerializeField] private TextMeshPro valueText;
        public int CubeValue { get; private set; }
        public TetrisBlockController parentTetrisBlock;
        public bool IsPlaceable;

        private void Awake()
        {
            parentTetrisBlock = GetComponentInParent<TetrisBlockController>();
        }

        private void Start()
        {
            CubeValue = Random.Range(1, 5);
            valueText.text = CubeValue.ToString();
        }
    }
}