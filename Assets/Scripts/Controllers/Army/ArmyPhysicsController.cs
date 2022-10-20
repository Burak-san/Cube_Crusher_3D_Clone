using System;
using Controllers.EnemyCube;
using Enums;
using Managers;
using Signals;
using UnityEngine;

namespace Controllers.Army
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
                
                //EnemyCubeSignals.Instance.onHitEnemyCube?.Invoke(0.1f,other.gameObject);
                _armyManager.ReturnToPoolArmy(gameObject);
                _armyManager.ArmyCheck();
                Debug.Log("HitEnemyCube");
                
            }
            

            if (other.CompareTag("EnemyBase"))
            {
                _armyManager.ReturnToPoolArmy(gameObject);
                _armyManager.ArmyCheck();
            }
            
            if (other.CompareTag("OurCube"))
            {
                //küpler scale olabilir
            }
        }
    }
}