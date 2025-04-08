using UnityEngine;

public class hint : MonoBehaviour
{
    public GameObject panel;
    public bool active;
    public GameObject on;
    public GameObject off;
    private ControllerMode controllerMode;
    public PauseManager pause;
    public MonoBehaviour cameraController;

    void Start()
    {
        panel.SetActive(false);
        on.SetActive(false);
        off.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && controllerMode == ControllerMode.PC && !active)
        {
            TogglePedia();
        }
        if (Input.GetKeyDown(KeyCode.Escape) && controllerMode == ControllerMode.PC && active)
        {
            TogglePedia();
        }
    }

    public void TogglePedia()
    {
        active = !active;

        if (active)
        {
            panel.SetActive(true);
            on.SetActive(true);
            off.SetActive(false);

            Time.timeScale = 0f;
            AudioListener.pause = true;
            pause.enabled = false;

            if (cameraController != null)
            {
                cameraController.enabled = false;
            }

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        if (!active)
        {
            panel.SetActive(false);
            on.SetActive(false);
            off.SetActive(true);
            pause.enabled = true;

            Time.timeScale = 1f;
            AudioListener.pause = false;

            if (cameraController != null)
            {
                cameraController.enabled = true;
            }

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

        }
    }
}
