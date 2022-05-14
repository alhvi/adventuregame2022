using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {
    public GameObject doorTop;
    public GameObject doorBottom;
    public GameObject doorRight;
    public GameObject doorLeft;

    public void OpenDoors(int i, int j, int[,] matrix, int max) {
        //TopDoor
        if (i != 0) {
            if (matrix[i - 1, j] > 0) {
                doorTop.SetActive(false);
            }
        }
        //BottomDoor
        if (i != max - 1) {
            if (matrix[i + 1, j] > 0) {
                doorBottom.SetActive(false);
            }
        }

        //LeftDoor 
        if (j != 0) {
            if (matrix[i, j - 1] > 0) {
                doorLeft.SetActive(false);
            }
        }

        //RightDoor
        if (j != max - 1) {
            if (matrix[i, j + 1] > 0) {
                doorRight.SetActive(false);
            }
        }

        CreateElements(i, j, matrix);

    }

    public void CreateElements(int i, int j, int[,] matrix) {
        if (matrix[i, j] == 2) {
            DungeonManager.Instance.player.transform.position = transform.position;
        } else if (matrix[i, j] == 3) {
            GameObject chest = Instantiate(DungeonManager.Instance.treasurePrefab);
            chest.transform.position = transform.position;
        }

    }
}
