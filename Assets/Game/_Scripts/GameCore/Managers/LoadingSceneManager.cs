using UnityEngine;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace GameCore.Managers
{
    public class LoadingSceneManager : MonoBehaviour
    {
        public async Task OnSceneLoaded(SceneName targetScene)
        {
            if (Equals(targetScene, SceneName.GameScene))
            {
                // Setup Game Scene
                await Task.Delay(2500);
            }
            else
            {
                await Task.Delay(2500);
            }
        }
    }
}
