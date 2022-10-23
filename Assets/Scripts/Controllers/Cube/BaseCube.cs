using System;
using Data.UnityObject;
using Data.ValueObject;
using Signals;
using TMPro;
using UnityEngine;

namespace Controllers.Cube
{
    public class BaseCube : Cube
    {
        [SerializeField] private TextMeshPro valueText;
        public Vector2Int BaseCubeTilePosition;
        public int CubeValue { get; private set; }
        private MoneyData _moneyData;
        private void Awake()
        {
            _moneyData = GetMoneyData();
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

