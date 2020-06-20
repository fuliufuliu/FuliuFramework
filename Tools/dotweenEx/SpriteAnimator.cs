#define _USE_TO_LUA
//#define TEST

using System;
//using LuaInterface;
using UnityEngine;
using UnityEngine.UI;

public class SpriteAnimator : BaseAnimator
{
    [Tooltip("是否在Awake中唤醒动画")]
    public bool playAwake;
    [Tooltip("序列帧")]
    public Sprite[] frames;
    [Tooltip("播放速度，帧与帧之间的时间间隔")]
    public float speed = 0.2f;
    [Tooltip("如果开启，在 Awake 中从uguiAtlas中找符合条件的sprite，并填充到属性 frames中去。必须指定 uguiAtlas")]
    public bool findAtlas = true;
    public UguiAtlas uguiAtlas;
    [Tooltip("前缀")]
    public string prefix;
    [Tooltip("是否循环播放")]
    [Obsolete("请使用loopCount变量来控制是否循环！")]
    [NonSerialized]
    public bool isLoop;
    [Tooltip("循环次数,小于0为无限循环")]
    public int loopCount = -1;
    [Tooltip("延迟时间")]
    public float delay = 0;
    [Tooltip("延迟时间前是否显示")]
    public bool isShowBeforeDelay = true;
    [Tooltip("最大播放帧数")]
    public int maxCount = int.MaxValue;

#if USE_TO_LUA
    /// <summary>
    /// 调用frameEvent是第几帧
    /// </summary>
    [NonSerialized]
    public float actionFrame = -1f;
    /// <summary>
    /// 指定帧切换的事件
    /// </summary>
    public LuaFunction frameEvent;
    /// <summary>
    /// 停止事件，只有在isLoop为false时才会触发
    /// </summary>
    public LuaFunction stopEvent;
#endif

    private Image container;
    private int ticked;
    private float time;
    private bool doAnim;
	private int m_TickedCount;
    private int currentLoopCount = 0;
    private float delayTime;

    private void Awake()
	{
		container = GetComponent<Image> ();
        if (findAtlas && null != uguiAtlas)
        {
#if USE_TO_LUA
            SetAtlas(uguiAtlas, prefix, null);
#endif
        }
        else
        {
            Init();
        }

	}

    protected override void OnEnable()
    {
        base.OnEnable();
        Play();
    }

    private void Init()
    {
        ticked = 0;
		m_TickedCount = 0;
        time = 0;
        doAnim = playAwake;
        if (frames.Length > 0)
        {
            container.sprite = frames[0];
        }
    }
#if USE_TO_LUA
	public void SetAtlas(UguiAtlas uguiAtlas, string prefix, LuaFunction theCallback, int playFrameCount)
	{
		this.maxCount = playFrameCount;
		SetAtlas (uguiAtlas,prefix,theCallback);
	}

	public void SetAtlas(UguiAtlas uguiAtlas, string prefix, LuaFunction theCallback)
	{
	    Stop();
        if (uguiAtlas)
        {
            this.stopEvent = theCallback;
            var list = new List<Sprite>();

            if (string.IsNullOrEmpty(prefix))
            {
                list = uguiAtlas.sprites.ToList();
            }
            else
            {
                foreach (var sprite in uguiAtlas.sprites)
                {
                    if (sprite.name.StartsWith(prefix))
                    {
                        list.Add(sprite);
                    }
                }
            }
            if (list.Count > 0)
            {
                list.Sort((a, b) =>
                {
                    return string.CompareOrdinal(a.name, b.name);
                });
            }
            frames = list.ToArray();
        }
        Init();
    }
#endif

    public void Play()
    {
        ticked = 0;
		m_TickedCount = 0;
        time = 0;
        doAnim = true;
#if TEST
        Debug.Log("Play动画!");
#endif
        if (frames.Length > 0)
        {
            container.sprite = frames[0];
        }

        delayTime = delay + Time.time;
        currentLoopCount = 0;
        if (!isShowBeforeDelay)
        {
            container.enabled = false;
        }
    }

    public void Pause()
    {
        doAnim = false;
    }

    public void Resume()
    {
        doAnim = true;
    }

    public void Stop()
    {
        ticked = 0;
		m_TickedCount = 0;
        time = 0;
#if TEST
        Debug.Log("Stop动画播放完成");
#endif
        doAnim = false;
        if (frames.Length > 0)
        {
            container.sprite = frames[0];
        }
    }

    void Update()
    {
        if (isEnable && doAnim)
        {
            if (Time.time < delayTime)
            {
                return;
            }
            else if(!container.enabled)
            {
                container.enabled = true;
                time = 0;
                return;
            }
            if (frames == null || frames.Length == 0)
            {
                Debug.LogErrorFormat("frames.Length 是零！无可播放的sprite");
                doAnim = false;
                time = 0;
                return;
            }
            time += Time.deltaTime;
            if (time > speed)
            {
                ticked++;
				m_TickedCount++;
                if (ticked == frames.Length && ((currentLoopCount < loopCount || loopCount < 1) || ticked < maxCount))
                {
                    ticked = 0;
                    if (loopCount > 0)
                    {
                        currentLoopCount++;
                    }
                }
                    
                time = 0;
#if USE_TO_LUA
                if (ticked == actionFrame && frameEvent != null)
                {
                    frameEvent.Call();
                }
#endif
				if (ticked < frames.Length) {
					container.sprite = frames [ticked];
				}
				if(((this.maxCount > 0)?( m_TickedCount >= this.maxCount):(ticked >= frames.Length))
				    || (currentLoopCount >= loopCount && loopCount > 0)
                )
                {
                    doAnim = false;
#if TEST
                    Debug.LogErrorFormat("1动画播放完成. maxCount:{0}, ticked:{1}, frames.Length:{2}, m_TickedCount: {3}", maxCount, ticked, frames.Length, m_TickedCount);
#endif

#if USE_TO_LUA
                    if (stopEvent != null)
                    {
                        stopEvent.Call();
                    }
#endif
#if TEST
                    Debug.LogErrorFormat("2动画播放完成. maxCount:{0}, ticked:{1}, frames.Length:{2}, m_TickedCount: {3}", maxCount, ticked, frames.Length, m_TickedCount);
#endif
                    base.Complete();
#if TEST
                    Debug.LogErrorFormat("3动画播放完成. maxCount:{0}, ticked:{1}, frames.Length:{2}, m_TickedCount: {3}", maxCount, ticked, frames.Length, m_TickedCount);
#endif
                }
            }
        }
    }
}