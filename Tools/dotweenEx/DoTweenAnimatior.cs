using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public enum DotweenType
{
    Move,
    MoveX,
    MoveY,
    MoveZ,
    LocalMove,
    Rotate,
    Scale,
    LocalMoveX,
    LocalMoveY,
    LocalMoveZ,
    LocalRotate,
    BlendableLocalMoveBy,
    BlendableLocalRotateBy,
    ShakePosition,
    ShakeRotation,
    ShakeScale,
    LookAt,
    ScaleX,
    ScaleY,
    ScaleZ,
    Alpha,
    Fade,
    ImageFillAmount,
}

public class DoTweenAnimatior : BaseAnimator
{
    [NonSerialized]
    public Tweener tweener;
    [Tooltip("是否使用组件当前状态作为动画起始状态？如果不是，则需要设置起始状态")]
    public bool useCurrentValueAsStart = false;
    [Tooltip("动画类型")]
    public DotweenType dotweenType;
    [Tooltip("起始状态")]
    public Vector3 startV3;
    [Tooltip("结束状态")]
    public Vector3 endV3;
    [Tooltip("起始状态")]
    public float startF;
    [Tooltip("结束状态")]
    public float endF;
    [Tooltip("一个周期的时间段")]
    public float duration = 1;
    [Tooltip("延迟多久播放动画")]
    public float delayTime;
    [Tooltip("循环几个周期, 设置为 -1 表示无限循环")]
    public int loopCount = 1;
    [Tooltip("是否使用FixedUpdate更新动画")]
    public bool isFixUpdate;
    [Tooltip("是否对齐值到整数")]
    public bool snapping = false;
    [Tooltip("指定过渡效果")]
    public Ease ease = Ease.Linear;
    [Tooltip("自定义过渡效果")]
    public AnimationCurve EaseCurve;
    [Tooltip("循环类型")]
    public LoopType loopType;
    [Tooltip("旋转模式")]
    public RotateMode rotateMode;
    /// <summary>
    /// 完成时的回调
    /// </summary>
    public TweenCallback onComplete;
    [Tooltip("要约束哪个轴？")]
    public AxisConstraint axisConstraint;
    [Tooltip("定义向上方向的向量(默认:Vector3.up)")]
    public Vector3 up;
    [Tooltip("是否忽略时间比例")]
    public bool isIgnoreTimeScale;
    [Tooltip("更新类型")]
    public UpdateType updateType;
    [Tooltip("震动的力量")]
    public float shakeStrength = 1;
    [Tooltip("震动的频率")]
    public int shakeVibrato = 10;
    [Tooltip("震动的随机性")]
    public float shakeRandomness = 90;
    [Tooltip("震动的随机性")]
    public bool fadeOut = true;

    protected override void OnEnable()
    {
        base.OnEnable();
        if (tweener != null)
        {
            tweener.Kill();
            tweener = null;
        }
        if(isEnable)
            tweener = play();
    }

    protected void OnDisable()
    {
        if (tweener != null)
        {
            tweener.Kill();
            tweener = null;
        }
    }


    public Tweener play()
    {
        switch (dotweenType)
        {
            case DotweenType.Move:
                if(!useCurrentValueAsStart) transform.position = startV3;
                tweener = transform.DOMove(endV3, duration, snapping);
                break;
            case DotweenType.LocalMove:
                if (!useCurrentValueAsStart) transform.localPosition = startV3;
                tweener = transform.DOLocalMove(endV3, duration, snapping);
                break;
            case DotweenType.Rotate:
                if (!useCurrentValueAsStart) transform.eulerAngles = startV3;
                tweener = transform.DORotate(endV3, duration, rotateMode);
                break;

            case DotweenType.Scale:
                if (!useCurrentValueAsStart) transform.localScale = startV3;
                tweener = transform.DOScale(endV3, duration);
                break;
            case DotweenType.ScaleX:
                if (!useCurrentValueAsStart) transform.localScale = new Vector3(startF, transform.localScale.y, transform.localScale.z);
                tweener = transform.DOScaleX(endF, duration);
                break;
            case DotweenType.ScaleY:
                if (!useCurrentValueAsStart) transform.localScale = new Vector3(transform.localScale.x, startF, transform.localScale.z);
                tweener = transform.DOScaleY(endF, duration);
                break;
            case DotweenType.ScaleZ:
                if (!useCurrentValueAsStart) transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, startF);
                tweener = transform.DOScaleZ(endF, duration);
                break;

            case DotweenType.LocalMoveX:
                if (!useCurrentValueAsStart) transform.localPosition = new Vector3(startF, transform.localScale.y, transform.localScale.z);
                tweener = transform.DOLocalMoveX(endF, duration, snapping);
                break;
            case DotweenType.LocalMoveY:
                if (!useCurrentValueAsStart) transform.localPosition = new Vector3(transform.localScale.x, startF, transform.localScale.z);
                tweener = transform.DOLocalMoveY(endF, duration, snapping);
                break;
            case DotweenType.LocalMoveZ:
                if (!useCurrentValueAsStart) transform.localPosition = new Vector3(transform.localScale.x, transform.localScale.y, startF);
                tweener = transform.DOLocalMoveZ(endF, duration, snapping);
                break;

            case DotweenType.MoveX:
                if (!useCurrentValueAsStart) transform.position = new Vector3(startF, transform.localScale.y, transform.localScale.z);
                tweener = transform.DOMoveX(endF, duration, snapping);
                break;
            case DotweenType.MoveY:
                if (!useCurrentValueAsStart) transform.position = new Vector3(transform.localScale.x, startF, transform.localScale.z);
                tweener = transform.DOMoveY(endF, duration, snapping);
                break;
            case DotweenType.MoveZ:
                if (!useCurrentValueAsStart) transform.position = new Vector3(transform.localScale.x, transform.localScale.y, startF);
                tweener = transform.DOMoveZ(endF, duration, snapping);
                break;

            case DotweenType.LocalRotate:
                if (!useCurrentValueAsStart) transform.localEulerAngles = startV3;
                tweener = transform.DOLocalRotate(endV3, duration, rotateMode);
                break;

            case DotweenType.BlendableLocalMoveBy:
                tweener = transform.DOBlendableLocalMoveBy(endV3, duration, snapping);
                break;
            case DotweenType.BlendableLocalRotateBy:
                tweener = transform.DOBlendableLocalRotateBy(endV3, duration, rotateMode);
                break;

            case DotweenType.LookAt:
                if (!useCurrentValueAsStart) tweener = transform.DOLookAt(endV3, duration, axisConstraint, up);
                break;

            case DotweenType.ShakePosition:
                tweener = transform.DOShakePosition(duration, shakeStrength, shakeVibrato, shakeRandomness, snapping, fadeOut);
                break;
            case DotweenType.ShakeRotation:
                tweener = transform.DOShakeRotation(duration, shakeStrength, shakeVibrato, shakeRandomness, fadeOut);
                break;
            case DotweenType.ShakeScale:
                tweener = transform.DOShakeScale(duration, shakeStrength, shakeVibrato, shakeRandomness, fadeOut);
                break;
            case DotweenType.ImageFillAmount:
                var image = GetComponent<Image>();
                if (image)
                {
                    if (!useCurrentValueAsStart) image.fillAmount = startF;
                    tweener = image.DOFillAmount(endF, duration);
                }
                else
                {
                    Debug.LogErrorFormat("{0} 上没有 Image 组件，无法使用 FillAmount 动画", gameObject);
                }
                break;
            case DotweenType.Alpha:
            case DotweenType.Fade:
                var canvasGroup = GetComponent<CanvasGroup>();
                if (canvasGroup != null)
                {
                    if (!useCurrentValueAsStart) canvasGroup.alpha = startF;
                    tweener = canvasGroup.DOFade(endF, duration);
                }

                if (!canvasGroup)
                {
                    var graphic = GetComponent<Graphic>();
                    if (graphic != null)
                    {
                        if (!useCurrentValueAsStart) graphic.SetAlpha(startF);
                        tweener = graphic.DOFade(endF, duration);
                    }
                }

                break;
        }
        if (tweener != null)
        {
            tweener.SetDelay(delayTime).SetLoops(loopCount, loopType).OnComplete(
                () =>
                {
                    if (onComplete != null)
                    {
                        onComplete();
                    }
                    // 处理下一步动画
                    base.Complete();
                }
                );
            if (isFixUpdate)
            {
                tweener.SetUpdate(updateType, isIgnoreTimeScale);
            }
            if (ease == Ease.INTERNAL_Custom)
            {
                tweener.SetEase(EaseCurve);
            }
            else
            {
                tweener.SetEase(ease);
            }
        }
        else
        {
            Debug.LogErrorFormat("不支持： dotweenType  : {0}", dotweenType);
        }
        return tweener;
    }
}
