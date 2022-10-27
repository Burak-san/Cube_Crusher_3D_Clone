using Controllers.Cube;
using UnityEngine;
namespace Data.ValueObject
{
    public class TileData : MonoBehaviour
    {
        
        #region Public Variables
        
        public bool IsPlaceable;
        public bool IsEnemyTile;
        public bool IsBaseTile;
        public Vector2Int CellIndex;
        public Material EnemySideMaterial;
        public Vector3 OffSetVector;
        public Cube HeldCube;
        
        #endregion

        public void Init(bool isPlaceable,bool isEnemyTile,bool isBaseTile, Vector2Int cellIndex){
            this.IsPlaceable = isPlaceable;
            this.CellIndex = cellIndex;
            this.IsEnemyTile = isEnemyTile;
            this.IsBaseTile = isBaseTile;
        }
        
        public void SnapPoint()
        {
            HeldCube.transform.position = transform.position + OffSetVector;
        }
    }
}
