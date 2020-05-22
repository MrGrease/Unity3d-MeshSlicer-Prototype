using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fit : MonoBehaviour
{
    public float sceneWidth = 10f;

    Camera _camera;
    void Start()
    {
        FitToScreen();
    }

    void FitToScreen()
    {
        _camera = GetComponent<Camera>();

        float unitsPerPixel = sceneWidth / Screen.width;

        float desiredHalfHeight = 0.5f * unitsPerPixel * Screen.height;

        _camera.orthographicSize = desiredHalfHeight;
    }
}