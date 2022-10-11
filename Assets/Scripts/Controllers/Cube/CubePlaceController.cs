using Managers;
using UnityEngine;

namespace Controllers
{
    public class CubePlaceController : MonoBehaviour
    {
        [SerializeField] private LayerMask mergeLayer;
        private Tile pickedTile;
        private GameObject _selectedObject;

        private Plane plane;
        
        private Vector3 mousePosition;

        public Vector3 smoothMousePosition;

        public bool IsOnGrid;

        private GridManager _gridManager;

        private void Update()
        {
            ObjectHoldAndGrab();
        }

        private void ObjectHoldAndGrab()
        {
            if (Input.GetMouseButtonDown(0))
                SelectObject(Camera.main.ScreenPointToRay(Input.mousePosition));

            // OBJEYI TUTTUGUMUZ YER
            if (_selectedObject != null)
            {
                MoveObject();
            }

            if (Input.GetMouseButtonUp(0))
            {
                //Drop Object
                //Vector2 kismina birakilacak tile'in indexini gir
                _selectedObject.GetComponent<TetrisBlockController>().Place(new Vector2(0,0));
            }
        }

        private void MoveObject()
        {
            Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                Camera.main.WorldToScreenPoint(_selectedObject.transform.position).z);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);
            _selectedObject.transform.position = new Vector3(worldPosition.x, 1.5f, worldPosition.z);
        }

        private RaycastHit CastRay()
        {
            Vector3 screenMousePosFar =
                new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.farClipPlane);
            Vector3 screenMousePosNear =
                new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane);

            Vector3 worldMousePosFar = Camera.main.ScreenToWorldPoint(screenMousePosFar);
            Vector3 worldMousePosNear = Camera.main.ScreenToWorldPoint(screenMousePosNear);

            RaycastHit hit;
            Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out hit);
            return hit;
        }

        private void SelectObject(Ray ray)
        {
            if (Physics.Raycast(ray, out RaycastHit hitInfo, Camera.main.farClipPlane, mergeLayer))
                if (hitInfo.collider.TryGetComponent(out Tile tile))
                {
                    pickedTile = tile;

                    if (tile.HeldCube == null) return;
                    
                    _selectedObject = tile.HeldCube.parentTetrisBlock.gameObject;
                    tile.HeldCube.parentTetrisBlock.RemoveCubesFromTiles();
                }
        }
        
        private void GetMousePositionOnGrid()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
            if (plane.Raycast(ray, out var enter))
            {
                mousePosition = ray.GetPoint(enter);
                smoothMousePosition = mousePosition;
                mousePosition.y = 0;
                mousePosition = Vector3Int.RoundToInt(mousePosition);
                foreach (var node in _gridManager._nodes)
                {
                    // if (node.CellPosition == mousePosition && node.IsPlaceable)
                    // {
                    //     if (Input.GetMouseButtonUp(0))
                    //     {
                    //         node.IsPlaceable = false;
                    //         IsOnGrid = true;
                    //         _selectedObject.transform.position = node.CellPosition + new Vector3(0, 1, 0);
                    //         _selectedObject = null;
                    //     }
                    // }
                }
            }
        }
    }
}
