using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour {
    private Animator anim;

    private void Start() {
        anim = GetComponentInChildren<Animator>();
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            anim.SetTrigger("Attack1");
        }

        if (Input.GetMouseButtonDown(1)) {
            anim.SetTrigger("Attack2");
        }
    }
}
