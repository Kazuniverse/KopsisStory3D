using UnityEngine;
using cherrydev;
using System.Collections;

public class InteractNPC : OnRaycast
{
    [SerializeField] private Transform npcTransform;
    [SerializeField] private Movement playerMovement;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private DialogBehaviour dialogBehaviour;
    public GameObject[] canvas;

    [Header("Camera Settings")]
    [SerializeField] private float cameraDistance = 3f;
    [SerializeField] private float cameraHeight = 1.5f;
    [SerializeField] public GameObject bobber;
    [SerializeField] public Animator animate;

    [Header("Transition Settings")]
    [SerializeField] private float transitionDuration = 1f;

    private static bool isInConversation = false;
    private static Vector3 staticCameraOriginalPosition;
    private static Quaternion staticCameraOriginalRotation;
    
    private Quaternion npcOriginalRotation;
    public DialogSetup dialogSetup;

    private void OnEnable()
    {
        if (dialogBehaviour != null)
        {
            dialogBehaviour.OnDialogEnded += OnConversationEnded;
        }
    }

    private void OnDisable()
    {
        if (dialogBehaviour != null)
        {
            dialogBehaviour.OnDialogEnded -= OnConversationEnded;
        }
    }

    public override void OnInteract()
    {
        if (!isInConversation)
        {
            StartConversation();
            foreach (GameObject isi in canvas)
            {
                isi.SetActive(false);
            }
        }
        gameObject.layer = LayerMask.NameToLayer("Default");
    }

    private void StartConversation()
    {
        if (dialogSetup == null) return;

        dialogSetup.StartDialogForNPC();
        isInConversation = true;

        bobber.GetComponent<AudioSource>().enabled = false;
        animate.SetFloat("walk", 0);
        animate.SetBool("fall", false);

        SaveOriginalStates();
        StartCoroutine(SmoothTransitionToConversation());

        TogglePlayerMovement(false);
        ShowCursor(true);
    }

    private IEnumerator SmoothTransitionToConversation()
    {
        float elapsedTime = 0f;

        Quaternion npcStartRotation = npcTransform.rotation;
        Quaternion cameraStartRotation = mainCamera.transform.rotation;
        Vector3 cameraStartPosition = mainCamera.transform.position;

        Vector3 directionToPlayer = playerTransform.position - npcTransform.position;
        directionToPlayer.y = 0;
        Quaternion npcTargetRotation = Quaternion.LookRotation(directionToPlayer);

        Vector3 directionToNPC = npcTransform.position - mainCamera.transform.position;
        directionToNPC.y = 0;
        directionToNPC.Normalize();
        Vector3 cameraTargetPosition = npcTransform.position + (-directionToNPC * cameraDistance) + Vector3.up * cameraHeight;
        Quaternion cameraTargetRotation = Quaternion.LookRotation(npcTransform.position - cameraTargetPosition);

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / transitionDuration;

            npcTransform.rotation = Quaternion.Slerp(npcStartRotation, npcTargetRotation, t);
            mainCamera.transform.position = Vector3.Lerp(cameraStartPosition, cameraTargetPosition, t);
            mainCamera.transform.rotation = Quaternion.Slerp(cameraStartRotation, cameraTargetRotation, t);

            yield return null;
        }

        npcTransform.rotation = npcTargetRotation;
        mainCamera.transform.position = cameraTargetPosition;
        mainCamera.transform.rotation = cameraTargetRotation;
    }

    private void SaveOriginalStates()
    {
        if (npcTransform != null)
        {
            npcOriginalRotation = npcTransform.rotation;
        }

        if (mainCamera != null)
        {
            staticCameraOriginalPosition = mainCamera.transform.position;
            staticCameraOriginalRotation = mainCamera.transform.rotation;
        }
    }

    private void TogglePlayerMovement(bool enable)
    {
        if (playerMovement != null)
        {
            playerMovement.enabled = enable;
        }
    }

    private void ShowCursor(bool show)
    {
        if (show)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            // Kunci dan sembunyikan cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void OnConversationEnded()
    {
        EndConversation();
    }

    private void EndConversation()
    {
        StartCoroutine(SmoothTransitionToOriginal());
        bobber.GetComponent<AudioSource>().enabled = true;

        foreach (GameObject isi in canvas)
        {
            isi.SetActive(true);
        }

        isInConversation = false;
        
        // Pastikan cursor disembunyikan
        ShowCursor(false);
    }

    private IEnumerator SmoothTransitionToOriginal()
    {
        float elapsedTime = 0f;

        Quaternion npcStartRotation = npcTransform.rotation;
        Quaternion cameraStartRotation = mainCamera.transform.rotation;
        Vector3 cameraStartPosition = mainCamera.transform.position;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / transitionDuration;

            npcTransform.rotation = Quaternion.Slerp(npcStartRotation, npcOriginalRotation, t);
            mainCamera.transform.position = Vector3.Lerp(cameraStartPosition, staticCameraOriginalPosition, t);
            mainCamera.transform.rotation = Quaternion.Slerp(cameraStartRotation, staticCameraOriginalRotation, t);

            yield return null;
        }

        mainCamera.transform.position = staticCameraOriginalPosition;
        mainCamera.transform.rotation = staticCameraOriginalRotation;

        TogglePlayerMovement(true);
        gameObject.layer = LayerMask.NameToLayer("Interacted");
    }
}