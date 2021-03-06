using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    private CharacterController charController;
    private Animator anim;
    public float speed = 5f;
    public float rotationSpeed = 5f;
    private Quaternion rotationQuat = Quaternion.identity;
    private float t = 0;
    private float speedFactor = 1;
    public GameObject cameraLookout;
    private float cameraRotationSpeed = 300f;
    public GameObject model;
    public bool thirdPersonActive = true;
    private bool isJumping = false;
    private float jumpMultiplier = 10f;
    public AnimationCurve jumpingAnimationCurve;
    public float updateFrameCount = 0;
    public float coroutineFrameCount = 0;

    private void Start() {
        charController = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
    }

    private void Update() {
        PrepareForRunning();

        CameraMovement();

        Vector3 movementVector = PlayerMov();

        //Set animation
        anim.SetFloat("Speed", movementVector.magnitude * speedFactor);

        Jump();

    }

    private Vector3 PlayerMov() {
        //Obtain movement values
        float movx = Input.GetAxis("Horizontal");
        float movz = Input.GetAxis("Vertical");
        Vector3 movementVector = Vector3.zero;

        //Obtain vectors
        if (!thirdPersonActive) {
            model.transform.forward = cameraLookout.transform.forward;
        }

        movementVector = movx * cameraLookout.transform.right + movz * cameraLookout.transform.forward;

        movementVector = new Vector3(movementVector.x, 0, movementVector.z);
        movementVector.Normalize();

        charController.SimpleMove(movementVector * speed * speedFactor);

        //If movement is not zero
        if (movementVector.magnitude > 0.01) {

            rotationQuat = Quaternion.LookRotation(movementVector);
            t = 0f;
        }

        if (thirdPersonActive) {
            //Rotate using lerp
            if (speed * t < 1) {
                t += Time.deltaTime;
                model.transform.rotation = Quaternion.Lerp(model.transform.rotation, rotationQuat, speed * t);
            }
        }

        return movementVector;
    }

    private void CameraMovement() {
        //Read mouse components to move camera
        float mousex = Input.GetAxis("Mouse X");
        float mousey = Input.GetAxis("Mouse Y");

        //Calculate camera rotation
        float rotationAngleYaw = mousex * cameraRotationSpeed * Time.deltaTime;
        Quaternion yaw = Quaternion.AngleAxis(rotationAngleYaw, Vector3.up);

        float rotationAnglePitch = mousey * cameraRotationSpeed * Time.deltaTime;
        Quaternion pitch = Quaternion.AngleAxis(rotationAnglePitch, Vector3.right);

        //Calculate and correct pitch if camera exceeds bounds
        Quaternion appliedPitch = cameraLookout.transform.rotation * pitch;

        if (FormatAngle(appliedPitch.eulerAngles.x) > 45) {
            appliedPitch = Quaternion.Euler(45f, appliedPitch.eulerAngles.y, appliedPitch.eulerAngles.z);
        } else if (FormatAngle(appliedPitch.eulerAngles.x) < -5) {
            appliedPitch = Quaternion.Euler(-5f, appliedPitch.eulerAngles.y, appliedPitch.eulerAngles.z);
        }

        //Apply rotation to camera lookout
        cameraLookout.transform.rotation = yaw * appliedPitch;
    }

    private void PrepareForRunning() {
        //Ability to run
        if (Input.GetKey(KeyCode.LeftShift)) {
            speedFactor = 2f;
        } else {
            speedFactor = 1f;
        }
    }

    private float FormatAngle(float angle) {
        angle = angle % 360;
        if (angle > 180) {
            angle = angle - 360;
        }

        return angle;
    }

    private void Jump() {
        if (!isJumping && Input.GetKeyDown(KeyCode.Space)) {
            isJumping = true;
            StartCoroutine(JumpingCoroutine());
        }
    }

    public IEnumerator JumpingCoroutine() {
        float timeJumping = 0;

        do {
            float jumpForce = jumpingAnimationCurve.Evaluate(timeJumping);
            charController.Move(Vector3.up * jumpForce * jumpMultiplier * Time.deltaTime);
            timeJumping += Time.deltaTime;
            yield return null;

        } while (!charController.isGrounded);

        isJumping = false;

    }

}
