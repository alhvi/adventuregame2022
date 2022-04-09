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

    private void Start() {
        charController = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
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
        Quaternion appliedPitch = cameraLookout.transform.rotation * pitch;

        if (FormatAngle(appliedPitch.eulerAngles.x) > 45) {
            appliedPitch = Quaternion.Euler(45f, appliedPitch.eulerAngles.y, appliedPitch.eulerAngles.z);
        } else if (FormatAngle(appliedPitch.eulerAngles.x) < -5) {
            appliedPitch = Quaternion.Euler(-5f, appliedPitch.eulerAngles.y, appliedPitch.eulerAngles.z);
        }

        cameraLookout.transform.rotation = yaw * appliedPitch;

        float movx = Input.GetAxis("Horizontal");
        float movz = Input.GetAxis("Vertical");

        Vector3 movementVector = new Vector3(movx, 0, movz);
        movementVector.Normalize();
        Vector3 cameraDirection = new Vector3(cameraLookout.transform.forward.x, 0, cameraLookout.transform.forward.z);
        cameraDirection.Normalize();

        Vector3 difference = movementVector - cameraDirection;
        difference.Normalize();

        movementVector = difference * movementVector.magnitude;

        if (movementVector.magnitude > 0.01) {
            //transform.forward = movementVector;
            charController.SimpleMove(movementVector * speed * speedFactor);

            rotationQuat = Quaternion.LookRotation(movementVector);
            t = 0f;
        }

        t += Time.deltaTime;
        model.transform.rotation = Quaternion.Lerp(model.transform.rotation, rotationQuat, speed * t);

        anim.SetFloat("Speed", movementVector.magnitude * speedFactor);
    }

    private float FormatAngle(float angle) {
        angle = angle % 360;
        if (angle > 180) {
            angle = angle - 360;
        }

        return angle;
    }
}
