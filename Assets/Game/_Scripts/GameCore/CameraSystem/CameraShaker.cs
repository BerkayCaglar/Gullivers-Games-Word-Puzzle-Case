using UnityEngine;

namespace GameCore.CameraSystem
{
    public class CameraShaker : MonoBehaviour
    {
        private static Cinemachine.CinemachineImpulseSource _impulseSource;
        public static void ShakeCamera() => _impulseSource.GenerateImpulse();
        public static void ShakeCamera(float force) => _impulseSource.GenerateImpulse(force);
        public static void ShakeCamera(Vector3 force) => _impulseSource.GenerateImpulse(force);

        private void Awake()
        {
            _impulseSource = GetComponent<Cinemachine.CinemachineImpulseSource>();
        }
    }
}
