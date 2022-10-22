using Controllers;
using Controllers.Cube;
using UnityEngine;
namespace Managers
{
    public class Tile : MonoBehaviour
    {
        public bool IsPlaceable;
        public Vector2Int CellIndex;
        public bool IsEnemyTile;
        
        public Cube HeldCube;
        
        [SerializeField] public Vector3 OffSetVector;

        public void Init(bool isPlaceable,bool isEnemyTile, Vector2Int cellIndex){
            this.IsPlaceable = isPlaceable;
            this.CellIndex = cellIndex;
            this.IsEnemyTile = isEnemyTile;
        }
        
        public void SnapPoint()
        {
            HeldCube.transform.position = transform.position + OffSetVector;
        }
    }
}
