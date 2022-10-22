using Controllers.UI;
using Enums;
using Signals;
using TMPro;
using UnityEngine;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        #region Serialized Variables

        [SerializeField] private UIPanelController uiPanelController;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI leftText;
        [SerializeField] private TextMeshProUGUI coinText;

        #endregion

        #region Event Subscriptions

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            UISignals.Instance.onOpenPanel += OnOpenPanel;
            UISignals.Instance.onClosePanel += OnClosePanel;
            UISignals.Instance.onSetLevelText += OnSetLevelText;
            UISignals.Instance.onSetLeftText += OnSetLeftText;
            UISignals.Instance.onSetCoinText += OnSetCoinText;

            CoreGameSignals.Instance.onPlay += OnPlay;
            CoreGameSignals.Instance.onReset += OnReset;
            CoreGameSignals.Instance.onChangeGameState += OnChangeGameState;

            LevelSignals.Instance.onLevelFailed += OnLevelFailed;
        }
        
        private void UnSubscribeEvents()
        {
            UISignals.Instance.onOpenPanel -= OnOpenPanel;
            UISignals.Instance.onClosePanel -= OnClosePanel;
            UISignals.Instance.onSetLevelText -= OnSetLevelText;
            UISignals.Instance.onSetLeftText -= OnSetLeftText;
            UISignals.Instance.onSetCoinText -= OnSetCoinText;
            
            CoreGameSignals.Instance.onPlay -= OnPlay;
            CoreGameSignals.Instance.onReset -= OnReset;
            CoreGameSignals.Instance.onChangeGameState -= OnChangeGameState;

            LevelSignals.Instance.onLevelFailed -= OnLevelFailed;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }
        
        #endregion

        private GameManager _gameManager;
        
        private void Awake()
        {
            _gameManager = FindObjectOfType<GameManager>();
        }

        public void PlayButton()
        {
            CoreGameSignals.Instance.onPlay?.Invoke();
            CoreGameSignals.Instance.onChangeGameState?.Invoke(GameStates.Playing);
        }

        public void RestartButton()
        {
            UISignals.Instance.onClosePanel?.Invoke(UIPanels.FailPanel);
            CoreGameSignals.Instance.onReset?.Invoke();
            CoreGameSignals.Instance.onChangeGameState?.Invoke(GameStates.GameOpen);
        }
        private void OnOpenPanel(UIPanels panel)
        {
            uiPanelController.OpenPanel(panel);
        }
        
        private void OnClosePanel(UIPanels panel)
        {
            uiPanelController.ClosePanel(panel);
        }
        
        private void OnPlay()
        {
            UISignals.Instance.onClosePanel?.Invoke(UIPanels.StartPanel);
        }
        
        private void OnChangeGameState(GameStates currentState)
        {
            
        }
        
        private void OnSetLevelText(int levelID)
        {
            if (levelID == 0)
            {
                levelID = 1;
                levelText.text = "Level " + levelID.ToString();
            }
            else
            {
                levelText.text = "Level " + levelID.ToString();
            }
        }
        
        private void OnSetLeftText(int leftTextValue)
        {
            leftText.text = "Left: " + leftTextValue.ToString();
        }
        
        private void OnSetCoinText(int coinTextValue)
        {
            coinText.text = coinTextValue.ToString();
        }
        
        private void OnLevelFailed()
        {
            OnOpenPanel(UIPanels.FailPanel);
        }
        
        private void OnReset()
        {
            UISignals.Instance.onOpenPanel?.Invoke(UIPanels.StartPanel);
            UISignals.Instance.onOpenPanel?.Invoke(UIPanels.LevelPanel);
        }
    }
}