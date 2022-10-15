using Enums;
using Signals;
using UnityEngine;

namespace Managers
{
    public class ArmyManager : MonoBehaviour
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
            if (currentState == GameStates.AttackPhase)
            {
                //HUMAN INSTANTIATE CART CURT
                
                //YUKARIDAKILER YAPILDIKTAN SONRA INVOKE ATILDI
                Debug.Log("attac kc");
                CoreGameSignals.Instance.onChangeGameState?.Invoke(GameStates.EnemyMovePhase);
            }
        }
    }
}