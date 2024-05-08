using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayTrick : MonoBehaviour
{
    public RenderTexture myBlur;
    public Camera mainCam;

    public void CaptureByMainCam(Camera cam)
    {
        mainCam = cam;
        mainCam.targetTexture = myBlur;
        mainCam.Render();
        mainCam.targetTexture = null;
    }
}
