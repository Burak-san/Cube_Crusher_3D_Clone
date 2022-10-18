﻿using System;
using System.Collections.Generic;
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
        [SerializeField] private List<GameObject> EnemyCubeList = new List<GameObject>();

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
                EnemyMove();
                CoreGameSignals.Instance.onChangeGameState?.Invoke(GameStates.EnemySpawnPhase);
            }
             
            if (currentState == GameStates.EnemySpawnPhase)
            {
                //dusman SPAWN mekanikleri cagirilacak POOL KONACAK
                Debug.Log("Enemy Spawn Phase");
                CoreGameSignals.Instance.onChangeGameState?.Invoke(GameStates.Playing);
            }
        }
        
        public void EnemyMove()
        {
            for (int i = 0; i < EnemyCubeList.Count; i++)
            {
                EnemyCubeList[i].transform.DOMove(EnemyCubeList[i].transform.position + Vector3.back, 1, false);
            }
        }
        
    }
}