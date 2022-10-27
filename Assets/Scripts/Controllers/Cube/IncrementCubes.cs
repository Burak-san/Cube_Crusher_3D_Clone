using Managers;
using TMPro;
using UnityEngine;

namespace Controllers.Cube
{
    public class IncrementCubes : Cube
    {
        #region Self Variables

        #region Public Variables

        public int CubeValue { get; private set; }
        public TetrisBlockManager ParentTetrisBlock;
        public bool IsPlaceable;
        
        #endregion

        #region Private Variables

        [SerializeField] private TextMeshPro valueText;

        #endregion

        #endregion

        private void Awake()
        {
            GetData();
        }

        private void GetData()
        {
            ParentTetrisBlock = GetComponentInParent<TetrisBlockManager>();
        }

        private void Start()
        {
            StartActions();
        }

        private void StartActions()
        {
            CubeValue = Random.Range(1, 5);
            valueText.text = CubeValue.ToString();
        }
    }
}