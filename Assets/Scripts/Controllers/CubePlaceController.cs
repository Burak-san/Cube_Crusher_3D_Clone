using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Controllers
{
    public class CubePlaceController : MonoBehaviour
    {
        private GameObject _selectedObject;

        private void Update()
        {
            ObjectHoldAndGrab();
        }

        private void ObjectHoldAndGrab()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (_selectedObject == null)
                {
                    RaycastHit hit = CastRay();
                    if (hit.collider != null)
                    {
                        if (!hit.collider.CompareTag("Draggable"))
                        {
                            return;
                        }

                        _selectedObject = hit.collider.gameObject;
                        Cursor.visible = false;
                    }

                }
                //BIRAKMAK ICIN
                else
                {
                    Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                        Camera.main.WorldToScreenPoint(_selectedObject.transform.position).z);
                    Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);
                    _selectedObject.transform.position = new Vector3(worldPosition.x, 1.5f, worldPosition.z);
                    _selectedObject = null;
                    Cursor.visible = true;
                }
            }

            // OBJEYI TUTTUGUMUZ YER
            if (_selectedObject != null)
            {
                Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                    Camera.main.WorldToScreenPoint(_selectedObject.transform.position).z);
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);
                _selectedObject.transform.position = new Vector3(worldPosition.x, 2, worldPosition.z);
            }
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
    }
}
