using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace fuliu
{
    public enum SliderFollowMode
    {
        /// <summary>
        /// 减少时跟随，增加时不跟随
        /// </summary>
        ReduceFollow_IncreaseNoFollow,
        /// <summary>
        /// 减少时不跟随，增加时不跟随
        /// </summary>
        ReduceNoFollow_IncreaseFollow,
        /// <summary>
        /// 减少时增加时都跟随
        /// </summary>
        ReduceIncreaseFollow,
    }
    
    /// <summary>
    /// 将此组件挂在 Fill Area（默认创建Slider时，下面有一个GameObject被命名为Fill Area）的 新建子物体（命名为Fill_follow）上，这个子物体负责跟踪 Fill 子物体。
    /// Fill_follow 属性需要设置成：
    /// 若是血条减血效果： Pivot.x = 0   Anchor 为左对齐
    /// 默认创建Slider：
    /// Slider
    /// -| Background
    /// -| Fill Area
    /// --| Fill
    /// -| Handle Slide Area
    /// --| Handle
    /// </summary>
    public class SliderValueFollower : MonoBehaviour
    {
        public RectTransform followTarget;
        public float followSpeed = 1f;
        private RectTransform myRectTrans;
        private Vector2 lastTargetWidthHeight;
        public bool isHorizontal = true;
        private TweenerCore<float, float, FloatOptions> _tween;
        public SliderFollowMode followMode = SliderFollowMode.ReduceFollow_IncreaseNoFollow;
        public Ease ease = Ease.InSine;
        public float delay = 0;

        private void Start()
        {
            myRectTrans = (RectTransform) transform;
        }

        private void LateUpdate()
        {
            var targetWidthHeight = followTarget.rect.size;
            var isChanged = false;
            if (isHorizontal)
            {
                if (Math.Abs(targetWidthHeight.x - lastTargetWidthHeight.x) > 0.0001f)
                {
                    isChanged = true;
                }
            }
            else
            {
                if (Math.Abs(targetWidthHeight.y - lastTargetWidthHeight.y) > 0.0001f)
                {
                    isChanged = true;
                }
            }

            if (isChanged)
            {
                if (_tween != null)
                {
                    _tween.Kill();
                    _tween = null;
                }
                if (isHorizontal)
                {
                    switch (followMode)
                    {
                        case SliderFollowMode.ReduceFollow_IncreaseNoFollow:
                            if (myRectTrans.rect.size.x > followTarget.rect.size.x)
                            {
                                _tween = DOTween.To(() => myRectTrans.rect.size.x,
                                    v => myRectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, v),
                                    targetWidthHeight.x, followSpeed);
                            }
                            else
                            {
                                myRectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, targetWidthHeight.x);
                            }
                            break;
                        case SliderFollowMode.ReduceNoFollow_IncreaseFollow:
                            if (myRectTrans.rect.size.x < followTarget.rect.size.x)
                            {
                                _tween = DOTween.To(() => myRectTrans.rect.size.x,
                                    v => myRectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, v),
                                    targetWidthHeight.x, followSpeed);
                            }
                            else
                            {
                                myRectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, targetWidthHeight.x);
                            }
                            break;
                        case SliderFollowMode.ReduceIncreaseFollow:
                            _tween = DOTween.To(() => myRectTrans.rect.size.x,
                                v => myRectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, v),
                                targetWidthHeight.x, followSpeed);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    
                }
                else
                {
                    switch (followMode)
                    {
                        case SliderFollowMode.ReduceFollow_IncreaseNoFollow:
                            if (myRectTrans.rect.size.y > followTarget.rect.size.y)
                            {
                                _tween = DOTween.To(() => myRectTrans.rect.size.y,
                                    v => myRectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, v),
                                    targetWidthHeight.y, followSpeed);
                            }
                            else
                            {
                                myRectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, targetWidthHeight.y);
                            }
                            break;
                        case SliderFollowMode.ReduceNoFollow_IncreaseFollow:
                            if (myRectTrans.rect.size.y < followTarget.rect.size.y)
                            {
                                _tween = DOTween.To(() => myRectTrans.rect.size.y,
                                    v => myRectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, v),
                                    targetWidthHeight.y, followSpeed);
                            }
                            else
                            {
                                myRectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, targetWidthHeight.y);
                            }
                            break;
                        case SliderFollowMode.ReduceIncreaseFollow:
                            _tween = DOTween.To(() => myRectTrans.rect.size.y,
                                v => myRectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, v),
                                targetWidthHeight.y, followSpeed);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                }

                _tween?.SetEase(ease).SetDelay(delay).OnComplete(() => { _tween = null; });
            }
            
            lastTargetWidthHeight = targetWidthHeight;
        }
    }
}
