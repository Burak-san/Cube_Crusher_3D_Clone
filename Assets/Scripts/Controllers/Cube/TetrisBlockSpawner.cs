using System;
using System.Collections.Generic;
using Signals;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

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

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onTetrisBlockPlace += RandomSpawnBlock;
        }
        
        private void UnsubcribeEvents()
        {
            CoreGameSignals.Instance.onTetrisBlockPlace -= RandomSpawnBlock;
        }


        private void OnDisable()
        {
            UnsubcribeEvents();
        }

        

        [Button]
        private void RandomSpawnBlock()
        {
            spawningObject = Instantiate(tetrisBlockList[Random.Range(0, tetrisBlockList.Count)]);
            spawningObject.transform.position = transform.position;
        }
    }
}
