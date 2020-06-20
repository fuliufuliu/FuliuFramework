using System;
using System.Collections;

namespace UnityEngine.UI
{
    [RequireComponent(typeof(Slider))]
    public class SliderJumpEffect: MonoBehaviour
    {
        public float startNum;
        public float endNum;
        public float duration;
        private Coroutine cor;
        private float stepTime = 0.05f;
        private Slider mySlider;

        private void Awake()
        {
            mySlider = GetComponent<Slider>();
        }

        public void StopCor()
        {
            if (cor != null)
            {
                StopCoroutine(cor);
            }
        }

        private void OnEnable()
        {
            if (duration > 0.00001f)
            {
                Jump(startNum, endNum, duration);
            }
        }

        public void Jump(float startNum, float endNum, float duration, Action onComplete = null)
        {
            StopCor();
            cor = StartCoroutine(_Jump(startNum, endNum, duration, onComplete));
        }

        private IEnumerator _Jump(float startNum, float endNum, float duration, Action onComplete)
        {
            var count = (int)(duration / stepTime);
            if (count == 0)
            {
                yield break;
            }
            var step = (endNum - startNum) / (float)count;
            mySlider.value = startNum;
            for (int i = 0; i < count; i++)
            {
                mySlider.value = startNum + step * i;
                yield return new WaitForSecondsRealtime(stepTime);
            }
            mySlider.value = endNum;
            onComplete?.Invoke();
        }

        private void OnDestroy()
        {
            StopCor();
        }
    }
}