using System.Collections.Generic;
using System.Threading.Tasks;
using Controllers.Cube;
using Data.UnityObject;
using Data.ValueObject;
using Enums;
using Signals;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Managers
{
    public class EnemyCubeManager : MonoBehaviour
    {
        [SerializeField] private List<EnemyCube> enemyCubeList = new List<EnemyCube>();
        [SerializeField] private Transform enemyCubeHolder;

        private EnemyData _data;
        private GridManager _gridManager;
        private ObjectPooler _objectPooler;
        private int _leftCubeIncrease;
        
        private void Awake()
        {
            GetDataResources();
        }

        private void GetDataResources()
        {
            _objectPooler = FindObjectOfType<ObjectPooler>();
            _gridManager = FindObjectOfType<GridManager>();
            
            _leftCubeIncrease = 5;
            
            _data = GetEnemyData();
            
            _data.SpawnCubeCount = _data.LeftCubeCount;
        }
        
        private void Start()
        {
            EnemyCubeGetFromPool();
            UISignals.Instance.onSetLeftText?.Invoke(_data.LeftCubeCount);
        }
        
        private EnemyData GetEnemyData() => Resources.Load<CD_Enemy>("Data/CD_Enemy").EnemyData;


        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onChangeGameState += OnChangeGameState;
            CoreGameSignals.Instance.onReset += OnReset;
            EnemyCubeSignals.Instance.onHitEnemyCube += OnHitEnemyCube;
        }
        private void UnSubscribeEvents()
        {
            CoreGameSignals.Instance.onChangeGameState -= OnChangeGameState;
            CoreGameSignals.Instance.onReset -= OnReset;
            EnemyCubeSignals.Instance.onHitEnemyCube -= OnHitEnemyCube;
        }
        
        private void OnDisable()
        {
            UnSubscribeEvents();
        }


        private void OnChangeGameState(GameStates currentState)
        {
            if (currentState == GameStates.EnemyMovePhase)
            {
                if (enemyCubeList.Count == 0)
                {
                    EnemySpawnPhase();
                    return;
                }
                EnemyCubeMove();
                EnemySpawnPhase();
            }
             
            if (currentState == GameStates.EnemySpawnPhase)
            {
                EnemyCubeGetFromPool();
                CoreGameSignals.Instance.onChangeGameState?.Invoke(GameStates.Playing);
            }
        }

        private async void EnemySpawnPhase()
        {
            await Task.Delay(1000);
            CoreGameSignals.Instance.onChangeGameState?.Invoke(GameStates.EnemySpawnPhase);
        }

        private void EnemyCubeMove()
        {
            for (int i = 0; i < enemyCubeList.Count; i++)
            {
                enemyCubeList[i].GetComponent<EnemyCubeMovementController>().Move();
                enemyCubeList.TrimExcess();
            }
        }

        private void OnHitEnemyCube(Transform enemyCube)
        {
            enemyCube.GetComponentInChildren<EnemyCubeMeshController>().ArmyHitEnemyCube();
            enemyCubeList.TrimExcess();
        }

        public void RemoveEnemyCubeList(EnemyCube enemyCube)
        {
            if (enemyCube.EnemyCubeTilePosition.y <=4)
            {
                _gridManager.Nodes[enemyCube.EnemyCubeTilePosition.x, enemyCube.EnemyCubeTilePosition.y].IsPlaceable = true;
                _gridManager.Nodes[enemyCube.EnemyCubeTilePosition.x, enemyCube.EnemyCubeTilePosition.y].IsEnemyTile = false;
            }
            _gridManager.Nodes[enemyCube.EnemyCubeTilePosition.x, enemyCube.EnemyCubeTilePosition.y].HeldCube = null;
            enemyCubeList.Remove(enemyCube);
            _data.LeftCubeCount--;
            UISignals.Instance.onSetLeftText?.Invoke(_data.LeftCubeCount);
            if (_data.LeftCubeCount <= 0 && _data.SpawnCubeCount <= 0)
            {
                UISignals.Instance.onOpenPanel?.Invoke(UIPanels.WinPanel);
                UISignals.Instance.onClosePanel?.Invoke(UIPanels.LevelPanel);
                CoreGameSignals.Instance.onChangeGameState?.Invoke(GameStates.GameStop);
            }
        }

        private void EnemyCubeGetFromPool()
        {
            for (int i = 0; i < _gridManager.Nodes.GetLength(0); i++)
            {
                if (_data.SpawnCubeCount <= 0) return;
                
                int spawnPointY = _gridManager.Nodes.GetLength(1) - 1;
                EnemyCube EnemyCube = _objectPooler.SpawnFromPool(
                    "EnemyCube",
                    Vector3.up,
                    Quaternion.identity,
                    _gridManager.Nodes[i, spawnPointY].transform).GetComponent<EnemyCube>();

                EnemyCube.EnemyCubeTilePosition = new Vector2Int(i, spawnPointY);
                _gridManager.Nodes[i, spawnPointY].HeldCube = EnemyCube;
                _gridManager.Nodes[i, spawnPointY].IsPlaceable = false;
                _gridManager.Nodes[i, spawnPointY].SnapPoint();
                enemyCubeList.Add(EnemyCube);
                _data.SpawnCubeCount--;
            }
        }
        
        public void ReturnToPoolArmy(GameObject enemyCube)
        {
            _objectPooler.ReturnToPool("EnemyCube",enemyCube);
        }

        private void GetLeftCubeCount()
        {
            _data.TempLeftCubeCount += _leftCubeIncrease;
            _data.LeftCubeCount = _data.TempLeftCubeCount;
            _data.SpawnCubeCount = _data.LeftCubeCount;
        }
        
        private void OnReset()
        {
            GetLeftCubeCount();
            UISignals.Instance.onSetLeftText?.Invoke(_data.LeftCubeCount);
        }
    }
}