using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace gs.chef.game.Scripts.Objects
{
    public class GoalPiece : MonoBehaviour
    {
        public SkinnedMeshRenderer Renderer;
        public int PointCount;
        public List<GameObject> Points = new List<GameObject>();

        Tween tween;

        public void BounceAnimationTrigger(float duration, int value, Ease ease)
        {
            tween?.Kill();

            tween = DOTween.To(() => 0, x => Renderer.SetBlendShapeWeight(0, x), value, duration).SetEase(ease)
                .OnComplete(() =>
                {
                    tween = DOTween.To(() => value, x => Renderer.SetBlendShapeWeight(0, x), 0, duration)
                        .SetEase(ease);
                });
        }
    }
}