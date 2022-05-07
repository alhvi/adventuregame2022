using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    private static GameManager _instance;

    public static GameManager Instance => _instance;
    public GameObject player;

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(_instance.gameObject);
        } else {
            _instance = this;
            DontDestroyOnLoad(_instance.gameObject);
        }
    }
}
