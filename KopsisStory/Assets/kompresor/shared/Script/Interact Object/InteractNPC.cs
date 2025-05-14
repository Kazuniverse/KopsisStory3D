using UnityEngine;
using cherrydev;
using System.Collections;

public class InteractNPC : OnRaycast
{
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

    public bool isInConversation = false;
    private static Vector3 staticCameraOriginalPosition = Vector3.zero;
    private static Quaternion staticCameraOriginalRotation = Quaternion.identity;

    private static Quaternion staticNpcOriginalRotation = Quaternion.identity;

    public DialogSetup dialogSetup;
    public GameObject gManager;

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
            SaveOriginalStates();
            StartConversation();
            foreach (GameObject isi in canvas)
            {
                isi.SetActive(false);
            }
            gManager.GetComponent<PauseManager>().enabled = false;
        }

        gameObject.layer = LayerMask.NameToLayer("Default");
    }

    private void StartConversation()
    {
        if (dialogSetup == null) return;

        dialogBehaviour.SetCurrentDialogOwner(this);
        dialogSetup.StartDialogForNPC();
        isInConversation = true;

        bobber.GetComponent<AudioSource>().enabled = false;
        animate.SetFloat("walk", 0);
        animate.SetBool("fall", false);

        StartCoroutine(SmoothTransitionToConversation());

        TogglePlayerMovement(false);
        ShowCursor(true);
    }

    private IEnumerator SmoothTransitionToConversation()
    {
        float elapsedTime = 0f;

        Quaternion npcStartRotation = transform.rotation;
        Quaternion cameraStartRotation = mainCamera.transform.rotation;
        Vector3 cameraStartPosition = mainCamera.transform.position;

        Vector3 directionToPlayer = playerTransform.position - transform.position;
        directionToPlayer.y = 0;
        Quaternion npcTargetRotation = Quaternion.LookRotation(directionToPlayer);

        Vector3 directionToNPC = transform.position - mainCamera.transform.position;
        directionToNPC.y = 0;
        directionToNPC.Normalize();
        Vector3 cameraTargetPosition = transform.position + (-directionToNPC * cameraDistance) + Vector3.up * cameraHeight;
        Quaternion cameraTargetRotation = Quaternion.LookRotation(transform.position - cameraTargetPosition);

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / transitionDuration;

            transform.rotation = Quaternion.Slerp(npcStartRotation, npcTargetRotation, t);
            mainCamera.transform.position = Vector3.Lerp(cameraStartPosition, cameraTargetPosition, t);
            mainCamera.transform.rotation = Quaternion.Slerp(cameraStartRotation, cameraTargetRotation, t);

            yield return null;
        }

        transform.rotation = npcTargetRotation;
        mainCamera.transform.position = cameraTargetPosition;
        mainCamera.transform.rotation = cameraTargetRotation;
    }

    private void SaveOriginalStates()
    {
        staticCameraOriginalPosition = mainCamera.transform.position;
        staticCameraOriginalRotation = mainCamera.transform.rotation;
        staticNpcOriginalRotation = transform.rotation;
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
        Cursor.lockState = show ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = show;
    }

    private void OnConversationEnded()
    {
        if (dialogBehaviour.CurrentDialogOwner != this) return;
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
        gManager.GetComponent<PauseManager>().enabled = true;
        isInConversation = false;

        ShowCursor(false);
    }

    private IEnumerator SmoothTransitionToOriginal()
    {
        float elapsedTime = 0f;

        Quaternion npcStartRotation = transform.rotation;
        Quaternion cameraStartRotation = mainCamera.transform.rotation;
        Vector3 cameraStartPosition = mainCamera.transform.position;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / transitionDuration;

            transform.rotation = Quaternion.Slerp(npcStartRotation, staticNpcOriginalRotation, t);
            mainCamera.transform.position = Vector3.Lerp(cameraStartPosition, staticCameraOriginalPosition, t);
            mainCamera.transform.rotation = Quaternion.Slerp(cameraStartRotation, staticCameraOriginalRotation, t);

            yield return null;
        }

        transform.rotation = staticNpcOriginalRotation;
        mainCamera.transform.position = staticCameraOriginalPosition;
        mainCamera.transform.rotation = staticCameraOriginalRotation;

        TogglePlayerMovement(true);
        gameObject.layer = LayerMask.NameToLayer("Interacted");

        // Reset original states
        staticCameraOriginalPosition = Vector3.zero;
        staticCameraOriginalRotation = Quaternion.identity;
        staticNpcOriginalRotation = Quaternion.identity;
    }
}
