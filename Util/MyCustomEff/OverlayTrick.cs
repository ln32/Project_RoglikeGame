using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayTrick : MonoBehaviour
{
    [SerializeField] protected RenderTexture myBlur;
    [SerializeField] protected Camera mainCam;

    public void CaptureByMainCam(Camera cam)
    {
        mainCam = cam;
        mainCam.targetTexture = myBlur;
        mainCam.Render();
        mainCam.targetTexture = null;
    }
}
