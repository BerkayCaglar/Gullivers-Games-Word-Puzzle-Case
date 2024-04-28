using Cinemachine;
using DG.Tweening;
using GameCore.GameFlowSystem;
using UnityEngine;

namespace GameCore.CameraSystem
{
    public class CameraController : MonoBehaviour
    {
        private CinemachineVirtualCamera _virtualCamera;

        private void Awake()
        {
            _virtualCamera = GetComponent<CinemachineVirtualCamera>();
            GameActions.OnGameOver += OnGameOver;
        }

        private void OnDestroy()
        {
            GameActions.OnGameOver -= OnGameOver;
        }

        private void OnGameOver()
        {
            _virtualCamera.transform.DOMoveZ(-10, 1f).SetEase(Ease.InOutSine);
        }
    }
}
