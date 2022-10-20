using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Controllers;
using Controllers.EnemyCube;
using DG.Tweening;
using Enums;
using Signals;
using UnityEngine;


namespace Managers
{
    public class EnemyCubeManager : MonoBehaviour
    {
        [SerializeField] private List<GameObject> enemyCubeList = new List<GameObject>();
        [SerializeField] private List<Transform> enemyCubeSpawnTransformList = new List<Transform>();
        [SerializeField] private Transform enemyCubeHolder;
        
        private ObjectPooler _objectPooler;
        
        private void Awake()
        {
            _objectPooler = FindObjectOfType<ObjectPooler>();
        }

        private void Start()
        {
            EnemyCubeGetFromPool();
        }

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onChangeGameState += OnChangeGameState;
        }
        private void UnSubscribeEvents()
        {
            CoreGameSignals.Instance.onChangeGameState -= OnChangeGameState;
        }
        
        private void OnDisable()
        {
            UnSubscribeEvents();
        }

        private void OnChangeGameState(GameStates currentState)
        {
            if (currentState == GameStates.EnemyMovePhase)
            {
                EnemyCubeMove();
                EnemySpawnPhaseSignal();
                
            }
             
            if (currentState == GameStates.EnemySpawnPhase)
            {
                EnemyCubeGetFromPool();
                CoreGameSignals.Instance.onChangeGameState?.Invoke(GameStates.Playing);
            }
        }

        private async void EnemySpawnPhaseSignal()
        {
            await Task.Delay(1000);
            CoreGameSignals.Instance.onChangeGameState?.Invoke(GameStates.EnemySpawnPhase);
        }

        private void EnemyCubeMove()
        {
            for (int i = 0; i < enemyCubeList.Count; i++)
            {
                enemyCubeList[i].GetComponent<EnemyCubeMovementController>().Move();
            }
        }
        
        

        private void EnemyCubeGetFromPool()
        {
            for (int i = 0; i < enemyCubeSpawnTransformList.Count; i++)
            {
                GameObject EnemyCube = _objectPooler.SpawnFromPool(
                    "EnemyCube",
                    enemyCubeSpawnTransformList[i].transform.position,
                    Quaternion.identity,
                    enemyCubeHolder.transform);
                        
                enemyCubeList.Add(EnemyCube);
            }
        }
    }
}