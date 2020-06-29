using UnityEngine;

namespace fuliu
{
    public class TrailsCtrl: MonoBehaviour
    {
        public TrailRenderer[] allTrailRenderers;
        
        private void Awake()
        {
            if (allTrailRenderers == null || allTrailRenderers.Length == 0)
            {
                allTrailRenderers = GetComponentsInChildren<TrailRenderer>(true);
            }
        }
        
        public void SetColor(Color color)
        {
            if (allTrailRenderers == null || allTrailRenderers.Length == 0)
            {
                return;
            }
            // var data = allTrailRenderers[0].startColor;
            if (allTrailRenderers != null) 
                for (int i = 0; i < allTrailRenderers.Length; i++)
                {
                    allTrailRenderers[i].startColor = color;
                }
        }
    }
}