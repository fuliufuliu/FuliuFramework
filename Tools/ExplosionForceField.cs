// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.Events;
// using Random = UnityEngine.Random;
//
// public class ExplosionForceField : MonoBehaviour
// {
//     public float force = 25f;
//     public float maxRedius = 15;
//     public AnimationCurve forceCurve;
//     public AnimationCurve rediusCurve;
//     public float duration = 2.5f;
//     public bool isExplosing;
//     private float startExplosingTime;
//     private Ball myBall;
//     private float currentForce;
//     private float currentRedius;
//     public UnityAction onExplosionEnd;
//     private bool lastIsExplosing;
//
//
//     public void StartExplosion(Ball myBall)
//     {
//         isExplosing = true;
//         startExplosingTime = Time.fixedTime;
//         this.myBall = myBall;
//     }
//
//
//     // Update is called once per frame
//     void FixedUpdate()
//     {
//         if (!lastIsExplosing && isExplosing)
//         {
//             startExplosingTime = Time.fixedTime;
//         }
//         
//         if (isExplosing && duration > 0)
//         {
//             // 控制半径和力度
//             if (Time.fixedTime - startExplosingTime < duration)
//             {
//                 var time = (Time.fixedTime - startExplosingTime) / duration;
//                 time = Mathf.Clamp(time, 0f, 1f);
// //                transform.position = myBall.transform.position;
//                 currentRedius = maxRedius * rediusCurve.Evaluate(time);
//                 transform.localScale = Vector3.one * currentRedius;
//                 currentForce = force * forceCurve.Evaluate(time);
//                 transform.localEulerAngles = new Vector3(0, Random.Range(0, 360), 0);
//             }
//             else
//             {
//                 isExplosing = false;
//                 onExplosionEnd?.Invoke();
//             }
//         }
//
//         lastIsExplosing = isExplosing;
//     }
//
//     private void OnTriggerEnter(Collider other)
//     {
//         OnTriggerStay(other);
//     }
//
//     private void OnTriggerStay(Collider other)
//     {
//         if (other.isTrigger)
//         {
//             return;
//         }
//         var otherBall = other.GetComponent<Ball>();
//         if (otherBall)
//         {
//             if (otherBall == myBall)
//             {
//                 return;
//             }
//
//             var _rigidBody = otherBall.GetComponent<Rigidbody>();  
//             _rigidBody.AddForce(currentForce * (_rigidBody.position - transform.position + Random.insideUnitSphere).normalized, ForceMode.Impulse);
// //            otherBall.transform.localScale = new Vector3(1,0.5f,1);
//         }
//     }
//     
// //    private void OnTriggerExit(Collider other)
// //    {
// //        if (other.isTrigger)
// //        {
// //            return;
// //        }
// //        var otherBall = other.GetComponent<Ball>();
// //        if (otherBall)
// //        {
// //            if (otherBall == myBall)
// //            {
// //                return;
// //            }
// //
// //            otherBall.transform.localScale = new Vector3(1, 1f, 1);
// //        }
// //    }
// }
