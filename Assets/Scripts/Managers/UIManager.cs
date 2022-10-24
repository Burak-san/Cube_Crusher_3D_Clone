using Controllers.UI;
using Data.UnityObject;
using Data.ValueObject;
using Enums;
using Signals;
using Sirenix.OdinInspector;
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
        
        [SerializeField] private TextMeshProUGUI powerButtonLevelText;
        [SerializeField] private TextMeshProUGUI powerButtonCoinText;
        
        [SerializeField] private TextMeshProUGUI coinButtonLevelText;
        [SerializeField] private TextMeshProUGUI coinButtonCoinText;

        #endregion

        #region Private Variables

        [ShowInInspector]private MoneyData _moneyData;
        private GameManager _gameManager;

        #endregion
        
        private void Awake()
        {
            GetDataResources();
        }

        private void GetDataResources()
        {
            _moneyData = GetMoneyData();
            _gameManager = FindObjectOfType<GameManager>();
            coinText.text = _moneyData.TotalMoney.ToString();
            powerButtonLevelText.text = "Level " + _moneyData.PowerLevel.ToString();
            powerButtonCoinText.text = _moneyData.PowerMoneyDecrease.ToString();
            coinButtonLevelText.text = "Level " + _moneyData.GainCoinLevel.ToString();
            coinButtonCoinText.text = _moneyData.GainCoinDecrease.ToString();
        }
        

        private MoneyData GetMoneyData() => Resources.Load<CD_Money>("Data/CD_Money").MoneyData;
        
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
            
            LevelSignals.Instance.onLevelFailed += OnLevelFailed;
            LevelSignals.Instance.onNextLevel += OnNextLevel;
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

            LevelSignals.Instance.onLevelFailed -= OnLevelFailed;
            LevelSignals.Instance.onNextLevel -= OnNextLevel;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }
        
        #endregion
        

        public void PlayButton()
        {
            CoreGameSignals.Instance.onPlay?.Invoke();
            CoreGameSignals.Instance.onChangeGameState?.Invoke(GameStates.Playing);
        }

        public void BaseCubePowerIncrease()
        {
            if (_moneyData.TotalMoney - _moneyData.PowerMoneyDecrease < 0)
            {
                return;
            }
            _moneyData.BaseCubeValue += 1;
            BaseCubeSignals.Instance.onBaseCubePowerIncrease?.Invoke();
            _moneyData.TotalMoney -= _moneyData.PowerMoneyDecrease;
            _moneyData.PowerMoneyDecrease += _moneyData.PowerMoneyDecrease;
            _moneyData.PowerLevel += 1;

            powerButtonCoinText.text = _moneyData.PowerMoneyDecrease.ToString();
            powerButtonLevelText.text = "Level " + _moneyData.PowerLevel.ToString();
            coinText.text = _moneyData.TotalMoney.ToString();
        }

        public void GainMoneyIncrease()
        {
            if (_moneyData.TotalMoney - _moneyData.GainCoinDecrease < 0)
            {
                return;
            }
            _moneyData.GainMoney += 1;
            _moneyData.TotalMoney -= _moneyData.GainCoinDecrease;
            _moneyData.GainCoinDecrease += _moneyData.GainCoinDecrease;
            _moneyData.GainCoinLevel += 1;

            coinButtonCoinText.text = _moneyData.GainCoinDecrease.ToString();
            coinButtonLevelText.text = "Level " + _moneyData.GainCoinLevel.ToString();
            coinText.text = _moneyData.TotalMoney.ToString();
            
        }
        public void RestartButton()
        {
            UISignals.Instance.onClosePanel?.Invoke(UIPanels.FailPanel);
            CoreGameSignals.Instance.onReset?.Invoke();
            CoreGameSignals.Instance.onChangeGameState?.Invoke(GameStates.GameOpen);
        }

        public void NextLevelButton()
        {
            UISignals.Instance.onClosePanel?.Invoke(UIPanels.WinPanel);
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
        
        private void OnSetCoinText()
        {
            _moneyData.TotalMoney += _moneyData.GainMoney;
            coinText.text = _moneyData.TotalMoney.ToString();
        }
        
        private void OnLevelFailed()
        {
            OnOpenPanel(UIPanels.FailPanel);
        }

        private void OnNextLevel()
        {
            OnOpenPanel(UIPanels.WinPanel);
        }

        private void OnReset()
        {
            LevelSignals.Instance.onClearActiveLevel?.Invoke();
            LevelSignals.Instance.onLevelInitialize?.Invoke();
            
            UISignals.Instance.onOpenPanel?.Invoke(UIPanels.StartPanel);
            UISignals.Instance.onOpenPanel?.Invoke(UIPanels.LevelPanel);
        }
    }
}