using System;
using System.Collections.Generic;
using Controllers;
using Controllers.Army;
using Enums;
using Signals;
using UnityEngine;
using Random = UnityEngine.Random;

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
                for (int i = 0; i < _gridManager.BaseCubeList.Count; i++)
                {
                    int spawnValue = _gridManager.BaseCubeList[i].CubeValue;
                    
                    for (int j = 0; j < spawnValue; j++)
                    {
                        
                        GameObject army = _objectPooler.SpawnFromPool(
                            "Army",
                            _gridManager.BaseCubeList[i].transform.position,
                            Quaternion.identity,
                            armyHolder.transform);
                        army.transform.position += new Vector3(Random.Range(0.1f,0.25f),-1,Random.Range(0.1f,1));
                        
                        army.GetComponent<ArmyMovementController>().Move();
                        ArmyList.Add(army);
                    }
                }
            }
        }
        public void ArmyCheck()
        {
            if (armyHolder.childCount == 0)
            {
                CoreGameSignals.Instance.onChangeGameState?.Invoke(GameStates.EnemyMovePhase);
            }
        }
        public void ReturnToPoolArmy(GameObject army)
        {
            _objectPooler.ReturnToPool("Army",army);
        }
    }
}