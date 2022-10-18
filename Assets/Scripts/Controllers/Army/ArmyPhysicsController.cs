using System;
using Enums;
using Managers;
using Signals;
using UnityEngine;

namespace Controllers.Cube
{
    public class ArmyPhysicsController : MonoBehaviour
    {

        private ArmyManager _armyManager;
        

        private void Awake()
        {
            _armyManager = FindObjectOfType<ArmyManager>();
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("EnemyCube"))
            {
                _armyManager.ReturnToPoolArmy(gameObject);
                _armyManager.ArmyCheck();
            }
            

            // if (other.CompareTag("EnemyBase"))
            // {
            //     _armyManager.ReturnToPoolArmy(gameObject);
            // }
            
            if (other.CompareTag("Draggable"))
            {
                //küpler scale olabilir
            }
        }
    }
}