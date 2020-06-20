using System;
using System.Collections;
using UnityEngine;

public class CoroutineHelper : SingleBhv<CoroutineHelper>
{
    IEnumerator _delay(float time, Action action)
    {
        yield return new WaitForSeconds(time);
        action();
    }
    
    IEnumerator _delay(float time, IEnumerator action)
    {
        yield return new WaitForSeconds(time);
        StartCoroutine(action);
    }

    public Coroutine Delay(float time, Action action)
    {
        if (action != null)
        {
            if (time <= 0)
            {
                time = 0;
            }

            return StartCoroutine(_delay(time, action));
        }

        return null;
    }

    public Coroutine DelayFrame(int frameCount, Action action)
    {
        if (action != null)
        {
            frameCount = Mathf.Max(1, frameCount);
            return StartCoroutine(_delayFrame(frameCount, action));
        }

        return null;
    }

    private IEnumerator _delayFrame(int frameCount, Action action)
    {
        for (int i = 0; i < frameCount; i++)
        {
            yield return new WaitForEndOfFrame();
        }
        action();
    }

    public Coroutine Delay(float time, IEnumerator action)
    {
        if (action != null)
        {
            if (time <= 0)
            {
                time = 0;
            }

            return StartCoroutine(_delay(time, action));
        }
        return null;
    }

    public void StopCor(Coroutine coroutine)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
    }

    public void Wait(AsyncOperation asyncOperation, Action callback, Action<float> progressAction = null)
    {
        StartCoroutine(_Wait(asyncOperation, callback, progressAction));
    }

    private IEnumerator _Wait(AsyncOperation asyncOperation, Action callback, Action<float> progressAction)
    {
        var progressValue = 1.0f;
        while (! asyncOperation.isDone)
        {
            if (asyncOperation.progress < 0.9f)
                progressValue = asyncOperation.progress;
            
            progressAction?.Invoke(progressValue);

            if (asyncOperation.progress > 0.89f)
            {
                break;
            }
            yield return null;
        }
        progressAction?.Invoke(1.0f);
        callback?.Invoke();
    }
}
