using Data.UnityObject;
using Data.ValueObject;
using Managers;
using Signals;
using TMPro;
using UnityEngine;

namespace Controllers.Cube
{
    public class BaseCube : Cube
    {
        #region Self Variables

        #region Public Variables

        public Vector2Int BaseCubeTilePosition;
        public int CubeValue { get; private set; }
        
        #endregion

        #region Serialized Variables

        [SerializeField] private TextMeshPro valueText;

        #endregion

        #region Private Variables

        private MoneyData _moneyData;

        #endregion

        #endregion

        
        private void Awake()
        {
            GetData();
        }

        private void GetData()
        {
            _moneyData = GetMoneyData();
            _moneyData.InitializeMoneyData();
            _moneyData.BaseCubeValue = SaveLoadManager.LoadValue("BaseCubeValue", _moneyData.BaseCubeValue);
            CubeValue = _moneyData.BaseCubeValue;
            valueText.text = CubeValue.ToString();
        }

        #region Event Subscription

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            BaseCubeSignals.Instance.onBaseCubePowerIncrease += OnBaseCubePowerIncrease;
        }
        
        private void UnSubscribeEvents()
        {
            BaseCubeSignals.Instance.onBaseCubePowerIncrease -= OnBaseCubePowerIncrease;
        }
        
        private void OnDisable()
        {
            UnSubscribeEvents();
        }

        #endregion
        
        private MoneyData GetMoneyData() => Resources.Load<CD_Money>("Data/CD_Money").MoneyData;

        private void OnBaseCubePowerIncrease()
        {
            IncreaseCubeValue(1);
        }
        
        public void IncreaseCubeValue(int amount)
        {
            CubeValue += amount;
            valueText.text = CubeValue.ToString();
        }
    }
}

