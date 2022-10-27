using Commands.Level;
using Data.UnityObject;
using Data.ValueObject;
using Signals;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Managers
{
    public class LevelManager : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables

        [Header("Data")] public LevelData LevelData;

        #endregion

        #region Serialized Variables

        [Space] [SerializeField] private GameObject levelHolder;
        

        #endregion

        #region Private Variables

        [ShowInInspector] private int _levelID;
        
        private LevelLoaderCommand _levelLoaderCommand;
        private ClearActiveLevelCommand _clearActiveLevelCommand;
        
        #endregion

        #endregion

        private void Awake()
        {
            GetData();
            
        }
        
        private void GetData()
        {
            LevelData = GetLevelData();
            LevelData.InıtializeLevelID();
            LevelData.LevelIDCount = SaveLoadManager.LoadValue("LevelIDCount",LevelData.LevelIDCount);
            UISignals.Instance.onSetLevelText?.Invoke(LevelData.LevelIDCount);
            _clearActiveLevelCommand = new ClearActiveLevelCommand(ref levelHolder);
            _levelLoaderCommand = new LevelLoaderCommand(ref levelHolder);
        }
        
        private void Start()
        {
            InitializeLevel();
        }
        
        private void InitializeLevel()
        {
            var newLevelData = 0;
            _levelLoaderCommand.Execute(newLevelData);
            SaveLoadManager.SaveValue("LevelIDCount",SaveLoadManager.LoadValue("LevelIDCount",LevelData.LevelIDCount));
            LevelData.LevelIDCount = SaveLoadManager.LoadValue("LevelIDCount",LevelData.LevelIDCount);
            UISignals.Instance.onSetLevelText?.Invoke(LevelData.LevelIDCount);
        }
        
        private LevelData GetLevelData()
        {
            int newLevelData = _levelID % Resources.Load<CD_Level>("Data/CD_Level").Levels.Count;
            return Resources.Load<CD_Level>("Data/CD_Level").Levels[newLevelData];
        }
        
        #region Event Subscription

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            LevelSignals.Instance.onLevelInitialize += OnInitializeLevel;
            LevelSignals.Instance.onClearActiveLevel += OnClearActiveLevel;
            LevelSignals.Instance.onNextLevel += OnNextLevel;
            LevelSignals.Instance.onGetLevel += OnGetLevel;
        }

        private void UnsubscribeEvents()
        {
            LevelSignals.Instance.onLevelInitialize -= OnInitializeLevel;
            LevelSignals.Instance.onClearActiveLevel -= OnClearActiveLevel;
            LevelSignals.Instance.onNextLevel -= OnNextLevel;
            LevelSignals.Instance.onGetLevel -= OnGetLevel;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion
        
        private void OnInitializeLevel()
        {
            var newLevelData = 0;
            _levelLoaderCommand.Execute(newLevelData);
            SaveLoadManager.SaveValue("LevelIDCount",SaveLoadManager.LoadValue("LevelIDCount",LevelData.LevelIDCount)+1);
            LevelData.LevelIDCount = SaveLoadManager.LoadValue("LevelIDCount",LevelData.LevelIDCount);
            UISignals.Instance.onSetLevelText?.Invoke(LevelData.LevelIDCount);
        }
        
        private void OnClearActiveLevel()
        {
            _clearActiveLevelCommand.Execute();
        }

        private int OnGetLevel()
        {
            return LevelData.LevelIDCount;
        }

        private void OnNextLevel()
        {
            LevelSignals.Instance.onClearActiveLevel?.Invoke();
            LevelSignals.Instance.onLevelInitialize?.Invoke();
            CoreGameSignals.Instance.onReset?.Invoke();
        }
    }
}