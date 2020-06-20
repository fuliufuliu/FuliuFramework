using UnityEngine;

namespace fuliu
{
    public class GoSwitcher : MonoBehaviour
    {
        public GameObject[] gos;
        private int currentIndex;
        private float lastSwitchTime;

        private void Switch()
        {
            if (gos != null)
            {
                if (currentIndex < gos.Length) gos[currentIndex]?.SetActive(true);
                currentIndex++;
                if (currentIndex == gos.Length)
                {
                    // 全部显示
                    return;
                }
                if (currentIndex > gos.Length)
                {
                    currentIndex = 0;
                }
                gos[currentIndex]?.SetActive(false);
            }
        }

        private void Update()
        {
            if (Application.isEditor && Input.GetMouseButton(1) && Input.GetMouseButton(2)
                ||
                Application.isMobilePlatform && Input.touchCount == 5 )
            {
                if (Time.unscaledTime - lastSwitchTime > 1)
                {
                    lastSwitchTime = Time.unscaledTime;
                    Switch();
                }
            }
        }
    }
}