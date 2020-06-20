using System;
using DG.Tweening;
using UnityEngine;

namespace UnityEditor
{
    [CustomEditor(typeof(DoTweenAnimatior))]
    public class DoTweenAnimatiorEditor : BaseAnimatorEditor
    {
        protected override void Gui(BaseAnimator _anim)
        {
            DoTweenAnimatior anim = _anim as DoTweenAnimatior;
            base.Gui(anim);
            if (!anim.isEnable)
            {
                GUI.backgroundColor = Color.green;
                showProperty("dotweenType");
                return;
            }
            else
            {
                GUI.backgroundColor = Color.blue;
            }

            EditorGUILayout.LabelField("Attributes", EditorStyles.objectFieldThumb);

            GUI.backgroundColor = Color.green;
            showProperty("dotweenType");
            EditorGUI.indentLevel++;
            switch (anim.dotweenType)
            {
                case DotweenType.ShakePosition:
                case DotweenType.ShakeRotation:
                case DotweenType.ShakeScale:
                    break;
                default:
                    showProperty("useCurrentValueAsStart");
                    break;
            }

            switch (anim.dotweenType)
            {
                case DotweenType.Move:
                case DotweenType.LocalMove:
                case DotweenType.Rotate:
                case DotweenType.Scale:
                case DotweenType.LocalRotate:
                case DotweenType.BlendableLocalMoveBy:
                case DotweenType.BlendableLocalRotateBy:
                    if (!anim.useCurrentValueAsStart)
                    {
                        showProperty("startV3");
                    }

                    showProperty("endV3");
                    break;
                case DotweenType.ShakePosition:
                case DotweenType.ShakeRotation:
                case DotweenType.ShakeScale:
                    showProperty("shakeStrength");
                    showProperty("shakeVibrato");
                    showProperty("shakeRandomness");
                    showProperty("fadeOut");
                    break;
                case DotweenType.LookAt:
                    if (!anim.useCurrentValueAsStart)
                    {
                        showProperty("startV3");
                    }

                    showProperty("endV3");

                    showProperty("axisConstraint");
                    showProperty("up");
                    break;
                case DotweenType.MoveX:
                case DotweenType.MoveY:
                case DotweenType.MoveZ:
                case DotweenType.LocalMoveX:
                case DotweenType.LocalMoveY:
                case DotweenType.LocalMoveZ:
                case DotweenType.ScaleX:
                case DotweenType.ScaleY:
                case DotweenType.ScaleZ:
                case DotweenType.ImageFillAmount:
                case DotweenType.Alpha:
                case DotweenType.Fade:
                    if (!anim.useCurrentValueAsStart)
                    {
                        showProperty("startF");
                    }

                    showProperty("endF");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            switch (anim.dotweenType)
            {
                case DotweenType.Move:
                case DotweenType.LocalMove:
                case DotweenType.MoveX:
                case DotweenType.MoveY:
                case DotweenType.MoveZ:
                case DotweenType.LocalMoveX:
                case DotweenType.LocalMoveY:
                case DotweenType.LocalMoveZ:
                case DotweenType.BlendableLocalMoveBy:
                case DotweenType.ShakePosition:
                    showProperty("snapping");
                    break;
                case DotweenType.Rotate:
                case DotweenType.LocalRotate:
                    showProperty("rotateMode");
                    break;
                case DotweenType.Scale:
                    break;
                case DotweenType.BlendableLocalRotateBy:
                    break;
                case DotweenType.ShakeRotation:
                case DotweenType.ShakeScale:
                    break;
                case DotweenType.LookAt:
                    break;
                case DotweenType.ScaleX:
                case DotweenType.ScaleY:
                case DotweenType.ScaleZ:
                case DotweenType.ImageFillAmount:
                case DotweenType.Alpha:
                case DotweenType.Fade:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            EditorGUI.indentLevel--;
            showProperty("duration");
            showProperty("delayTime");
            showProperty("loopCount");
            showProperty("loopType");

            GUI.backgroundColor = Color.cyan;
            showProperty("ease");
            if (anim.ease == Ease.INTERNAL_Custom)
            {
                showProperty("EaseCurve");
            }
        }
    }
}
