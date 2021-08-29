using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Misc
{
    [ExecuteInEditMode]
    public class CanvasScalerController : MonoBehaviour
    {
        public DeviceOrientation deviceOrientation = DeviceOrientation.Landscape;
        public ScaleType type = ScaleType.AspectRatioDepending;

        public float currentAspect;
        public CanvasScaler canvasScaler;

        private float lastCanvasScale = -1000;
        private float currentCanvasScale;
        void Update()
        {
            //Aspect reference depends on device orientation
            if (deviceOrientation == DeviceOrientation.Portrait)
            {
                currentAspect = ((float)Screen.width / (float)Screen.height);
            }
            else if (deviceOrientation == DeviceOrientation.Landscape)
            {
                currentAspect = ((float)Screen.height / (float)Screen.width);
            }

            switch (type)
            {
                case ScaleType.Width:
                    ScaleWidth();
                    break;
                case ScaleType.Height:
                    ScaleHeight();
                    break;
                case ScaleType.Middle:
                    ScaleMiddle();
                    break;
                case ScaleType.AspectRatioDepending:
                    if (currentAspect > 0.5f)
                    {
                        ScaleHeight();
                    }
                    else
                    {
                        ScaleWidth();
                    }
                    break;
            }
        }
        void ScaleWidth()
        {
            currentCanvasScale = 0f;
            UpdateCanvasScale();
        }
        void ScaleHeight()
        {
            currentCanvasScale = 1f;
            UpdateCanvasScale();
        }
        void ScaleMiddle()
        {
            currentCanvasScale = 0.5f;
            UpdateCanvasScale();
        }

        void UpdateCanvasScale()
        {
            if (currentCanvasScale != lastCanvasScale)
            {
                if (canvasScaler != null)
                    canvasScaler.matchWidthOrHeight = currentCanvasScale;
            }
            lastCanvasScale = currentCanvasScale;
        }
    }
}
