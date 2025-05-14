using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [Header("Camera Settings")]
    public Camera mainCamera;
    [SerializeField, Range(25, 60)] private int fieldOfView = 45;
    public bool enableMSAA = true;
    public bool enableHDR = true;

    [Header("Audio Settings")]
    public AudioListener listener;
    public AudioSource audioSource;
    [SerializeField, Range(0f, 1f)] private float masterVolume = 1f;
    [SerializeField] private bool isSoundEnabled = true; // Default sound state

    [Header("Graphics Settings")]
    [SerializeField, Range(0, 2)] private int shadowQuality = 2; // 0 = Disable, 1 = Low, 2 = Medium, 3 = High
    
    [Header("General or Graphics")]
    public GameObject generalSettings;
    public GameObject graphicsSettings;
    public GameObject settings;
    public int generalValue = 0;
    public int graphicsValue = 0;
    public bool isGeneral = true; // General or Graphics
    public Slider volValue;
    public Slider fovValue;
    public Dropdown shadowValue;

    void Start()
    {
        ApplySettings();
        graphicsValue = 0;
        generalValue = 0;
        isGeneral = true;
    }

    public void ApplySettings()
    {
        // Apply Camera Settings
        if (mainCamera != null)
        {
            mainCamera.fieldOfView = fieldOfView;
            mainCamera.allowMSAA = enableMSAA;
            mainCamera.allowHDR = enableHDR;
        }

        // Apply Audio Settings
        if (audioSource != null)
        {
            audioSource.volume = masterVolume; // Convert linear to decibel
        }

        // Apply Graphics Settings
        SetShadowQuality(shadowQuality);
    }

    public void SetFieldOfView(int fov)
    {
        fieldOfView = Mathf.Clamp(fov, 25, 60);
        if (mainCamera != null)
        {
            mainCamera.fieldOfView = fieldOfView;
        }
    }

    public void SetMSAA(bool enabled)
    {
        enableMSAA = enabled;
        if (mainCamera != null)
        {
            mainCamera.allowMSAA = enableMSAA;
        }
    }

    public void SetHDR(bool enabled)
    {
        enableHDR = enabled;
        if (mainCamera != null)
        {
            mainCamera.allowHDR = enableHDR;
        }
    }

    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
        if (audioSource != null)
        {
            audioSource.volume = masterVolume;
        }
    }

    public void SetShadowQuality(int quality)
    {
        shadowQuality = Mathf.Clamp(quality, 0, 2);

        // Cari semua objek dengan komponen Light di scene
        Light[] lights = FindObjectsOfType<Light>();
        foreach (Light light in lights)
        {
            switch (shadowQuality)
            {
                case 0: // Disable Shadows
                    light.shadows = LightShadows.None;
                    break;
                case 1: // Low Shadows (Hard Only)
                    light.shadows = LightShadows.Hard;
                    break;
                case 2: // Medium Shadows (Soft Shadows)
                    light.shadows = LightShadows.Soft;
                    break;
                case 3: // High Shadows (Soft Shadows, High Quality)
                    light.shadows = LightShadows.Soft;
                    // Anda dapat menambahkan pengaturan tambahan untuk kualitas tinggi
                    break;
            }
            Debug.Log($"Shadow Quality for {light.name} set to: {light.shadows}");
        }

        if (lights.Length == 0)
        {
            Debug.LogWarning("No Lights found in the scene!");
        }
    }

    public void ToggleMSAA()
    {
        enableMSAA = !enableMSAA; // Toggle the current state
        if (mainCamera != null)
        {
            mainCamera.allowMSAA = enableMSAA;
        }
        Debug.Log("MSAA is now " + (enableMSAA ? "enabled" : "disabled"));
    }

    public void ToggleHDR()
    {
        enableHDR = !enableHDR; // Toggle the current state
        if (mainCamera != null)
        {
            mainCamera.allowHDR = enableHDR;
        }
        Debug.Log("HDR is now " + (enableHDR ? "enabled" : "disabled"));
    }

    public void General()
    {
        isGeneral = true;
        generalSettings.SetActive(true);
        graphicsSettings.SetActive(false);
    }

    public void Graphics()
    {
        isGeneral = false;
        generalSettings.SetActive(false);
        graphicsSettings.SetActive(true);
    }

    public void BackToPauseMenu()
    {
        isGeneral = true;
        generalValue = 0;
        graphicsValue = 0;
        generalSettings.SetActive(true);
        graphicsSettings.SetActive(false);
        settings.SetActive(false);
    }

    public void ActiveSettings()
    {
        settings.SetActive(true);
    }

    public void DeactiveSettings()
    {
        settings.SetActive(false);
    }

    public void ToggleSound()
    {
        isSoundEnabled = !isSoundEnabled; // Toggle the sound state

        if (isSoundEnabled)
        {
            listener.enabled = true;
        }
        else
        {
            listener.enabled = false;
        }
    }

    // Mengatur volume berdasarkan nilai slider
    public void OnVolumeSliderChanged()
    {
        SetMasterVolume(volValue.value); // Panggil metode yang sudah ada untuk mengatur volume
    }

    // Mengatur FoV berdasarkan nilai slider
    public void OnFOVSliderChanged()
    {
        SetFieldOfView((int)fovValue.value); // Panggil metode yang sudah ada untuk mengatur FoV
    }

    [System.Obsolete]
    public void OnShadowDropdownChanged()
    {
        SetShadowQuality(shadowValue.value);
    }
}