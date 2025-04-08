using UnityEngine;

public class AspectRatioEnforcer : MonoBehaviour
{
    public float targetWidth = 16f; // Sesuaikan rasio aspek target
    public float targetHeight = 9f;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = GetComponent<Camera>();
        UpdateViewport();
    }

    void UpdateViewport()
    {
        float targetAspect = targetWidth / targetHeight;
        float screenAspect = (float)Screen.width / Screen.height;
        float scaleRatio = screenAspect / targetAspect;

        Rect rect = mainCamera.rect;

        if (scaleRatio < 1.0f)
        {
            rect.width = 1.0f;
            rect.height = scaleRatio;
            rect.y = (1.0f - scaleRatio) / 2.0f;
        }
        else
        {
            rect.width = 1.0f / scaleRatio;
            rect.height = 1.0f;
            rect.x = (1.0f - rect.width) / 2.0f;
        }

        mainCamera.rect = rect;
    }
}