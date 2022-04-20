using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCameras : MonoBehaviour {
    public GameObject camera1st;
    public GameObject camera3rd;
    public GameObject model;
    private PlayerMovement playerMovement;
    private bool thirdPersonActive = true;

    private void Start() {
        playerMovement = GetComponent<PlayerMovement>();
        ActivateThirdPerson();
    }

    public void ActivateFristPerson() {
        camera1st.SetActive(true);
        camera3rd.SetActive(false);
        model.SetActive(false);
        thirdPersonActive = false;
        playerMovement.thirdPersonActive = false;
    }

    public void ActivateThirdPerson() {
        camera3rd.SetActive(true);
        camera1st.SetActive(false);
        model.SetActive(true);
        thirdPersonActive = true;
        playerMovement.thirdPersonActive = true;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.F5)) {
            if (thirdPersonActive) {
                ActivateFristPerson();
            } else {
                ActivateThirdPerson();
            }
        }
    }

}
