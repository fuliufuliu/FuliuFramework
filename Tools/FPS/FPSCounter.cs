using UnityEngine;
using UnityEngine.UI;

namespace fuliu
{
    public class FPSCounter : MonoBehaviour
    {
        public Text m_Text;
        const float fpsMeasurePeriod = 0.5f;
        private int m_FpsAccumulator = 0;
        private float m_FpsNextPeriod = 0;
        private int m_CurrentFps;
        private string currentText;
        const string display = "{0} FPS";
        public GUIStyle guiStyle = new GUIStyle();
        private static FPSCounter instance;

        private void Awake()
        {
            if (instance)
            {
                gameObject.SetActive(false);
                Destroy(gameObject);
                return;
            }
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            m_FpsNextPeriod = Time.realtimeSinceStartup + fpsMeasurePeriod;
        }

        private void Update()
        {
            m_FpsAccumulator++;
        }

        private void FixedUpdate()
        {
            currentText = GetFps();
            if (m_Text)
            {
                m_Text.text = currentText;
            }
        }

        private void OnGUI()
        {
            if (! m_Text)
                GUILayout.Label(currentText, guiStyle);
        }

        string GetFps()
        {
            // measure average frames per second
            if (Time.realtimeSinceStartup > m_FpsNextPeriod)
            {
                m_CurrentFps = (int) (m_FpsAccumulator/fpsMeasurePeriod);
                m_FpsAccumulator = 0;
                m_FpsNextPeriod += fpsMeasurePeriod;
                return string.Format(display, m_CurrentFps);
            }

            return currentText;
        }
    }
}
