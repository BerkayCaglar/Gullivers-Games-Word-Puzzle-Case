using System.Linq;
using System.Threading.Tasks;
using DG.Tweening;
using GameCore.CameraSystem;
using GameCore.PopupSystem.VFXPoolSystem;
using UnityEngine;

namespace GameCore.AnimationSystem
{
    public class AnimationsManager
    {
        public static async Task PlayExplosionAnimation(Transform target)
        {
            VFXPooler.Instance.SpawnFromPool(VFXType.ConfettiBurstVFX, target.position, Quaternion.identity, 0.5f);
            await target.DOPunchScale(Vector3.one * 0.1f, 0.1f).AsyncWaitForCompletion();
            CameraShaker.ShakeCamera(0.2f);
            await target.DOScale(Vector3.zero, 0.1f).AsyncWaitForCompletion();
            target.gameObject.SetActive(false);
        }

        public static async Task PlayMoveAnimation(Transform obj, Vector3 targetPos, Vector3 targetScale, bool useLocalMove, bool useVFX)
        {
            await obj.DOPunchScale(Vector3.one * 0.1f, 0.1f).AsyncWaitForCompletion();

            switch (useLocalMove)
            {
                case true:
                    await obj.DOLocalJump(targetPos, Random.Range(0.1f, 0.2f), Random.Range(1, 2), 0.1f).SetEase(Ease.InOutQuad).AsyncWaitForCompletion();
                    break;
                case false:
                    await obj.DOJump(targetPos, Random.Range(0.1f, 0.2f), Random.Range(1, 2), 0.1f).SetEase(Ease.InOutQuad).AsyncWaitForCompletion();
                    break;
            }

            if (useVFX)
            {
                VFXPooler.Instance.SpawnFromPool(VFXType.PuffVFX, obj.position, 0.5f);
                float[] randoms = Enumerable.Range(0, 3)
                    .Select(_ => Random.Range(0.1f, 0.2f))
                    .ToArray();
                CameraShaker.ShakeCamera(new Vector3(randoms[0], randoms[1], randoms[2]));
            }

            await obj.DOScale(targetScale, 0.1f).AsyncWaitForCompletion();
            await obj.DOPunchScale(Vector3.one * 0.1f, 0.1f).AsyncWaitForCompletion();

            obj.localScale = targetScale;
        }

        public static async Task PlayPunchAnimation(Transform obj)
        {
            if (DOTween.IsTweening(obj))
            {
                obj.DOKill();
                obj.localScale = Vector3.one;
            }

            await obj.DOPunchScale(Vector3.one * 0.1f, 0.1f).AsyncWaitForCompletion();
        }
    }
}
