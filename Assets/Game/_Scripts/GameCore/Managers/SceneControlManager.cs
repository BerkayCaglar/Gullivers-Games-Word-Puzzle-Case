using GameCore.SingletonSystem;
using UnityEngine.SceneManagement;
using GameCore.UI;
using System;
using System.Threading.Tasks;
using UnityEngine.UI;
using MyBox;

namespace GameCore.Managers
{
    public class SceneControlManager : PersistentSingleton<SceneControlManager>
    {
        private event Action OnSceneLoaded, OnSceneBeginLoad;

        private void Start()
        {
            OnSceneLoaded += OnSceneLoadedEvent;
            OnSceneBeginLoad += OnSceneBeginLoadEvent;
        }

        public async Task LoadSceneWithFadeInAnimation(SceneName targetScene, bool useLoadingScene)
        {
            OnSceneBeginLoad?.Invoke();

            if (useLoadingScene)
            {
                await CameraUIAnimations.Instance.StartFadeInFadeOutAnimation(async () =>
                {
                    await LoadSceneAsync(SceneName.LoadingScene, targetScene);

                    await CameraUIAnimations.Instance.StartFadeInFadeOutAnimation(async () =>
                    {
                        await LoadSceneAsync(targetScene);
                    });
                });
            }
            else
            {
                await CameraUIAnimations.Instance.StartFadeInFadeOutAnimation(async () =>
                {
                    await LoadSceneAsync(targetScene);
                });
            }
        }

        private async Task LoadSceneAsync(SceneName loadImmidiateScene, SceneName targetScene = default)
        {
            var loadSceneAsyncOperation = SceneManager.LoadSceneAsync(loadImmidiateScene.ToString());

            while (!loadSceneAsyncOperation.isDone) { await Task.Yield(); }

            if (targetScene == default) { return; }

            var loadingSceneManager = FindObjectOfType<LoadingSceneManager>();
            await loadingSceneManager.OnSceneLoaded(targetScene);
            OnSceneLoaded?.Invoke();
        }

        private void OnSceneLoadedEvent()
        {
            SetActiveSceneUIObjects(isActive: true);
        }

        private void OnSceneBeginLoadEvent()
        {
            SetActiveSceneUIObjects(isActive: false);
        }

        private void SetActiveSceneUIObjects(bool isActive)
        {
            SetActiveAllSceneButtons(isActive);
        }

        private void SetActiveAllSceneButtons(bool isActive)
        {
            var buttons = FindObjectsOfType<Button>();
            foreach (var button in buttons)
            {
                button.interactable = isActive;
            }
        }

        [ButtonMethod]
        public async void LoadMainMenuScene()
        {
            await LoadSceneWithFadeInAnimation(SceneName.MainMenu, useLoadingScene: true);
        }

        [ButtonMethod]
        public async void LoadGameScene()
        {
            await LoadSceneWithFadeInAnimation(SceneName.GameScene, useLoadingScene: true);
        }
    }
}

[Serializable]
public enum SceneName
{
    SplashScreen,
    LoadingScene,
    MainMenu,
    GameScene
}
