using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    private CharacterController charController;
    private Animator anim;
    public float speed = 5f;
    public float rotationSpeed = 5f;
    private Quaternion rotationQuat;
    private float t = 0;
    private float speedFactor = 1;

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

        float movx = Input.GetAxis("Horizontal");
        float movz = Input.GetAxis("Vertical");

        Vector3 movementVector = new Vector3(movx, 0, movz);

        if (movementVector.magnitude > 0.01) {
            //transform.forward = movementVector;
            charController.SimpleMove(movementVector * speed * speedFactor);

            rotationQuat = Quaternion.LookRotation(movementVector);
            t = 0f;
        }

        t += Time.deltaTime;
        transform.rotation = Quaternion.Slerp(transform.rotation, rotationQuat, speed * t);

        anim.SetFloat("Speed", movementVector.magnitude * speedFactor);
    }
}
