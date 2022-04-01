using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    private CharacterController charController;
    public float speed = 5f;
    public float rotationSpeed = 5f;
    private Quaternion rotationQuat;
    private float t = 0;

    private void Start() {
        charController = GetComponent<CharacterController>();
    }

    private void Update() {
        float movx = Input.GetAxis("Horizontal");
        float movz = Input.GetAxis("Vertical");

        Vector3 movementVector = new Vector3(movx, 0, movz);

        if (movementVector.magnitude > 0.01) {
            //transform.forward = movementVector;
            charController.SimpleMove(movementVector * speed);

            rotationQuat = Quaternion.LookRotation(movementVector);
            t = 0f;
        }

        t += Time.deltaTime;
        transform.rotation = Quaternion.Slerp(transform.rotation, rotationQuat, speed * t);
    }
}
