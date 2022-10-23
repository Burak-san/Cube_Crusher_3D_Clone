using System;

namespace Data.ValueObject
{
    [Serializable]
    public class MoneyData
    {
        public int TotalMoney = 500;
        
        public int GainMoney = 10;
        public int BaseCubeValue = 1;
        
        public int PowerMoneyDecrease = 100;
        public int PowerLevel = 1;
        
        public int GainCoinLevel = 1;
        public int GainCoinDecrease = 100;
    }
}