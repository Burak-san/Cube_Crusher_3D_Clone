using DG.Tweening;
using Enums;
using Managers;
using Signals;
using UnityEngine;

namespace Controllers.Cube
{
    public class EnemyCubeMovementController : MonoBehaviour
    {
        private GridManager _gridManager;
        private TetrisBlockManager _tetrisBlockManager;
        private EnemyCube _enemyCube;
        
        private void Awake()
        {
            _gridManager = FindObjectOfType<GridManager>();
            _enemyCube = GetComponent<EnemyCube>();
            _tetrisBlockManager = GetComponent<TetrisBlockManager>();
        }

        public void Move()
        {
            Vector2Int newPosition = _gridManager.Nodes[_enemyCube.EnemyCubeTilePosition.x, _enemyCube.EnemyCubeTilePosition.y - 1].CellIndex;
            
                if (_gridManager.Nodes[newPosition.x, newPosition.y].HeldCube == null)
                {
                    MoveActions(newPosition);

                    transform.DOMove(transform.position + Vector3.back, 1, false);
                }
                else if(_gridManager.Nodes[newPosition.x, newPosition.y].HeldCube.TryGetComponent(out IncrementCubes _))
                {
                    
                    Cube destroyCube = _gridManager.Nodes[newPosition.x, newPosition.y].HeldCube;
                    
                    MoveActions(newPosition);
                    
                    transform.DOMove(transform.position + Vector3.back, 1, false).OnComplete((() =>
                    {
                        Destroy(destroyCube.gameObject);
                    }));
                }
                else
                {
                    MoveActions(newPosition);

                    transform.DOMove(transform.position + Vector3.back, 1, false);
                }
        }

        private void MoveActions(Vector2Int newPosition)
        {
            _gridManager.Nodes[newPosition.x, newPosition.y].HeldCube = _enemyCube;
            _gridManager.Nodes[newPosition.x, newPosition.y].IsEnemyTile = true;
            _gridManager.Nodes[newPosition.x, newPosition.y].IsPlaceable = false;
            transform.SetParent(_gridManager.Nodes[newPosition.x, newPosition.y].gameObject.transform);
            _enemyCube.EnemyCubeTilePosition = newPosition;

            if (_gridManager.Nodes[newPosition.x, newPosition.y] == _gridManager.Nodes[newPosition.x,0])
            {
                UISignals.Instance.onOpenPanel?.Invoke(UIPanels.FailPanel);
                UISignals.Instance.onClosePanel?.Invoke(UIPanels.LevelPanel);
                CoreGameSignals.Instance.onChangeGameState?.Invoke(GameStates.GameStop);
            }
        }
    }
}    