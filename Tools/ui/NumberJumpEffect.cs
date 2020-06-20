using System;
using System.Collections;

namespace UnityEngine.UI
{
    [RequireComponent(typeof(Text))]
    public class NumberJumpEffect: MonoBehaviour
    {
        public int startNum;
        public int endNum;
        public float duration;
        public string format = string.Empty;
        private Text myText;
        private Coroutine cor;
        private float stepTime = 0.05f;

        private void Awake()
        {
            myText = GetComponent<Text>();
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
                Jump(format, startNum, endNum, duration);
            }
        }

        public void Jump(string format, int startNum, int endNum, float duration, Action onComplete = null)
        {
            StopCor();
            cor = StartCoroutine(_Jump(format, startNum, endNum, duration, onComplete));
        }

        private IEnumerator _Jump(string format, int startNum, int endNum, float duration, Action onComplete)
        {
            var count = (int)(duration / stepTime);
            if (count == 0)
            {
                yield break;
            }
            var step = (endNum - startNum) / (float)count;
            myText.text = string.Format(format, startNum);
            for (int i = 0; i < count; i++)
            {
                myText.text = string.Format(format, (int)(startNum + step * i));
                yield return new WaitForSecondsRealtime(stepTime);
            }
            myText.text = string.Format(format, endNum);
            onComplete?.Invoke();
        }

        private void OnDestroy()
        {
            StopCor();
        }
    }
}