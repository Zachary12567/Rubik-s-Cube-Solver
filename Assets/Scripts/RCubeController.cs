using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CubePropertiesNS;

public class RCubeController : MonoBehaviour {



    public RCube myRCube;

    public GameObject cubePrefab;
    public GameObject stickerPrefab;

    private void Awake() {
        
    }

    private void Init() {
        const int len = 2;
        const int hei = 2;
        const int width = 2;

        myRCube = new RCube(len, hei, width);

        //myRCube.TEST__Print_GetSide_GONames();
        //myRCube.RotateUp();

    }
    private void RotateLeft() {
        
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.KeypadEnter)) {
            Init();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            if (Input.GetKey(KeyCode.LeftShift)) {
                myRCube.RotateUpInv();
            } else {
                myRCube.RotateUp();
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            if (Input.GetKey(KeyCode.LeftShift)) {
                myRCube.RotateDownInv();
            } else {
                myRCube.RotateDown();
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            if (Input.GetKey(KeyCode.LeftShift)) {
                myRCube.RotateLeftInv();
            } else {
                myRCube.RotateLeft();
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            if (Input.GetKey(KeyCode.LeftShift)) {
                myRCube.RotateRightInv();
            } else {
                myRCube.RotateRight();
            }
        }
    }
}