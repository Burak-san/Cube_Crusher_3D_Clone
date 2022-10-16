using System.Collections.Generic;
using Enums;
using Signals;
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
            //Debug.Log(FindObjectOfType<LevelSignals>().gameObject.name);
        }

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onChangeGameState += OnChangeGameState;
        }
        
        private void UnsubscribeEvents()
        {
            CoreGameSignals.Instance.onChangeGameState -= OnChangeGameState;
        }
        
        private void OnDisable()
        {
            UnsubscribeEvents();
        }
        
        private void OnChangeGameState(GameStates currentState)
        {
            if (currentState == GameStates.EnemyMovePhase)
                RandomSpawnBlock();
        }

        private void RandomSpawnBlock()
        {
            spawningObject = Instantiate(tetrisBlockList[Random.Range(0, tetrisBlockList.Count)]);
            spawningObject.transform.position = transform.position;
        }
    }
}
