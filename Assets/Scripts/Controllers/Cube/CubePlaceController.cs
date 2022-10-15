using System;
using Managers;
using UnityEditor;
using UnityEngine;

namespace Controllers
{
    public class CubePlaceController : MonoBehaviour
    {
        [SerializeField] private LayerMask tetrisLayer;
        [SerializeField] private LayerMask tileLayer;
        
        private Tile pickedTile;
        
        private TetrisBlockController _selectedObject;
        private Vector3 pickedPosition;
        
        private Camera mainCam;

        private void Awake()
        {
            mainCam = Camera.main;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
                SelectObject(mainCam.ScreenPointToRay(Input.mousePosition));

            //TODO ECHOS: delete this after
            Debug.DrawRay(mainCam.ScreenPointToRay(Input.mousePosition).origin, mainCam.ScreenPointToRay(Input.mousePosition).direction * mainCam.farClipPlane, Color.yellow);
            
            if (_selectedObject != null)
                MoveObject();
            
            if (Input.GetMouseButtonUp(0) && _selectedObject != null)
                DropSelectedObject(DropRay());
        }

        private void SelectObject(Ray ray)
        {
            if (Physics.Raycast(ray, out RaycastHit hitInfo, mainCam.farClipPlane, tetrisLayer))
                if (hitInfo.collider.transform.parent.TryGetComponent(out TetrisBlockController tbc))
                {
                    _selectedObject = tbc;
                    pickedPosition = tbc.transform.position;
                }

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
                            //Check if can merge
                            //Spawn stickmans
                            //move enemy blok + spawn tetris blok
                            //new enemy cubes
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
            
            _selectedObject.transform.position = pickedPosition;
            _selectedObject = null;
        }
    }
}
