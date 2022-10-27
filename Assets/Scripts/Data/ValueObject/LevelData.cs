using System;
using Managers;

namespace Data.ValueObject
{
    [Serializable]
    public class LevelData
    {
      public int LevelIDCount;

      public void InıtializeLevelID()
      {
         LevelIDCount = SaveLoadManager.LoadValue("LevelIDCount", 1);
      }
    }
}