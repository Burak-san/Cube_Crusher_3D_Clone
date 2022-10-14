using System.Collections.Generic;
using Signals;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Controllers.Cube
{
    public class TetrisBlockSpawner : MonoBehaviour
    {
        [SerializeField] private List<TetrisBlockController> tetrisBlockList;
        
        private TetrisBlockController spawningObject;
        void Start()
        {
            RandomSpawnBlock();
            Debug.Log(FindObjectOfType<LevelSignals>().gameObject.name);
        }

        [Button]
        private void RandomSpawnBlock()
        {
            spawningObject = Instantiate(tetrisBlockList[Random.Range(0, tetrisBlockList.Count)]);
            spawningObject.transform.position = transform.position;
        }
    }
}
