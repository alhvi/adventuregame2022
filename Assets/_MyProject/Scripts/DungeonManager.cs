using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour {
    private static DungeonManager _instance;

    public static DungeonManager Instance => _instance;
    public GameObject player;
    public GameObject treasurePrefab;

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(_instance.gameObject);
        } else {
            _instance = this;
            DontDestroyOnLoad(_instance.gameObject);
        }
    }
}
