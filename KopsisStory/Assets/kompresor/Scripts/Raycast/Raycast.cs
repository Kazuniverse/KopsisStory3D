using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Raycast : MonoBehaviour
{
    public float range;
    public LayerMask interacted;

    public GameObject dotUI; // Dot di tengah layar
    public Image interactRaycast; // Gambar di lokasi objek yang di-raycast
    public TextMeshProUGUI textInteract;
    public Camera cam;

    private bool isRaycast;
    private OnRaycast onRaycast;
    private ControllerMode controllerMode;

    [System.Obsolete]
    void Start()
    {
        controllerMode = GetComponentInParent<Movement>().readControllerMode;
    }

    private void Update()
    {
        // Cek jika game sedang dijeda, hentikan proses raycast dan interaksi
        if (Time.timeScale == 0)
        {
            isRaycast = false;
            dotUI.SetActive(false);
            interactRaycast.gameObject.SetActive(false);
            textInteract.text = "";
            return;
        }

        isRaycast = Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, range, interacted);

        if (isRaycast)
        {
            onRaycast = hit.transform.GetComponent<OnRaycast>();

            if (onRaycast != null && onRaycast.HasInteracted)
            {
                isRaycast = false;
            }
        }

        dotUI.SetActive(isRaycast);
        interactRaycast.gameObject.SetActive(isRaycast);

        if (isRaycast && onRaycast != null)
        {
            Vector3 pointRaycast = onRaycast.pointInteract != null ? onRaycast.pointInteract.position : hit.transform.position;
            interactRaycast.transform.position = cam.WorldToScreenPoint(pointRaycast);

            // Tambahkan cek status jeda sebelum interaksi
            if (Input.GetKeyDown(KeyCode.E) && controllerMode == ControllerMode.PC && Time.timeScale != 0)
            {
                onRaycast.OnInteract();
            }

            textInteract.text = onRaycast.information;
        }
    }

    public void Interact()
    {
        // Cek jika game dijeda atau tidak ada objek yang diraycast
        if (Time.timeScale == 0|| !isRaycast || onRaycast == null) return;

        onRaycast.OnInteract();
    }
}
