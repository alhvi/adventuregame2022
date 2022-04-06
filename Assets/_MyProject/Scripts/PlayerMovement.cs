using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    private CharacterController charController;
    private Animator anim;
    public float speed = 10f;
    public float rotationSpeed = 10f;
    private Quaternion rotationQuat;
    private float t = 0;
    private float speedFactor = 1;
    private float cameraRotationSpeed = 300f;
    public GameObject cameraLookout;
    public GameObject model;

    private void Awake() {
        Application.targetFrameRate = 60;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Start() {
        charController = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        cameraLookout.transform.rotation = Quaternion.identity;
    }

    private void Update() {

        if (Input.GetKey(KeyCode.LeftShift)) {
            speedFactor = 2f;
        } else {
            speedFactor = 1f;
        }

        float mousex = Input.GetAxis("Mouse X");
        float mousey = Input.GetAxis("Mouse Y");

        Quaternion yaw = Quaternion.AngleAxis(mousex * cameraRotationSpeed * Time.deltaTime, Vector3.up);
        Quaternion pitch = Quaternion.AngleAxis(mousey * cameraRotationSpeed * Time.deltaTime, Vector3.right);
        Quaternion resultingPitch = cameraLookout.transform.rotation * pitch;

        if (CustomAngle(resultingPitch.eulerAngles.x) > 45f) {
            resultingPitch = Quaternion.Euler(45f, resultingPitch.eulerAngles.y, resultingPitch.eulerAngles.z);
        } else if (CustomAngle(resultingPitch.eulerAngles.x) < -5f) {
            resultingPitch = Quaternion.Euler(-5f, resultingPitch.eulerAngles.y, resultingPitch.eulerAngles.z);
        }

        cameraLookout.transform.rotation = yaw * resultingPitch;

        float movx = Input.GetAxis("Horizontal");
        float movz = Input.GetAxis("Vertical");

        Vector3 movementVector = new Vector3(-movx, 0, movz);
        movementVector.Normalize();
        Vector3 cameraDirection = new Vector3(cameraLookout.transform.forward.x, 0, cameraLookout.transform.forward.z);
        cameraDirection.Normalize();

        if (movementVector.magnitude > 0.01) {

            float angleCam = Mathf.Atan2(cameraDirection.z, cameraDirection.x);
            float angleInput = Mathf.Atan2(movementVector.z, movementVector.x);
            float angle = angleInput - angleCam;

            Vector3 movementDir = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle));

            charController.SimpleMove(movementDir * speed * speedFactor);

            rotationQuat = Quaternion.LookRotation(movementDir);
            t = 0f;
        }

        if (t < 1) {
            t += Time.deltaTime * rotationSpeed;
            model.transform.rotation = Quaternion.Slerp(model.transform.rotation, rotationQuat, t);
        }

        anim.SetFloat("Speed", movementVector.magnitude * speedFactor);
    }

    public float CustomAngle(float angle) {
        angle = angle % 360;
        if (angle > 180) {
            angle = -360 + angle;
        }
        return angle;
    }
}
