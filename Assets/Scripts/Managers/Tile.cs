using Controllers;
using UnityEngine;
namespace Managers
{
    public class Tile : MonoBehaviour
    {
        public bool IsPlaceable;
        public Vector2Int CellIndex;
        
        public IncrementCubes HeldCube;
        
        [SerializeField] public Vector3 OffSetVector;

        public void Init(bool isPlaceable, Vector2Int cellIndex){
            this.IsPlaceable = isPlaceable;
            this.CellIndex = cellIndex;
        }
        
        public void SnapPoint()
        {
            HeldCube.transform.position = transform.position + OffSetVector;
        }
    }
}
