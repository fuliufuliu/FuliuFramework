using System;
using System.Collections;
using UnityEngine;

namespace fuliu.game {
    public class UnityCoroutineHelper : SingleBhv<UnityCoroutineHelper> {

        /// <summary>
        /// 帮助外部非Monobehavier子类停止Coroutine
        /// </summary>
        /// <param name="coroutine"></param>
        public void stopCoroutine(Coroutine coroutine) {
            if (coroutine != null) 
                StopCoroutine(coroutine);
        }

        /// <summary>
        /// 帮助外部非Monobehavier子类开始Coroutine
        /// </summary>
        /// <param name="coroutine"></param>
        public Coroutine startCoroutine(IEnumerator enumerator) {
           return StartCoroutine(enumerator);
        }

        /// <summary>
        /// 延迟几秒后执行指定的代码
        /// </summary>
        /// <param name="delaySeconds"></param>
        /// <param name="action"></param>
        public Coroutine delayExcute(float delaySeconds, Action action) {
            return StartCoroutine(delayExcuteCor(delaySeconds, action));
        }

        public void delayExcute(float delaySeconds, IEnumerator action) {
            StartCoroutine(delayExcuteCor(delaySeconds, action));
        }

        private IEnumerator delayExcuteCor(float delaySeconds, IEnumerator action)
        {
            yield return new WaitForSeconds(delaySeconds);
            if (action != null)
            {
                StartCoroutine(action);
            }
        }

        private IEnumerator delayExcuteCor(float delaySeconds, Action action) {
            yield return new WaitForSeconds(delaySeconds);
            if (action != null) {
                action();
            }
        }
        /// <summary>
        /// 延迟几帧后执行指定的代码
        /// </summary>
        /// <param name="someFrames"></param>
        /// <param name="action"></param>
        public Coroutine delayFrameExcute(int someFrames, Action action) {
            return StartCoroutine(delayFrameExcuteCor(someFrames, action));
        }

        private IEnumerator delayFrameExcuteCor(int someFrames, Action action) {
            for (int i = 0; i < someFrames; i++) {
                yield return new WaitForEndOfFrame();
            }
            if (action != null) {
                action();
            }
        }
    }
}