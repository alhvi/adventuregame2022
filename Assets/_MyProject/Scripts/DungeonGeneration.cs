using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGeneration : MonoBehaviour {
    public int max = 6;
    public int[,] matrix;
    private int roomCount = 0;

    private void Awake() {

        matrix = new int[max, max];
        //Initialize in zero
        for (int i = 0; i < max; i++) {
            for (int j = 0; j < max; j++) {
                matrix[i, j] = 0;
            }
        }

        int si = Random.Range(2, 3);
        int sj = Random.Range(2, 3);

        VisitRooms(si, sj);

        matrix[si, sj] = 2;

        PrintMatrix();
    }

    private void VisitRooms(int i, int j) {
        //Set node as visited
        matrix[i, j] = 1;

        //Return condition
        if (IsCorner(i, j)) {
            matrix[i, j] = 3;
            return;
        }

        //Determining next node to visit
        int nexti = i;
        int nextj = j;

        float rand = Random.Range(0f, 1f);
        if (rand >= 0 && rand < 0.3) {
            nextj++;
        } else if (rand >= 0.3 && rand < 0.6) {
            nextj--;
        } else if (rand >= 0.6 && rand < 0.8) {
            nexti++;
        } else if (rand >= 0.8 && rand <= 1f) {
            nexti--;
        }

        //Validating Boundaries
        if (nexti == max) {
            nexti = max - 1;
        }

        if (nexti < 0) {
            nexti = 0;
        }

        if (nextj == max) {
            nextj = max - 1;
        }

        if (nextj < 0) {
            nextj = 0;
        }

        //Recursive Visit
        VisitRooms(nexti, nextj);
    }

    public int CountRooms() {
        int count = 0;
        for (int i = 0; i < max; i++) {
            for (int j = 0; j < max; j++) {
                if (matrix[i, j] != 0) {
                    count++;
                }
            }
        }
        return count;
    }

    public bool IsCorner(int i, int j) {
        bool corner = false;
        if (i == 0 && j == 0) {
            corner = true;
        }
        if (i == 0 && j == max - 1) {
            corner = true;
        }
        if (i == max - 1 && j == 0) {
            corner = true;
        }
        if (i == max - 1 && j == max - 1) {
            corner = true;
        }

        return corner;
    }

    private void PrintMatrix() {
        string m = "";
        for (int i = 0; i < max; i++) {
            string a = "";
            for (int j = 0; j < max; j++) {
                a = a + matrix[i, j].ToString();
            }
            m += a + "\n";
        }
        Debug.Log(m);
    }
}
