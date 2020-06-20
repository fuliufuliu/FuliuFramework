using UnityEngine;
using UnityEngine.UI;

namespace DG.Tweening {
    public delegate float GetFloat(); 
    public delegate void SetFloat(float f); 

    /// <summary>
    /// 针对NGUI的UIWidget对Dotween做一些扩展
    /// </summary>
    public static class DotweenEx {
#if NGUI
        public static Tweener DOAlpha(this UIWidget target, float endValue, float duration) {
            return DOTween.To(() => target.alpha, a => target.alpha = a,endValue, duration);
        }
        
        public static Tweener DOFloat(this UIWidget target, GetFloat getFloat, SetFloat setFloat, float endValue, float duration) {
            return DOTween.To(() => getFloat(), a => setFloat(a), endValue, duration);
        }
        public static Tweener DOColor(this UIWidget target, Color endValue, float duration) {
            return DOTween.To(() => target.color, c => target.color = c, endValue, duration);
        }
#endif
        public static Tweener DoColor(this Material target, string propertyInShader, Color endValue, float duration)
        {
            return DOTween.To(() => target.GetColor(propertyInShader),
                a => target.SetColor(propertyInShader, a), 
                endValue, 
                duration);
        }

        public static void SetColor(this Material target, string propertyInShader, Color endValue)
        {
            target.SetColor(propertyInShader, endValue);
        }


        public static void SetAlpha(this Graphic graphics, float alpha)
        {
            var color = graphics.color;
            color.a = alpha;
            graphics.color = color;
        }

        public static float GetAlpha(this Graphic graphics)
        {
            return graphics.color.a;
        }

        public static Tweener DoColor(this Graphic target, Color endValue, float duration)
        {
            return DOTween.To(() => target.color,
                a => target.color = a,
                endValue,
                duration);
        }

        public static Tweener DoAlpha(this Graphic target, float endValue, float duration)
        {
            return target.DOFade(endValue, duration);
//            return DOTween.To(() => target.color.a,
//                a =>{
//                    var color = target.color;
//                    color.a = a;
//                    target.color = color;
//                },
//                endValue,
//                duration);
        }

        public static Tweener DoAlpha(this CanvasGroup target, float endValue, float duration)
        {
            return DOTween.To(() => target.alpha,
                a => target.alpha = a,
                endValue,
                duration);
        }

		public static Tweener DoAnchoredPositionV2(this RectTransform target,Vector2 endValue,float duration)
		{
			return DOTween.To (() => target.anchoredPosition,
				a => target.anchoredPosition = a,
				endValue,
				duration);
		}
    }
}
