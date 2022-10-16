using Enums;
using Signals;
using UnityEngine;

namespace Managers
{
    public class EnemyCubeManager : MonoBehaviour
    {
        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onChangeGameState += OnChangeGameState;
        }
        
        private void UnSubscribeEvents()
        {
            CoreGameSignals.Instance.onChangeGameState -= OnChangeGameState; 
        }
        
        private void OnDisable()
        {
            UnSubscribeEvents();
        }

        private void OnChangeGameState(GameStates currentState)
        {
            if (currentState == GameStates.EnemyMovePhase)
            {
                //dusman MOVE mekanikleri cagirilacak
                Debug.Log("Enemy Move Phase");
                //YUKARIDAKILER YAPILDIKTAN SONRA INVOKE ATILDI
                CoreGameSignals.Instance.onChangeGameState?.Invoke(GameStates.EnemySpawnPhase);
            }
             
            if (currentState == GameStates.EnemySpawnPhase)
            {
                //dusman SPAWN mekanikleri cagirilacak
                Debug.Log("Enemy Spawn Phase");
                //YUKARIDAKILER YAPILDIKTAN SONRA INVOKE ATILDI
                CoreGameSignals.Instance.onChangeGameState?.Invoke(GameStates.Playing);
            }
        }
        
        
        
    }
}