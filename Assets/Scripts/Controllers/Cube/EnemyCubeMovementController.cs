using DG.Tweening;
using Managers;
using UnityEngine;

namespace Controllers.Cube
{
    public class EnemyCubeMovementController : MonoBehaviour
    {
        private GridManager _gridManager;
        private EnemyCube _enemyCube;
        
        private void Awake()
        {
            _gridManager = FindObjectOfType<GridManager>();
            _enemyCube = GetComponent<EnemyCube>();
        }

        public void Move()
        {
            Vector2Int newPosition = _gridManager._nodes[_enemyCube.tilePosition.x, _enemyCube.tilePosition.y - 1].CellIndex;
            
                if (_gridManager._nodes[newPosition.x, newPosition.y].HeldCube == null)
                {
                    MoveActions(newPosition);
                    
                    transform.DOMove(transform.position + Vector3.back, 1, false);
                }
                else if(_gridManager._nodes[newPosition.x, newPosition.y].HeldCube.TryGetComponent(out IncrementCubes _))
                {
                    MoveActions(newPosition);
                    
                    Cube destroyCube = _gridManager._nodes[newPosition.x, newPosition.y].HeldCube;
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
            _gridManager._nodes[newPosition.x, newPosition.y].HeldCube = _enemyCube;
            _gridManager._nodes[newPosition.x, newPosition.y].IsEnemyTile = true;
            transform.SetParent(_gridManager._nodes[newPosition.x, newPosition.y].gameObject.transform);
            _enemyCube.tilePosition = newPosition;
            
        }
    }
}    