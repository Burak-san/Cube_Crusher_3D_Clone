using System;
using System.Collections.Generic;
using Controllers;
using Enums;
using Signals;
using UnityEngine;

namespace Managers
{
    public class ArmyManager : MonoBehaviour
    {
        [SerializeField] private Transform armyHolder;
        
        private ObjectPooler _objectPooler;
        private List<GameObject> ArmyList = new List<GameObject>();
        private ArmyMovementController _armyMovementController;
        private GridManager _gridManager;

        private void Awake()
        {
            _gridManager = FindObjectOfType<GridManager>();
            _objectPooler = FindObjectOfType<ObjectPooler>();
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
            if (currentState == GameStates.AttackPhase)
            {
                Debug.Log("Attack Phase");
                for (int i = 0; i < _gridManager.BaseCubeList.Count; i++)
                {
                    int spawnValue = _gridManager.BaseCubeList[i].CubeValue;
                    
                    for (int j = 0; j < spawnValue; j++)
                    {
                        GameObject army = _objectPooler.SpawnFromPool(
                            "Human",
                            _gridManager.BaseCubeList[i].transform.position,
                            Quaternion.identity,
                            armyHolder.transform);
                        army.transform.position += new Vector3(0,-0.75f,0);
                        army.GetComponent<ArmyMovementController>().Move();
                        ArmyList.Add(army);
                        
                    }
                }
                
                CoreGameSignals.Instance.onChangeGameState?.Invoke(GameStates.EnemyMovePhase);
            }
        }
    }
}