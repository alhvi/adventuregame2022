using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovOld : MonoBehaviour {
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

        //Ability to run
        if (Input.GetKey(KeyCode.LeftShift)) {
            speedFactor = 2f;
        } else {
            speedFactor = 1f;
        }

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

        //Obtain movement values
        float movx = Input.GetAxis("Horizontal");
        float movz = Input.GetAxis("Vertical");

        //Obtain vectors
        Vector3 movementVector = new Vector3(-movx, 0, movz);
        movementVector.Normalize();
        Vector3 cameraDirection = new Vector3(cameraLookout.transform.forward.x, 0, cameraLookout.transform.forward.z);
        cameraDirection.Normalize();

        //If movement is not zero
        if (movementVector.magnitude > 0.01) {

            //Obtain angles to combine camera movement and directional movement (wasd)
            float angleCam = Mathf.Atan2(cameraDirection.z, cameraDirection.x);
            float angleInput = Mathf.Atan2(movementVector.z, movementVector.x);
            float angle = angleInput - angleCam;

            //Obtain new vector with movement combined
            Vector3 movementDir = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle));

            //Move using new vectos
            charController.SimpleMove(movementDir * speed * speedFactor);

            //Obtain quaternion to new forward 
            rotationQuat = Quaternion.LookRotation(movementDir);
            t = 0f;
        }

        //Rotate using lerp
        if (speed * t < 1) {
            t += Time.deltaTime;
            model.transform.rotation = Quaternion.Lerp(model.transform.rotation, rotationQuat, speed * t);
        }

        //Set animation
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
