using System;
using Managers;

namespace Data.ValueObject
{
    [Serializable]
    public class MoneyData
    {
        public int TotalMoney ;
        
        public int GainMoney ;
        public int BaseCubeValue;
        
        public int PowerMoneyDecrease;
        public int PowerLevel ;
        
        public int GainCoinLevel ;
        public int GainCoinDecrease ;
        
        public void InitializeMoneyData()
        {
            TotalMoney = SaveLoadManager.LoadValue("TotalMoney", 500);
            GainMoney = SaveLoadManager.LoadValue("GainMoney", 10);
            BaseCubeValue = SaveLoadManager.LoadValue("BaseCubeValue", 1);
            PowerMoneyDecrease = SaveLoadManager.LoadValue("PowerMoneyDecrease", 100);
            PowerLevel = SaveLoadManager.LoadValue("PowerLevel", 1);
            GainCoinLevel = SaveLoadManager.LoadValue("GainCoinLevel", 1);
            GainCoinDecrease = SaveLoadManager.LoadValue("GainCoinDecrease", 100);
        }
    }
}