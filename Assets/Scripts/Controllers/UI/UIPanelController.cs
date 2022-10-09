using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace Controllers.UI
{
    public class UIPanelController : MonoBehaviour
    {
        [SerializeField] private List<GameObject> UIPanelList = new List<GameObject>();

        public void OpenPanel(UIPanels panelState)
        {
            UIPanelList[(int)panelState].SetActive(true);
        }

        public void ClosePanel(UIPanels panelState)
        {
            UIPanelList[(int)panelState].SetActive(false);
        }
    }
}