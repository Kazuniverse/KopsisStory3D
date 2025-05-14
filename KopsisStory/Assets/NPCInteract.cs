using cherrydev;
using UnityEngine;

[RequireComponent(typeof(DialogSetup))]
public class NPCInteract : OnRaycast
{
    [Header("Interaction Settings")]
    public float interactionDistance = 3f;
    public float rotationSpeed = 5f;
    public float returnRotationSpeed = 2f;
    public float cameraHeightOffset = 1.5f;

    [Header("References")]
    public Transform playerTransform;
    public Camera playerCamera;

    private DialogSetup dialogSetup;
    private DialogBehaviour dialogBehaviour;
    private Quaternion originalNPCRotation;
    private Quaternion originalCameraRotation;
    private Vector3 originalCameraPosition;
    private bool isInConversation = false;
    private bool isReturningToOriginalRotation = false;

    private void Awake()
    {
        dialogSetup = GetComponent<DialogSetup>();
        dialogBehaviour = dialogSetup.dialogBehaviour;
        originalNPCRotation = transform.rotation;
        
        if (playerTransform == null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
        
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }
    }

    private void OnEnable()
    {
        // Subscribe to the dialog ended event
        if (dialogBehaviour != null)
        {
            dialogBehaviour.OnDialogEnded += HandleDialogEnded;
        }
    }

    private void OnDisable()
    {
        // Unsubscribe to prevent memory leaks
        if (dialogBehaviour != null)
        {
            dialogBehaviour.OnDialogEnded -= HandleDialogEnded;
        }
    }

    private void Update()
    {
        if (isInConversation)
        {
            RotateNPCToFacePlayer();
            RotateCameraToFaceNPC();
        }
        else if (isReturningToOriginalRotation)
        {
            ReturnNPCToOriginalRotation();
        }
    }

    public override void OnInteract()
    {
        if (!isInConversation)
        {
            StartConversation();
            gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }

    private void StartConversation()
    {
        originalCameraRotation = playerCamera.transform.rotation;
        originalCameraPosition = playerCamera.transform.position;

        isInConversation = true;
        dialogSetup.StartDialogForNPC();
    }

    // New method to handle dialog ended event
    private void HandleDialogEnded()
    {
        EndConversation();
    }

    private void EndConversation()
    {
        if (!isInConversation) return;

        Debug.Log("Conversation ended"); // Debug log to verify execution

        isInConversation = false;
        isReturningToOriginalRotation = true;

        playerCamera.transform.rotation = originalCameraRotation;
        playerCamera.transform.position = originalCameraPosition;
        gameObject.layer = LayerMask.NameToLayer("Interacted");
    }

    private void RotateNPCToFacePlayer()
    {
        Vector3 directionToPlayer = playerTransform.position - transform.position;
        directionToPlayer.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void ReturnNPCToOriginalRotation()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, originalNPCRotation, returnRotationSpeed * Time.deltaTime);

        if (Quaternion.Angle(transform.rotation, originalNPCRotation) < 1f)
        {
            transform.rotation = originalNPCRotation;
            isReturningToOriginalRotation = false;
        }
    }

    private void RotateCameraToFaceNPC()
    {
        Vector3 npcHeadPosition = transform.position + Vector3.up * cameraHeightOffset;
        Vector3 directionToNPC = npcHeadPosition - playerCamera.transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(directionToNPC);
        
        playerCamera.transform.rotation = Quaternion.Slerp(
            playerCamera.transform.rotation, 
            targetRotation, 
            rotationSpeed * Time.deltaTime
        );
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionDistance);
    }
}