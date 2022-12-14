using System.Threading.Tasks;
using Cinemachine;
using Enums;
using Signals;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Managers
{
    public class CameraManager : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables

        public CinemachineStateDrivenCamera StateCam;
        
        #endregion

        #region Private Variables

        [ShowInInspector] private Vector3 _initialPosition;

        private Animator _animator;
        private CameraStatesType _cameraStatesType;
        
        #endregion

        #endregion

        private void Awake()
        {
            GetData();
            GetInitialPosition();
        }

        private void GetData()
        {
            _animator = GetComponent<Animator>();
            _cameraStatesType = CameraStatesType.GameOpen;
        }

        private void GetInitialPosition()
        {
            _initialPosition = transform.localPosition;
        }

        #region Event Subscription

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onPlay += OnPlay;
            CoreGameSignals.Instance.onChangeGameState += OnChangeGameState;
            CoreGameSignals.Instance.onReset += OnReset;
         
            LevelSignals.Instance.onNextLevel += OnNextLevel;
        }

    

        private void UnsubscribeEvents()
        {
            CoreGameSignals.Instance.onPlay -= OnPlay;
            CoreGameSignals.Instance.onChangeGameState -= OnChangeGameState;
            CoreGameSignals.Instance.onReset -= OnReset;
         
            LevelSignals.Instance.onNextLevel -= OnNextLevel;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
            
        }

        #endregion

        private void OnPlay()
        {
            SetCameraTarget(CameraStatesType.Playing);
        }
        
        private void SetCameraTarget(CameraStatesType cameraCurrentState)
        {
            _animator.Play(cameraCurrentState.ToString());
        }

        private void OnChangeGameState(GameStates currentGameStates)
        {
            switch (currentGameStates)
            {
                case GameStates.GameOpen :
                    _cameraStatesType = CameraStatesType.GameOpen;
                    SetCameraTarget(_cameraStatesType);
                    break;
                case  GameStates.Playing :
                    _cameraStatesType = CameraStatesType.Playing;
                    SetCameraTarget(_cameraStatesType);
                    break;
            }
        }

        private void OnNextLevel()
        {
            CameraTargetSetting();
        }
        private async void CameraTargetSetting()
        {
            await Task.Delay(50);
            SetCameraTarget(CameraStatesType.GameOpen);
        }

        private void OnReset()
        {
            CameraTargetSetting();
        }
    }
}