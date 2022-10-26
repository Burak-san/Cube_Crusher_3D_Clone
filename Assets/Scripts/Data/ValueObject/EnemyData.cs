using System;
using Managers;

namespace Data.ValueObject
{
    [Serializable]
    public class EnemyData
    {
        public int LeftCubeCount ;
        public int SpawnCubeCount;
        public int TempLeftCubeCount ;
        public int LeftCubeIncrease;

        public void InitializeEnemyData()
        {
            LeftCubeCount = SaveLoadManager.LoadValue("LeftCubeCount", 30);
            SpawnCubeCount = SaveLoadManager.LoadValue("SpawnCubeCount", 0);
            TempLeftCubeCount = SaveLoadManager.LoadValue("LeftCubeIncrease", 5);
        }
    }
}