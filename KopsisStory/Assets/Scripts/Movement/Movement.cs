using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Movement : MonoBehaviour
{
    [SerializeField] Transform cameraHolder;
    [SerializeField] Animator animator;

    [Header("Steering")]
    [SerializeField] ControllerMode controllerMode;
    public ControllerMode readControllerMode {get {return controllerMode;}}

    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] float crouchSpeed;

    [Range(1, 1.9f)]
    [SerializeField] float crouchHeight;
    [SerializeField] float gravity;
    [SerializeField] float jumpHeight;

    [Header("Camera Looking")]
    [SerializeField] float cameraSensitivty;

    [Header("Mobile")]
    [SerializeField] Joystick joystickMobile;

    [Range(.2f, 1)]
    [SerializeField] float screenTouchForLooking;

    [Header("Sound Effect")]
    public AudioClip audioGrounded;
    public AudioClip audioWalk;
 
    bool run, crouch, buttonRun, buttonCrouch, buttonJump;
    float slope, currentSpeed, originalStepOffset, _directionY, velocityY;

    CharacterController charController;
    Vector2 move, look;
    Vector3 characterMove;

    void Start () {
        charController = GetComponent<CharacterController>();
        cameraHolder.localPosition = new Vector3(0, 2.5f, 0);
        originalStepOffset = charController.stepOffset;

        if(controllerMode == ControllerMode.PC) {

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

        } else if(controllerMode == ControllerMode.Mobile) {

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

        }
    }

    float axisX, axisY, mouseX, mouseY;

    void Update () {
        switch(controllerMode) {
            case ControllerMode.Mobile :

                velocityY = joystickMobile.Vertical * walkSpeed;

                axisX = joystickMobile.Horizontal * currentSpeed;
                axisY = joystickMobile.Vertical * currentSpeed;

                for(int i = 0; i < Input.touchCount; i++) {
                        Touch touch = Input.GetTouch(i);
                        if(InputManager.instance.verifiedTouch(touch, Screen.width * screenTouchForLooking)) {
                            switch(touch.phase) {
                                case TouchPhase.Moved :
                                    mouseX += (touch.deltaPosition.x * Time.deltaTime) * cameraSensitivty;
                                    mouseY = Mathf.Clamp(look.y -= (touch.deltaPosition.y * Time.deltaTime) * cameraSensitivty, -45, 45);
                                break;
                            }
                        }
                } 
            break;
            case ControllerMode.PC :

                velocityY = Input.GetAxis("Vertical") * walkSpeed;

                axisX = (Input.GetAxis("Horizontal") * currentSpeed);
                axisY = (Input.GetAxis("Vertical")  * currentSpeed);

                mouseX = mouseX += Input.GetAxis("Mouse X") * cameraSensitivty;
                mouseY = Mathf.Clamp(mouseY -= Input.GetAxis("Mouse Y") * cameraSensitivty, -45, 45);

                buttonRun = Input.GetKey(KeyCode.LeftShift);
                buttonCrouch = Input.GetKey(KeyCode.C);
                buttonJump = Input.GetKeyDown(KeyCode.Space);
            break;
        }

        move = new Vector2(axisX, axisY);
        look = new Vector2(mouseX, mouseY);
    
        run = buttonRun && velocityY > (walkSpeed * .6f);
        crouch = charController.isGrounded && buttonCrouch && !run;
            
        if(run) {
            currentSpeed = Mathf.Lerp(currentSpeed, runSpeed, 7 * Time.deltaTime);
        } else if(crouch || crouchAboveDetect()) 
            currentSpeed = crouchSpeed;
        else
            currentSpeed = Mathf.Lerp(currentSpeed, walkSpeed, 3 * Time.deltaTime);

        Animation();
    }

    void LateUpdate () {
        move = Vector3.ClampMagnitude(move, currentSpeed);
        characterMove = new Vector3(move.x, 0, move.y);

        characterMove = transform.TransformDirection(characterMove);

        if(charController.isGrounded) {
            _directionY = -.5f;
            
            if(buttonJump && !crouch)
                _directionY = jumpHeight;

            charController.stepOffset = originalStepOffset;
        } else {
            charController.stepOffset = 0;  

            if(buttonJump && controllerMode == ControllerMode.Mobile)
                buttonJump = false;
        }

        if(_directionY > -25)
            _directionY -= Time.deltaTime * gravity;

        characterMove.y = _directionY;

        characterMove.y = Slope(characterMove);
        charController.Move(characterMove * Time.deltaTime);
        cameraHolder.transform.localRotation = Quaternion.Euler(look.y, 0, 0);
        transform.localRotation = Quaternion.Euler(0, look.x, 0);

        float cameraHolderY = (crouch || crouchAboveDetect()) ? crouchHeight + .5f : 2 + .5f;
        float currentHeight = (crouch || crouchAboveDetect()) ? crouchHeight : 2;
        charController.height = Mathf.MoveTowards(charController.height, currentHeight, Time.deltaTime * 6);
        charController.center = new Vector3(0, (charController.height * .5f), 0);
        cameraHolder.transform.localPosition = new Vector3(0,Mathf.MoveTowards(cameraHolder.transform.localPosition.y, cameraHolderY, Time.deltaTime * 6),0);
    }

    void Animation () {
        if(animator == null) return;

        animator.SetFloat("walk", new Vector3(charController.velocity.x, 0, charController.velocity.z).magnitude);
        animator.SetBool("fall", !charController.isGrounded);
    }

    public void MobileButton (string act) {
        if(act == "run") {
            buttonRun = !buttonRun;
            buttonCrouch = false;
        } else if(act == "crouch") {
            buttonCrouch = !buttonCrouch;
        } else if(act == "jump") {
            buttonJump = true;
        }
    }

    float front, back, right, left, above;
    bool frontAbove, backAbove, rightAbove, leftAbove;

    bool crouchAboveDetect () {
        front = charController.bounds.center.z + charController.bounds.extents.z;
        back = charController.bounds.center.z - charController.bounds.extents.z;
        right = charController.bounds.center.x + charController.bounds.extents.x;
        left = charController.bounds.center.x - charController.bounds.extents.x;
        above = charController.bounds.center.y + charController.bounds.extents.y;

        RaycastHit hit;

        frontAbove = Physics.Raycast(new Vector3(transform.position.x, above, front), transform.up, out hit, 1.5f, ~3, QueryTriggerInteraction.Ignore);
        backAbove = Physics.Raycast(new Vector3(transform.position.x, above, back), transform.up, out hit, 1.5f, ~3, QueryTriggerInteraction.Ignore);
        rightAbove = Physics.Raycast(new Vector3(right, above, transform.position.z), transform.up, out hit, 1.5f, ~3, QueryTriggerInteraction.Ignore);
        leftAbove = Physics.Raycast(new Vector3(left, above, transform.position.z), transform.up, out hit, 1.5f, ~3, QueryTriggerInteraction.Ignore);

        return (frontAbove || backAbove || rightAbove || leftAbove);
    }

    void OnDrawGizmos () {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(new Vector3(transform.position.x, above, front), transform.up * 1.5f);
        Gizmos.DrawRay(new Vector3(transform.position.x, above, back), transform.up * 1.5f);
        Gizmos.DrawRay(new Vector3(right, above, transform.position.z), transform.up * 1.5f);
        Gizmos.DrawRay(new Vector3(left, above, transform.position.z), transform.up * 1.5f);
    }

    float Slope(Vector3 velocity) { 
        var ray = new Ray(transform.position, Vector3.down);
        if(Physics.Raycast(ray, out RaycastHit hitInfo, 0.2f)) {
            var slopeRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
            var adjustedVelocity = slopeRotation * velocity;

            if(adjustedVelocity.y < 0)
                return adjustedVelocity.y;
        }

        return velocity.y; 
    }
    
}
