using System.Collections.Generic;
using System;
using UnityEngine;

public enum AnimEventType
{
    IsAnimEnd,
    IsOnEnable,
}

[Serializable]
public class AnimEventObject
{
    public float delay;
    public GameObject gameObject;
    public AnimEventType animEventType;
    public bool isEnable = true;
}

public class BaseAnimator : MonoBehaviour {
    [Tooltip("指定播放完此动画后触发下一步要播的哪些动画")]
    public GameObject[] nextGameObjects;

//    [Tooltip("指定和此动画一起播放的动画")]
//    public GameObject[] togetherGameObjects;

    [Tooltip("指定从此组件的各种事件")]
    public AnimEventObject[] eventObjects;
    /// <summary>
    /// 是否在播放完成时关闭GameObject的active属性
    /// </summary>
    [Tooltip("是否在播放完成时关闭GameObject的active属性")]
    public bool isDisactiveOnEnd = false;
    [Tooltip("是否在播放完成时关闭GameObject的active属性,延迟多久？")]
    public float disactiveDelay = 0;
    /// <summary>
    /// 需要在子类中实现它的作用
    /// 如果值为false，表示停止工作。它与MonoBehaviour是不相关的。一个BaseAnimator要正常播放动画，必须开启 isEnable 。
    /// 这个值主要用于暂时不想看到它的动画效果时。
    /// </summary>
    [Tooltip(@"如果值为false，表示停止工作。它与MonoBehaviour是不相关的。一个BaseAnimator要正常播放动画，必须开启 isEnable 。
    这个值主要用于暂时不想看到它的动画效果时。")]
    public bool isEnable = true;
    [Tooltip("自定义的key，用于代码获取并筛选相应的动画组件,以便设置属性值。")]
    public string customKey;


    public BaseAnimator[] getAnimators(string customKey)
    {
        var anims = GetComponentsInChildren<BaseAnimator>();
        List < BaseAnimator > res = new List<BaseAnimator>();
        for (int i = 0; i < anims.Length; i++)
        {
            if (customKey == anims[i].customKey)
            {
                res.Add(anims[i]);
            }
        }

        return res.ToArray();
    }

    public BaseAnimator getAnimator(string customKey)
    {
        var anims = GetComponentsInChildren<BaseAnimator>();
        for (int i = 0; i < anims.Length; i++)
        {
            Debug.Log(i);
            if (customKey == anims[i].customKey)
            {
                return anims[i];
            }
        }
        return null;
    }


    protected virtual void OnEnable()
    {
        disableAllNextAniamtiors();
        enableEventCall(AnimEventType.IsOnEnable);
    }

    protected void enableEventCall(AnimEventType eventType)
    {
        foreach (var ev in eventObjects)
        {
            if (ev != null)
            {
                if (ev.gameObject && ev.animEventType == eventType)
                {
                    Action action = () =>
                    {
                        ev.gameObject.SetActive(ev.isEnable);
                        var nextAnims = ev.gameObject.GetComponents<BaseAnimator>();
                        foreach (var nextAnimatior in nextAnims)
                        {
                            nextAnimatior.enableEventCall(eventType);
                        }
                    };
                    if (ev.delay > 0)
                    {
                        CoroutineHelper.Instance.Delay(ev.delay, action);
                    }
                    else
                    {
                        action();
                    }
                }
            }
        }
    }

    protected void disableAllNextAniamtiors()
    {
        foreach (var go in nextGameObjects)
        {
            // 触发nextAnimatior的OnEnable方法
            if (go)
            {
                go.SetActive(false);
                var nextAnims = go.GetComponents<BaseAnimator>();
                foreach (var nextAnimatior in nextAnims)
                {
                    nextAnimatior.disableAllNextAniamtiors();
                }
            }
        }
    }


    protected void Complete()
    {
        if (nextGameObjects != null && nextGameObjects.Length > 0)
        {
            foreach (var go in nextGameObjects)
            {
                // 触发nextAnimatior的OnEnable方法
                if (go)
                {
                    var nextAnims = go.GetComponents<BaseAnimator>();
                    if (!go.gameObject.activeSelf)
                    {
                        go.gameObject.SetActive(true);
                    }
                    foreach (var nextAnimatior in nextAnims)
                    {
                        if (!nextAnimatior.gameObject.activeSelf)
                        {
                            if (!nextAnimatior.enabled)
                            {
                                nextAnimatior.enabled = true;
                            }
                        }
                        else if (!nextAnimatior.enabled)
                        {
                            nextAnimatior.enabled = true;
                        }
                        else
                        {
                            nextAnimatior.enabled = false;
                            nextAnimatior.enabled = true;
                        }
                    }
                }
            }
        }

        if (isDisactiveOnEnd)
        {
            if (disactiveDelay > 0)
            {
                CoroutineHelper.Instance.Delay(disactiveDelay, () =>
                {
                    gameObject.SetActive(false);
                });
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        enableEventCall(AnimEventType.IsAnimEnd);
    }
}
