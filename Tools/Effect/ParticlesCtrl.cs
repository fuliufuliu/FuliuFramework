using System.Reflection;
using UnityEngine;
using Object = System.Object;

namespace fuliu
{
    public class ParticlesCtrl : MonoBehaviour
    {
        private ParticleSystem[] allParticles;
        private static MethodInfo SetEnabledFunc;
        private PropertyInfo startColorProperty;

        private void Awake()
        {
            if (SetEnabledFunc == null)
            {
#if UNITY_2019
                SetEnabledFunc = typeof(ParticleSystem.EmissionModule).GetMethod("set_enabled");
                startColorProperty = typeof(ParticleSystem.MainModule).GetProperty("startColor");
#elif UNITY_2018
                SetEnabledFunc = typeof(ParticleSystem.EmissionModule).GetMethod("SetEnabled", BindingFlags.Static | BindingFlags.NonPublic);
#endif
            }
            if (startColorProperty == null)
            {
#if UNITY_2019
                startColorProperty = typeof(ParticleSystem.MainModule).GetProperty("startColor");
#elif UNITY_2018
                ???
#endif
            }
            allParticles = GetComponentsInChildren<ParticleSystem>(true);
        }

        public void SetEmission(bool isEmit)
        {
            if (allParticles != null) 
                for (int i = 0; i < allParticles.Length; i++)
                {
#if UNITY_2019
                    SetEnabledFunc.Invoke(allParticles[i].emission, new Object[] {isEmit});
#elif UNITY_2018
                    SetEnabledFunc.Invoke(null, new Object[] {allParticles[i], isEmit});
#endif
                }
        }
        
        public void SetStartColor(Color color)
        {
            if (allParticles == null || allParticles.Length == 0)
            {
                return;
            }
            var data = allParticles[0].main.startColor;
            if (allParticles != null) 
                for (int i = 0; i < allParticles.Length; i++)
                {
                    data.color = color;
                    startColorProperty.SetMethod.Invoke(allParticles[i].main, new Object[] {data});
                }
        }
    }
}
//
// using System;
// using System.Reflection;
// using UnityEngine;
// using Object = System.Object;
//
// namespace fuliu
// {
//     public class ParticlesCtrl : MonoBehaviour
//     {
//         private ParticleSystem[] allParticles;
//
//         private ParticleSystem.MinMaxCurve[] allRateOverTime;
//         // private static MethodInfo SetEnabledFunc;
//
//         private void Awake()
//         {
//             allParticles = GetComponentsInChildren<ParticleSystem>(true);
//             allRateOverTime = new ParticleSystem.MinMaxCurve[allParticles.Length];
//             for (int i = 0; i < allParticles.Length; i++)
//             {
//                 allRateOverTime[i] = allParticles[i].emission.rateOverTime;
//             }
//         }
//
//         // private void Start()
//         // {
//         //     allParticles = GetComponentsInChildren<ParticleSystem>(true);
//         // }
//
//         public void SetEmission(bool isEmit)
//         {
//             // rate = new ParticleSystem.MinMaxCurve (pValue)
//             if (allParticles != null) 
//                 for (int i = 0; i < allParticles.Length; i++)
//                 {
//                     // SetEnabledFunc.Invoke(null, new Object[] {allParticles[i], isEmit});
//                     ParticleSystem.EmissionModule emission = allParticles[i].emission;
//                     if (isEmit)
//                     {
//                         emission.rateOverTime = allRateOverTime[i];
//                     }
//                     else
//                     {
//                         emission.rateOverTime = new ParticleSystem.MinMaxCurve(0);
//                     }
//                 }
//         }
//     }
// }