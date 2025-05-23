using UnityEngine;

public class saklar : OnRaycast
{
    public GameObject lampuDalam;  // Referensi ke lampu
    public Material bolamdalam;     // Referensi ke material bolam
    public bool hidup;             // Kondisi hidup/mati lampu

    // Start is called before the first frame update
    public void Start()
    {
        // Memastikan lampu mati saat mulai
        lampuDalam.SetActive(false);
        bolamdalam.SetColor("_EmissionColor", new Color(0, 0, 0) * 0);
        hidup = false; // Lampu mati saat mulai
    }

    // Update is called once per frame
    public void Update()
    {
        // Tidak perlu memeriksa hidup di sini, cukup menggunakan fungsi OnInteract untuk mengubah kondisi lampu
    }

    public override void OnInteract()
    {
        hidup = !hidup;  // Mengubah status hidup/mati lampu
        
        // Menyalakan atau mematikan lampu sesuai status hidup
        lampuDalam.SetActive(hidup);

        // Menyesuaikan emisi bolam berdasarkan status hidup lampu
        if (hidup)
        {
            // Lampu menyala, emisi meningkat
            bolamdalam.SetColor("_EmissionColor", new Color(1, 1, 1) * 10);
        }
        else
        {
            // Lampu mati, emisi berkurang
            bolamdalam.SetColor("_EmissionColor", new Color(0, 0, 0) * 0);
        }
    }
}
