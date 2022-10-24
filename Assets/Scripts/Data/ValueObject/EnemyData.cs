using System;

namespace Data.ValueObject
{
    [Serializable]
    public class EnemyData
    {
        public int LeftCubeCount = 30;
        public int SpawnCubeCount;
        public int TempLeftCubeCount = 30;
    }
}