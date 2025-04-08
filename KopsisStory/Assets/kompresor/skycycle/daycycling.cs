using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine.UI; // Jangan lupa import namespace untuk UI

public class daycycling : MonoBehaviour 
{
    public GameObject[] lampLuar; // Array untuk lampu luar
    public GameObject[] pedagang;
    public Material bolamluar;
    public GameObject lampumerah; // Lampu merah
    public GameObject fobj; // Objek lainnya
    [SerializeField] private Light sun; // Matahari
    [SerializeField, Range(0, 24)] public float timeOfDay; // Waktu dalam sehari
    [SerializeField] private float sunRotationSpeed; // Kecepatan rotasi matahari
    [Header("Lightning Preset")]
    [SerializeField] private Gradient skyColor; // Warna langit
    [SerializeField] private Gradient equatorColor; // Warna ekuator
    [SerializeField] private Gradient sunColor; // Warna matahari
    [Header("Skybox Set")]
    public Material mat1;
    public Material mat2;
    public Material mat3;
    public Material mat4;
    public Material mat5;
    [Header("UI Settings")]
    public GameObject skipDay;
    public Text timeDisplayText; // Referensi ke UI Text untuk menampilkan waktu
    public npcroute route;
    public GameObject[] npcSiswa;
    [Header("NPC Fade Settings")]
    public float fadeDuration = 2.0f; // Durasi fade dalam detik

    void Start()
    {
        if (timeOfDay > 16.0f || timeOfDay >= 0 && timeOfDay < 5.66667f)
        {   
            foreach (GameObject siswa in npcSiswa)
            {
                siswa.SetActive(false);
            }
        }
    }

    private void Update()
    {
        timeOfDay += Time.deltaTime * sunRotationSpeed;
        if (timeOfDay > 24)
            timeOfDay = 0;

        UpdateTimeDisplay(); // Panggil fungsi untuk memperbarui tampilan waktu

        // Horror ------------------------------------------------------------
        if (timeOfDay < 23 && timeOfDay > 3)
        {
            fobj.SetActive(false);
            lampumerah.SetActive(false);
        }

        // Lampu --------------------------------------------------------------
        if (timeOfDay >= 19 || timeOfDay <= 5)
        {
            foreach (GameObject lamp in lampLuar)
            {
                lamp.SetActive(true); // Menyalakan semua lampu luar
            }
            bolamluar.SetColor("_EmissionColor", new Color(1, 1, 1) * 20);
        }
        if (timeOfDay < 19 && timeOfDay > 5)
        {
            foreach (GameObject lamp in lampLuar)
            {
                lamp.SetActive(false); // Mematikan semua lampu luar
            }
            bolamluar.SetColor("_EmissionColor", new Color(0, 0, 0) * 0);
        }

        // Langit -------------------------------------------------------------
        //// Pagi -----------------------------
        if (timeOfDay > 5 && timeOfDay <= 8)
        {
            RenderSettings.skybox = mat1;
        }
        //// Siang ----------------------------
        if (timeOfDay > 8 && timeOfDay <= 17 )
        {
            RenderSettings.skybox = mat2;
        }
        //// Sore -----------------------------
        if (timeOfDay > 17 && timeOfDay <= 19)
        {
            RenderSettings.skybox = mat3;
        }
        //// Malam ----------------------------
        if (timeOfDay > 3 && timeOfDay <= 5 || timeOfDay > 19 && timeOfDay <= 23)
        {
            RenderSettings.skybox = mat5;
        }
        //// MidMalam -------------------------
        if (timeOfDay > 23 || timeOfDay <= 3)
        {
            RenderSettings.skybox = mat4;
            lampumerah.SetActive(true);
            fobj.SetActive(true);
        }
        if (timeOfDay < 15 || timeOfDay > 17)
        {
           foreach (GameObject gerobak in pedagang)
            {
                gerobak.SetActive(false);
            }
        }
        if (timeOfDay >= 15 && timeOfDay <= 17)
        {
           foreach (GameObject gerobak in pedagang)
            {
                gerobak.SetActive(true);
            }
        }
        if ((timeOfDay >= 21 || timeOfDay <= 2) && Input.GetKeyDown(KeyCode.Return))
        {
            timeOfDay = 5;
        }
        if (timeOfDay >= 21 || timeOfDay <= 2)
        {
            skipDay.SetActive(true);
        }
        if (timeOfDay <= 21 && timeOfDay >= 2)
        {
            skipDay.SetActive(false);
        }

        if (timeOfDay > 16.0f || timeOfDay >= 0 && timeOfDay < 5.66667f)
        {   
            StartCoroutine(IlangGantian());
        }

        if (timeOfDay >= 5.66667f && timeOfDay <= 16.0f)
        {
            StartCoroutine(SpawnGantian());
        }

        UpdateSunRotation();
        UpdateLightning();
    }

    IEnumerator SpawnGantian()
    {
        foreach (GameObject siswa in npcSiswa)
        {
            siswa.SetActive(true);
            float randomDelay = Random.Range(2, 3);
            yield return new WaitForSeconds(randomDelay);
        }
    }
    IEnumerator IlangGantian()
    {
        foreach (GameObject siswa in npcSiswa)
        {
            siswa.SetActive(false);
            float randomDelay = Random.Range(2, 3);
            yield return new WaitForSeconds(randomDelay);
        }
    }

    private void OnValidate()
    {
        UpdateSunRotation();
        UpdateLightning();
    }

    private void UpdateSunRotation()
    {
        float sunRotation = Mathf.Lerp(-90, 270, timeOfDay / 24);
        sun.transform.rotation = Quaternion.Euler(sunRotation, sun.transform.rotation.y, sun.transform.rotation.z);
    }

    private void UpdateLightning()
    {
        float timeFraction = timeOfDay / 24;
        RenderSettings.ambientEquatorColor = equatorColor.Evaluate(timeFraction);
        RenderSettings.ambientSkyColor = skyColor.Evaluate(timeFraction);
        sun.color = sunColor.Evaluate(timeFraction);
    }

    // Fungsi untuk mengonversi timeOfDay ke format jam:menit dan menampilkannya di UI
    private void UpdateTimeDisplay()
    {
        if (timeDisplayText != null)
        {
            int hours = Mathf.FloorToInt(timeOfDay); // Ambil jam
            int minutes = Mathf.FloorToInt((timeOfDay - hours) * 60); // Ambil menit

            // Format waktu menjadi "HH:MM"
            string timeString = string.Format("{0:00}:{1:00}", hours, minutes);
            timeDisplayText.text = timeString;
        }
    }

    IEnumerator FadeOutNPC(GameObject npc)
    {
        // Dapatkan semua renderer pada NPC dan anak-anaknya
        Renderer[] renderers = npc.GetComponentsInChildren<Renderer>();
        List<Material> materials = new List<Material>();
        
        // Kumpulkan semua material
        foreach (Renderer renderer in renderers)
        {
            materials.AddRange(renderer.materials);
        }

        // Proses fade out
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            
            foreach (Material material in materials)
            {
                Color color = material.color;
                color.a = alpha;
                material.color = color;
                
                // Untuk material yang menggunakan Standard Shader dengan mode Fade/Transparent
                if (material.HasProperty("_Mode"))
                {
                    material.SetFloat("_Mode", 2); // 2 = Fade mode
                    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    material.EnableKeyword("_ALPHABLEND_ON");
                    material.renderQueue = 3000;
                }
            }
            yield return null;
        }

        // Setelah selesai fade, nonaktifkan NPC
        npc.SetActive(false);
        
        // Reset alpha untuk下次使用
        foreach (Material material in materials)
        {
            Color color = material.color;
            color.a = 1f;
            material.color = color;
        }
    }
}