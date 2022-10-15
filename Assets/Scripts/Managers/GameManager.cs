using Enums;
using Signals;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameStates CurrentState;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        GameOpen();
    }

    #region  EventSubscription

    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void SubscribeEvents()
    {
        CoreGameSignals.Instance.onChangeGameState += OnChangeGameState;
        CoreGameSignals.Instance.onPlay += OnPlay; 
    }

    private void UnsubscribeEvents()
    {
        CoreGameSignals.Instance.onChangeGameState -= OnChangeGameState;
        CoreGameSignals.Instance.onPlay -= OnPlay;
    }

    private void OnDisable()
    {
        UnsubscribeEvents();
        GameClose();
    }
    
    private void OnChangeGameState(GameStates newCurrentState)
    {
        CurrentState = newCurrentState;
    }

    private void GameOpen()
    {
        CurrentState = GameStates.GameOpen;
        CoreGameSignals.Instance.onGameOpen?.Invoke();
    }
    private void GameClose()
    {
        CoreGameSignals.Instance.onGameClose?.Invoke();
    }
    

    #endregion
    
    private void OnPlay()
    {
        CurrentState = GameStates.GameOpen;
    }
    
    
}



