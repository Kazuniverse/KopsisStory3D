using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public bool isPaused = false; // Status pause game
    public GameObject pauseMenuUI;
    public GameObject enable;
    public GameObject disable;
    public MonoBehaviour cameraController;
    public hint hint;

    void Start()
    {
        pauseMenuUI.SetActive(false);
        Cursor.visible = false; // Sembunyikan pointer mouse saat game dimulai
        Cursor.lockState = CursorLockMode.Locked; // Kunci pointer mouse di tengah layar
    }

    void Update()
    {
        // Cek input untuk menjeda/melanjutkan game (misalnya tombol Escape)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    // Fungsi untuk menjeda/melanjutkan game
    public void TogglePause()
    {
        isPaused = !isPaused; // Toggle status pause

        if (isPaused)
        {
            PauseGame();
            enable.SetActive(false);
            disable.SetActive(true);
            hint.enabled = false;
        }
        else
        {
            ResumeGame();
            disable.SetActive(false);
            enable.SetActive(true);
            hint.enabled = true;
        }
    }

    // Fungsi untuk menjeda game
    public void PauseGame()
    {
        Time.timeScale = 0f; // Menghentikan waktu game
        AudioListener.pause = true; // Menjeda semua audio
        pauseMenuUI.SetActive(true); // Menampilkan UI pause menu

        // Nonaktifkan kontrol kamera
        if (cameraController != null)
        {
            cameraController.enabled = false;
        }

        // Tampilkan pointer mouse dan bebaskan kursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Debug.Log("Game Paused");
    }

    // Fungsi untuk melanjutkan game
    public void ResumeGame()
    {
        Time.timeScale = 1f; // Mengembalikan waktu game ke normal
        AudioListener.pause = false; // Melanjutkan semua audio
        pauseMenuUI.SetActive(false); // Menyembunyikan UI pause menu

        // Aktifkan kembali kontrol kamera
        if (cameraController != null)
        {
            cameraController.enabled = true;
        }

        // Sembunyikan pointer mouse dan kunci kursor di tengah layar
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        Debug.Log("Game Resumed");
    }

    // Fungsi untuk tombol Resume di UI
    public void OnResumeButtonClicked()
    {
        TogglePause();
    }

    // Fungsi untuk tombol Quit di UI
    public void OnQuitButtonClicked()
    {
        Debug.Log("Quitting Game...");
        Application.Quit(); // Keluar dari game (hanya bekerja di build, bukan di editor)
    }
}