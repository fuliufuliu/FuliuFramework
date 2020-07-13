using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using Random = UnityEngine.Random;

namespace fuliu
{
    /// <summary>
    /// 使挂上该组件的GameObject震动
    /// </summary>
    public class ShakeEffect : MonoBehaviour
    {
        public Vector3 shakeFactor = Vector3.one;
        public float duration = 0.5f;
        private Vector3 localPos;
        private TweenerCore<Vector3, Vector3, VectorOptions> tweenX;
        private TweenerCore<Vector3, Vector3, VectorOptions> tweenY;
        private TweenerCore<Vector3, Vector3, VectorOptions> tweenZ;
        public int keyCount = 15;
        public AnimationCurve curveX;
        public AnimationCurve curveY;
        public AnimationCurve curveZ;

        private void Awake()
        {
            enabled = false;
        }

        private void OnEnable()
        {
            Play();
        }

        public ShakeEffect SetTarget(Vector3 targetPos)
        {
            localPos = targetPos;
            return this;
        }

        public ShakeEffect SetDuration(float duration)
        {
            this.duration = duration;
            return this;
        }
        
        public ShakeEffect SetShakeFactor(Vector3 shakeFactor)
        {
            this.shakeFactor = shakeFactor;
            return this;
        }
        
        public ShakeEffect SetKeyCount(int keyCount)
        {
            this.keyCount = keyCount;
            return this;
        }

        public void Play(Vector3 targetPos)
        {
            localPos = targetPos;
            Play();
        }

        public void Play()
        {
            curveX = GetNewCurve();
            curveY = GetNewCurve();
            curveZ = GetNewCurve();
            var vector = shakeFactor;
            transform.localPosition += vector;
            tweenX = transform.DOLocalMoveX(localPos.x, duration).SetEase(curveX);
            tweenY = transform.DOLocalMoveY(localPos.y, duration).SetEase(curveY);
            tweenZ = transform.DOLocalMoveZ(localPos.z, duration).SetEase(curveZ)
                // .OnComplete(
                // () => { CoroutineHelper.Instance.DelayFrame(1, () => { transform.localPosition = localPos; }); })
                ;
        }

        private void OnDisable()
        {
            if (tweenX != null)
            {
                tweenX.Kill();
            }
            if (tweenY != null)
            {
                tweenY.Kill();
            }
            if (tweenZ != null)
            {
                tweenZ.Kill();
            }
        }

        AnimationCurve GetNewCurve()
        {
            var curve = new AnimationCurve();
            var keys = new Keyframe[keyCount + 1];
            for (int i = 0; i < keyCount; i++)
            {
                var time = (float) i / keyCount;
                keys[i] = new Keyframe(time, (Random.value - 0.5f) * 2);
            }
            keys[keyCount] = new Keyframe(1, 1);
            curve.keys = keys;
            return curve;
        }
        
        
    }
}