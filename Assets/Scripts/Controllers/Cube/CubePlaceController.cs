using System;
using Managers;
using UnityEditor;
using UnityEngine;

namespace Controllers
{
    public class CubePlaceController : MonoBehaviour
    {
        [SerializeField] private LayerMask tileLayer;
        
        private Tile pickedTile;
        
        private TetrisBlockController _selectedObject;
        
        private Camera mainCam;

        private void Awake()
        {
            mainCam = Camera.main;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
                SelectObject(mainCam.ScreenPointToRay(Input.mousePosition));
            
            Debug.DrawRay(mainCam.ScreenPointToRay(Input.mousePosition).origin, mainCam.ScreenPointToRay(Input.mousePosition).direction * 10, Color.yellow);
            
            if (_selectedObject != null)
                MoveObject();
            
            if (Input.GetMouseButtonUp(0))
                DropSelectedObject(DropRay());
        }

        private void SelectObject(Ray ray)
        {
            if (Physics.Raycast(ray, out RaycastHit hitInfo, mainCam.farClipPlane, tileLayer))
            {
                Debug.Log("Layer found");
                if (hitInfo.collider.TryGetComponent(out Tile tile))
                {
                    pickedTile = tile;

                    if (tile.HeldCube == null) return;
                    
                    _selectedObject = tile.HeldCube.parentTetrisBlock;
                    tile.HeldCube.parentTetrisBlock.RemoveCubesFromTiles();
                }
            }
            Debug.Log("ANAN31");
        }

        private void OnDrawGizmos()
        {
            Handles.DrawLine(mainCam.ScreenPointToRay(Input.mousePosition).origin, mainCam.ScreenPointToRay(Input.mousePosition).direction);
        }

        private void MoveObject()
        {
            Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                mainCam.WorldToScreenPoint(_selectedObject.transform.position).z);
            Vector3 worldPosition = mainCam.ScreenToWorldPoint(position);
            _selectedObject.transform.position = new Vector3(worldPosition.x, 1.5f, worldPosition.z);
        }

        private void DropSelectedObject(Ray ray)
        {
            if (Physics.Raycast(ray, out RaycastHit hitInfo, mainCam.farClipPlane, tileLayer))
            {
                if (hitInfo.collider.TryGetComponent(out Tile tile))
                {
                    if (!tile.IsPlaceable) 
                    {
                        DropTetrisBlock();
                    }
                    else
                    {
                        if (_selectedObject.Check(tile.CellIndex))
                        {
                            _selectedObject.Place(tile.CellIndex);
                        }
                        else
                        {
                            DropTetrisBlock();
                        }
                    }
                }
                else
                {
                    DropTetrisBlock();
                }
            }
            else
            {
                DropTetrisBlock();
            }
        }
        
        private Ray DropRay()
        {
            var selectionPos = _selectedObject.transform.position;
            var rayDirection = selectionPos - mainCam.transform.position;
            Ray ray = new Ray(selectionPos, rayDirection);
            return ray;
        }

        private void DropTetrisBlock()
        {
            if (_selectedObject == null) return;
            
            pickedTile.HeldCube = _selectedObject.cubePositions[0].cube;
            pickedTile.SnapPoint();
            _selectedObject = null;
        }
    }
}
