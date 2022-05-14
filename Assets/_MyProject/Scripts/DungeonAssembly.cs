using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonAssembly : MonoBehaviour {
    public GameObject roomPrefab;
    public GameObject dungeon;
    public float startx = -40;
    public float startz = 40;
    private DungeonGeneration dg;

    private void Start() {
        dg = GetComponent<DungeonGeneration>();
        Assembly();
    }

    private void Assembly() {
        float zpos = startz;
        for (int i = 0; i < dg.max; i++) {
            float xpos = startx;
            for (int j = 0; j < dg.max; j++) {
                if (dg.matrix[i, j] > 0) {
                    GameObject newRoom = Instantiate(roomPrefab, dungeon.transform);
                    newRoom.transform.position = new Vector3(xpos, 0, zpos);
                    Room r = newRoom.GetComponent<Room>();
                    r.OpenDoors(i, j, dg.matrix, dg.max);
                }
                xpos += 20f;
            }
            zpos -= 20f;
        }
    }

}
