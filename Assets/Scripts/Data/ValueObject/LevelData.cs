using System;
using Managers;
using UnityEngine;

namespace Data.ValueObject
{
    [Serializable]
    public class LevelData
    {
      //  public GameObject LevelGameObject;

      public int LevelIDCount;

      public void InıtializeLevelID()
      {
         LevelIDCount = SaveLoadManager.LoadValue("LevelIDCount", 1);
      }
    }
}