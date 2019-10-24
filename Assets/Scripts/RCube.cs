using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CubePropertiesNS;

namespace CubePropertiesNS {
    public enum _Color { white, yellow, red, orange, blue, green }
    public enum Side { top, front, right, back, left, bottom }
}

public class RCube {

    public struct Sticker {
        public GameObject parentCube;
        public GameObject stickerObj;
        public _Color color;

        public Sticker(GameObject parentObj, GameObject sObj, _Color c) {
            parentCube = parentObj;
            stickerObj = sObj;
            color = c;
        }
    }
    int len, hei, width;

    public GameObject center;
    public GameObject[] cubes;
    public Sticker[] stickers;
    /*      mapping:    
     *                  0 1
     *                  2 3     for each side & top, as well as bottom when rotated about "x" axis      
     *      red top, white front            
     */
    public GameObject[] anchors;

    public RCube(int len, int hei, int width) {
        this.len = len;
        this.hei = hei;
        this.width = width;

        cubes = new GameObject[len * hei * width];
        stickers = new Sticker[6 * len * width];
        anchors = new GameObject[6];


        CreateAnchors();
        FindStickers();
        SetCubes();

        center = cubes[0].transform.parent.gameObject;
    }

    private void CreateAnchors() {
        for (int i=0; i<6; i++) {
            if (i == (int)Side.top) {
                GameObject topAnchor = new GameObject("top anchor");
                topAnchor.transform.position = new Vector3(-len / 2, hei, width / 2);
                anchors[i] = topAnchor;
            } else if (i == (int)Side.front) {
                GameObject frontAnchor = new GameObject("front anchor");
                frontAnchor.transform.position = new Vector3(-len / 2, hei / 2, 0);
                anchors[i] = frontAnchor;
            } else if (i == (int)Side.right) {
                GameObject rightAnchor = new GameObject("right anchor");
                rightAnchor.transform.position = new Vector3(0, hei / 2, width / 2);
                anchors[i] = rightAnchor;
            } else if (i == (int)Side.back) {
                GameObject backAnchor = new GameObject("back anchor");
                backAnchor.transform.position = new Vector3(-len / 2, hei / 2, width);
                anchors[i] = backAnchor;
            } else if (i == (int)Side.left) {
                GameObject leftAnchor = new GameObject("left anchor");
                leftAnchor.transform.position = new Vector3(-len, hei / 2, width / 2);
                anchors[i] = leftAnchor;
            } else if (i == (int)Side.bottom) {
                GameObject bottomAnchor = new GameObject("bottom anchor");
                bottomAnchor.transform.position = new Vector3(-len / 2, 0, width / 2);
                anchors[i] = bottomAnchor;
            }
        }
    }

    public GameObject[] GetSide(Side side) {
        GameObject[] resultant = new GameObject[len * hei];

        // odds
        if (side == Side.left) {
            for (int i = 0, j = 0; j < (len*hei*width); i++, j+=len) {
                resultant[i] = this.cubes[j];
            }
        }
        // evens
        else if (side == Side.right) {
            for (int i = 0, j = 1; j < (len*hei*width); i++, j += len) {
                resultant[i] = this.cubes[j];
            }
        }
        // last few per row ( 3,4 & 7,8 )
        else if (side == Side.front) {
            for (int i = 0, j = (len*len)-len; j < (len*hei*width); i++, j++) {
                if (j > (len*len)-1 && j < 2*(len*len)-len) {
                    j = 2 * (len * len) - len;

                } /* 3x3
                else if (j > 2*(len*len)-1 && j < 3*(len*len)-len) {
                    j = 3 * (len * len) - len;
                } */
                resultant[i] = this.cubes[j];
            }
        }
        // first few per row (1,2 & 5,6)
        else if (side == Side.back) {
            for (int i = 0, j = 0; j < ((len-1)*(len*width))+len; i++, j++) {
                if (j > len-1 && j < (len*len)) {
                    j = len * len;
                }
                /* 3x3
                else if (j > (len*len)+len-1 && j < 2*(len*len)+len) {
                    j = 2*(len*len);
                */
                resultant[i] = this.cubes[j];
            }
        }
        // last len*len amount
        else if (side == Side.top) {
            for (int i = 0, j = (len-1)*(len*len); j < (len*hei*width); i++, j++) {
                resultant[i] = this.cubes[j];
            }
        }
        // first len*len amount
        else if (side == Side.bottom) {
            for (int i = 0; i < (len*len); i++) {
                resultant[i] = this.cubes[i];
            }
        }
        else {
            throw new System.ArgumentException("not a valid side");
        }

        return resultant;
    }
    // 2x2 exclusive
    private void FindStickers() {
        RaycastHit hit;

        if (Physics.Raycast(new Vector3(-1.5f, 2.5f, 1.5f), Vector3.down, out hit)) {
            stickers[0].stickerObj = hit.transform.gameObject;
            stickers[0].color = _Color.red;
        }
        if (Physics.Raycast(new Vector3(-.5f, 2.5f, 1.5f), Vector3.down, out hit)) {
            stickers[1].stickerObj = hit.transform.gameObject;
            stickers[1].color = _Color.red;
        }
        if (Physics.Raycast(new Vector3(-1.5f, 2.5f, .5f), Vector3.down, out hit)) {
            stickers[2].stickerObj = hit.transform.gameObject;
            stickers[2].color = _Color.red;
        }
        if (Physics.Raycast(new Vector3(-.5f, 2.5f, .5f), Vector3.down, out hit)) {
            stickers[3].stickerObj = hit.transform.gameObject;
            stickers[3].color = _Color.red;
        }
        if (Physics.Raycast(new Vector3(-1.5f, 1.5f, -.5f), Vector3.forward, out hit)) {
            stickers[4].stickerObj = hit.transform.gameObject;
            stickers[4].color = _Color.white;
        }
        if (Physics.Raycast(new Vector3(-.5f, 1.5f, -.5f), Vector3.forward, out hit)) {
            stickers[5].stickerObj = hit.transform.gameObject;
            stickers[5].color = _Color.white;
        }
        if (Physics.Raycast(new Vector3(-1.5f, .5f, -.5f), Vector3.forward, out hit)) {
            stickers[6].stickerObj = hit.transform.gameObject;
            stickers[6].color = _Color.white;
        }
        if (Physics.Raycast(new Vector3(-.5f, .5f, -.5f), Vector3.forward, out hit)) {
            stickers[7].stickerObj = hit.transform.gameObject;
            stickers[7].color = _Color.white;
        }
        if (Physics.Raycast(new Vector3(.5f, 1.5f, .5f), Vector3.left, out hit)) {
            stickers[8].stickerObj = hit.transform.gameObject;
            stickers[8].color = _Color.green;
        }
        if (Physics.Raycast(new Vector3(.5f, 1.5f, 1.5f), Vector3.left, out hit)) {
            stickers[9].stickerObj = hit.transform.gameObject;
            stickers[9].color = _Color.green;;
        }
        if (Physics.Raycast(new Vector3(.5f, .5f, .5f), Vector3.left, out hit)) {
            stickers[10].stickerObj = hit.transform.gameObject;
            stickers[10].color = _Color.green;
        }
        if (Physics.Raycast(new Vector3(.5f, .5f, 1.5f), Vector3.left, out hit)) {
            stickers[11].stickerObj = hit.transform.gameObject;
            stickers[11].color = _Color.green;
        }
        if (Physics.Raycast(new Vector3(-.5f, 1.5f, 2.5f), Vector3.back, out hit)) {
            stickers[12].stickerObj = hit.transform.gameObject;
            stickers[12].color = _Color.yellow;
        }
        if (Physics.Raycast(new Vector3(-1.5f, 1.5f, 2.5f), Vector3.back, out hit)) {
            stickers[13].stickerObj = hit.transform.gameObject;
            stickers[13].color = _Color.yellow;
        }
        if (Physics.Raycast(new Vector3(-.5f, .5f, 2.5f), Vector3.back, out hit)) {
            stickers[14].stickerObj = hit.transform.gameObject;
            stickers[14].color = _Color.yellow;
        }
        if (Physics.Raycast(new Vector3(-1.5f, .5f, 2.5f), Vector3.back, out hit)) {
            stickers[15].stickerObj = hit.transform.gameObject;
            stickers[15].color = _Color.yellow;
        }
        if (Physics.Raycast(new Vector3(-2.5f, 1.5f, 1.5f), Vector3.right, out hit)) {
            stickers[16].stickerObj = hit.transform.gameObject;
            stickers[16].color = _Color.blue;
        }
        if (Physics.Raycast(new Vector3(-2.5f, 1.5f, .5f), Vector3.right, out hit)) {
            stickers[17].stickerObj = hit.transform.gameObject;
            stickers[17].color = _Color.blue;
        }
        if (Physics.Raycast(new Vector3(-2.5f, .5f, 1.5f), Vector3.right, out hit)) {
            stickers[18].stickerObj = hit.transform.gameObject;
            stickers[18].color = _Color.blue;
        }
        if (Physics.Raycast(new Vector3(-2.5f, .5f, .5f), Vector3.right, out hit)) {
            stickers[19].stickerObj = hit.transform.gameObject;
            stickers[19].color = _Color.blue;
        }
        // bottom 0th is directly underneath top 0th
        if (Physics.Raycast(new Vector3(-1.5f, -.5f, 1.5f), Vector3.up, out hit)) {
            stickers[20].stickerObj = hit.transform.gameObject;
            stickers[20].color = _Color.orange;
        }
        if (Physics.Raycast(new Vector3(-.5f, -.5f, 1.5f), Vector3.up, out hit)) {
            stickers[21].stickerObj = hit.transform.gameObject;
            stickers[21].color = _Color.orange;
        }
        if (Physics.Raycast(new Vector3(-1.5f, -.5f, .5f), Vector3.up, out hit)) {
            stickers[22].stickerObj = hit.transform.gameObject;
            stickers[22].color = _Color.orange;
        }
        if (Physics.Raycast(new Vector3(-.5f, -.5f, .5f), Vector3.up, out hit)) {
            stickers[23].stickerObj = hit.transform.gameObject;
            stickers[23].color = _Color.orange;
        }
    }

    private void SetCubes() {
        cubes[0] = stickers[15].stickerObj.transform.parent.gameObject;
        cubes[1] = stickers[11].stickerObj.transform.parent.gameObject;
        cubes[2] = stickers[6].stickerObj.transform.parent.gameObject;
        cubes[3] = stickers[7].stickerObj.transform.parent.gameObject;
        cubes[4] = stickers[0].stickerObj.transform.parent.gameObject;
        cubes[5] = stickers[1].stickerObj.transform.parent.gameObject;
        cubes[6] = stickers[2].stickerObj.transform.parent.gameObject;
        cubes[7] = stickers[3].stickerObj.transform.parent.gameObject;
        /*
        for (int i = 0; i < (len*len*len); i+=len) {
        }
        */
    }

    // belogns in some controller class
    public void RotateUp() {
        // internally
        // 0->1, 1->2, 2->3, 3->0, 4->16, 5->17, 8->4, 9->5, 12->8, 13->9, 16->12, 17->13
        Sticker tmp;

        tmp = stickers[0];
        stickers[0] = stickers[2];
        stickers[2] = stickers[3];
        stickers[3] = stickers[1];
        stickers[1] = tmp;

        tmp = stickers[4];
        stickers[4] = stickers[8];
        stickers[8] = stickers[12];
        stickers[12] = stickers[16];
        stickers[16] = tmp;

        tmp = stickers[5];
        stickers[5] = stickers[9];
        stickers[9] = stickers[13];
        stickers[13] = stickers[17];
        stickers[17] = tmp;

        // physically
        GameObject[] cubeObjs = new GameObject[4];
        for (int i=0; i<4; i++) {
            cubeObjs = GetSide(Side.top);
        }
        foreach (GameObject go in cubeObjs) {
            go.transform.parent = anchors[(int)Side.top].transform;
        }

        anchors[(int)Side.top].transform.Rotate(new Vector3(0, 90, 0));
        

        foreach (GameObject go in cubeObjs) {
            go.transform.parent = center.transform;
        }

    }
    public void RotateUpInv() {
        RotateUp();
        RotateUp();
        RotateUp();
    }
    public void RotateDown() {
        RotateUpInv();
    }
    public void RotateDownInv() {
        RotateUp();
    }

    public void RotateLeft() {
        // internally
        // 0->15, 2->13, 4->0, 6->2, 13->20, 15->22, 16->18, 17->16, 18->19, 19->17, 20->6, 22->4
        Sticker tmp;

        tmp = stickers[0];
        stickers[0] = stickers[15];
        stickers[15] = stickers[22];
        stickers[22] = stickers[4];
        stickers[4] = tmp;

        tmp = stickers[2];
        stickers[2] = stickers[13];
        stickers[13] = stickers[20];
        stickers[20] = stickers[6];
        stickers[6] = tmp;

        tmp = stickers[16];
        stickers[16] = stickers[18];
        stickers[18] = stickers[19];
        stickers[19] = stickers[17];
        stickers[17] = tmp;

        // physically
        GameObject[] cubeObjs = new GameObject[4];
        for (int i=0; i<4; i++) {
            cubeObjs = GetSide(Side.left);
        }
        foreach (GameObject go in cubeObjs) {
            go.transform.parent = anchors[(int)Side.left].transform;
        }

        anchors[(int)Side.left].transform.Rotate(new Vector3(90, 0, 0));
        

        foreach (GameObject go in cubeObjs) {
            go.transform.parent = center.transform;
        }
    }
    public void RotateLeftInv() {
        RotateLeft();
        RotateLeft();
        RotateLeft();
    }
    public void RotateRight() {
        RotateLeft();
        // Change camera or implement actual rotation
    }
    public void RotateRightInv() {
        RotateLeftInv();
    }

    public void RotateFront() {
        // internally
        // 2->19, 3->17, 4->6, 5->4, 6->7, 7->5, 8->2, 10->3, 17->22, 19->23, 22->10, 23->8
        Sticker tmp;

        tmp = stickers[2];
        stickers[2] = stickers[19];
        stickers[19] = stickers[23];
        stickers[23] = stickers[8];
        stickers[8] = tmp;

        tmp = stickers[3];
        stickers[3] = stickers[17];
        stickers[17] = stickers[22];
        stickers[22] = stickers[10];
        stickers[10] = tmp;

        tmp = stickers[4];
        stickers[4] = stickers[6];
        stickers[6] = stickers[7];
        stickers[7] = stickers[5];
        stickers[5] = tmp;

        // physically
        GameObject[] cubeObjs = new GameObject[4];
        for (int i = 0; i < 4; i++) {
            cubeObjs = GetSide(Side.front);
        }
        foreach (GameObject go in cubeObjs) {
            go.transform.parent = anchors[(int)Side.front].transform;
        }

        anchors[(int)Side.front].transform.Rotate(new Vector3(90, 0, 0));


        foreach (GameObject go in cubeObjs) {
            go.transform.parent = center.transform;
        }
    }
    public void RotateFrontInv() {
        RotateFront();
        RotateFront();
        RotateFront();
    }
    public void RotateBack() {
        RotateFrontInv();
    }
    public void RotateBackInv() {
        RotateFront();
    }
    /*
    IEnumerable RotUp() {
        float rotSpeed = .7f;

        float elapsedTime = 0;
        Vector3 startingRotation = anchors[(int)Side.top].transform.eulerAngles;
        Vector3 endingRotation = new Vector3(anchors[(int)Side.top].transform.eulerAngles.x, anchors[(int)Side.top].transform.eulerAngles.y, anchors[(int)Side.top].transform.eulerAngles.z);
        while (elapsedTime < rotSpeed) {
            anchors[(int)Side.top].transform.eulerAngles = Vector3.Lerp(startingRotation, endingRotation, elapsedTime / rotSpeed);
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
    */

    public void TEST__Print_GetSide_GONames() {

        Debug.Log("top side: ");
        GameObject[] topSideGOs = GetSide(Side.top);
        foreach (GameObject go in topSideGOs) {
            Debug.Log(go.name);
        }

        Debug.Log("\n bottom side: ");
        GameObject[] bottomSideGOs = GetSide(Side.bottom);
        foreach (GameObject go in bottomSideGOs) {
            Debug.Log(go.name);
        }

        Debug.Log("\n left side: ");
        GameObject[] leftSideGOs = GetSide(Side.left);
        foreach (GameObject go in leftSideGOs) {
            Debug.Log(go.name);
        }

        Debug.Log("\n right side: ");
        GameObject[] rightSideGOs = GetSide(Side.right);
        foreach (GameObject go in rightSideGOs) {
            Debug.Log(go.name);
        }

        Debug.Log("\n front side: ");
        GameObject[] gSideGOs = GetSide(Side.front);
        foreach (GameObject go in gSideGOs) {
            Debug.Log(go.name);
        }

        Debug.Log("\n back side: ");
        GameObject[] bSideGOs = GetSide(Side.back);
        foreach (GameObject go in bSideGOs) {
            Debug.Log(go.name);
        }
    }
    public void TEST__Create_DupeCubeFromStickers() {
        Vector3 offset = new Vector3(5, 0, 5);

        CreateBaseCubes(len,hei,width, offset);
        AttachStickers(len,hei,width, offset);
    }
    public void TEST__Print_StickersArray() {
        for (int i = 0; i < stickers.Length; i++) {
            Debug.Log("stickers[" + i + "] is " + stickers[i].color);
        }
    }
    private void CreateBaseCubes(int len, int hei, int width, Vector3 offset) {
        Material black = Resources.Load("Material/black", typeof(Material)) as Material;

        for (int y = 0; y < hei; y++) {
            for (int z = 0; z < width; z++) {
                for (int x = 0; x < len; x++) {
                    GameObject tmpCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    tmpCube.name = "Cube " + x + " " + y + " " + z;
                    tmpCube.transform.position = new Vector3(x+.5f, y+.5f, z+.5f) + offset;
                    tmpCube.GetComponent<Renderer>().material = black;
                }
            }
        }
    }
    private void AttachStickers(int len, int hei, int width, Vector3 offset) {
        //int toBePlaced = len * hei * 3;
        Material white = Resources.Load("Material/white", typeof(Material)) as Material;
        Material yellow = Resources.Load("Material/yellow", typeof(Material)) as Material;
        Material red = Resources.Load("Material/red", typeof(Material)) as Material;
        Material orange = Resources.Load("Material/orange", typeof(Material)) as Material;
        Material blue = Resources.Load("Material/blue", typeof(Material)) as Material;
        Material green = Resources.Load("Material/green", typeof(Material)) as Material;

        int toBePlacedPerSide = len * width;

        for (int side = 0; side < 3; side++) {
            bool variedLen = false, variedHei = false , variedWidth = false;
            
            if (side == (int)Side.top) {
                variedHei = true;
            }
            else if (side == (int)Side.front) {
                variedWidth = true;
            } 
            else if( side == (int)Side.right) {
                variedLen = true;
            } else {
                throw new System.IndexOutOfRangeException("side index out of range.");
            }

            for (int i = 0; i < len; i++) {
                for (int j = 0; j < width; j++) {
                    GameObject tmpSticker = GameObject.CreatePrimitive(PrimitiveType.Quad);
                    GameObject tmpSticker2 = GameObject.CreatePrimitive(PrimitiveType.Quad);
                    tmpSticker.transform.localScale = new Vector3(.9f, .9f, .9f);
                    tmpSticker2.transform.localScale = new Vector3(.9f, .9f, .9f);
                    tmpSticker.name = "sticker " + side + ": " + i + " " + j;
                    tmpSticker2.name = "sticker -" + side + ": " + i + " " + j;

                    int switchVal = (i << 1) | j;
                    // left/right sides
                    if (variedLen) {
                        tmpSticker.transform.position = new Vector3(-.01f, i + .5f, j + .5f) + offset;
                        tmpSticker.transform.Rotate(new Vector3(0,90,0));
                        switch (switchVal) {
                            case 0:
                                tmpSticker.GetComponent<MeshRenderer>().material = stickers[16].stickerObj.GetComponent<MeshRenderer>().material;
                                break;
                            case 1:
                                tmpSticker.GetComponent<MeshRenderer>().material = stickers[17].stickerObj.GetComponent<MeshRenderer>().material;
                                break;
                            case 2:
                                tmpSticker.GetComponent<MeshRenderer>().material = stickers[18].stickerObj.GetComponent<MeshRenderer>().material;
                                break;
                            case 3:
                                tmpSticker.GetComponent<MeshRenderer>().material = stickers[19].stickerObj.GetComponent<MeshRenderer>().material;
                                break;
                            default:
                                throw new System.IndexOutOfRangeException("i and j escaped their loop bounds in AttachStickers()");
                        }

                        tmpSticker2.transform.position = new Vector3(len+.01f, i + .5f, j + .5f) + offset;
                        tmpSticker2.transform.Rotate(new Vector3(0, 270, 0));
                        switch (switchVal) {
                            case 0:
                                tmpSticker.GetComponent<MeshRenderer>().material = stickers[8].stickerObj.GetComponent<MeshRenderer>().material;
                                break;
                            case 1:
                                tmpSticker.GetComponent<MeshRenderer>().material = stickers[9].stickerObj.GetComponent<MeshRenderer>().material;
                                break;
                            case 2:
                                tmpSticker.GetComponent<MeshRenderer>().material = stickers[10].stickerObj.GetComponent<MeshRenderer>().material;
                                break;
                            case 3:
                                tmpSticker.GetComponent<MeshRenderer>().material = stickers[11].stickerObj.GetComponent<MeshRenderer>().material;
                                break;
                            default:
                                throw new System.IndexOutOfRangeException("i and j escaped their loop bounds in AttachStickers()");
                        }
                        //tmpSticker2.GetComponent<MeshRenderer>().material = green;
                    // front/back sides
                    } else if (variedHei) {
                        tmpSticker.transform.position = new Vector3(i + .5f, hei+.01f, j + .5f) + offset;
                        tmpSticker.transform.Rotate(new Vector3(90, 0, 0));
                        switch (switchVal) {
                            case 0:
                                tmpSticker.GetComponent<MeshRenderer>().material = stickers[4].stickerObj.GetComponent<MeshRenderer>().material;
                                break;
                            case 1:
                                tmpSticker.GetComponent<MeshRenderer>().material = stickers[5].stickerObj.GetComponent<MeshRenderer>().material;
                                break;
                            case 2:
                                tmpSticker.GetComponent<MeshRenderer>().material = stickers[6].stickerObj.GetComponent<MeshRenderer>().material;
                                break;
                            case 3:
                                tmpSticker.GetComponent<MeshRenderer>().material = stickers[7].stickerObj.GetComponent<MeshRenderer>().material;
                                break;
                            default:
                                throw new System.IndexOutOfRangeException("i and j escaped their loop bounds in AttachStickers()");
                        }
                        //tmpSticker.GetComponent<MeshRenderer>().material = red;

                        tmpSticker2.transform.position = new Vector3(i + .5f, -.01f, j + .5f) + offset;
                        tmpSticker2.transform.Rotate(new Vector3(270, 0, 0));
                        switch (switchVal) {
                            case 0:
                                tmpSticker.GetComponent<MeshRenderer>().material = stickers[12].stickerObj.GetComponent<MeshRenderer>().material;
                                break;
                            case 1:
                                tmpSticker.GetComponent<MeshRenderer>().material = stickers[13].stickerObj.GetComponent<MeshRenderer>().material;
                                break;
                            case 2:
                                tmpSticker.GetComponent<MeshRenderer>().material = stickers[14].stickerObj.GetComponent<MeshRenderer>().material;
                                break;
                            case 3:
                                tmpSticker.GetComponent<MeshRenderer>().material = stickers[15].stickerObj.GetComponent<MeshRenderer>().material;
                                break;
                            default:
                                throw new System.IndexOutOfRangeException("i and j escaped their loop bounds in AttachStickers()");
                        }
                        //tmpSticker2.GetComponent<MeshRenderer>().material = orange;
                        // top/bottom sides
                    } else if (variedWidth) {
                        tmpSticker.transform.position = new Vector3(i + .5f, j + .5f, -.01f) + offset;
                        switch (switchVal) {
                            case 0:
                                tmpSticker.GetComponent<MeshRenderer>().material = stickers[0].stickerObj.GetComponent<MeshRenderer>().material;
                                break;
                            case 1:
                                tmpSticker.GetComponent<MeshRenderer>().material = stickers[1].stickerObj.GetComponent<MeshRenderer>().material;
                                break;
                            case 2:
                                tmpSticker.GetComponent<MeshRenderer>().material = stickers[2].stickerObj.GetComponent<MeshRenderer>().material;
                                break;
                            case 3:
                                tmpSticker.GetComponent<MeshRenderer>().material = stickers[3].stickerObj.GetComponent<MeshRenderer>().material;
                                break;
                            default:
                                throw new System.IndexOutOfRangeException("i and j escaped their loop bounds in AttachStickers()");
                        }
                        //tmpSticker.GetComponent<MeshRenderer>().material = white;

                        tmpSticker2.transform.position = new Vector3(i + .5f, j + .5f, width+.01f) + offset;
                        tmpSticker2.transform.Rotate(new Vector3(0, 180, 0));
                        switch (switchVal) {
                            case 0:
                                tmpSticker.GetComponent<MeshRenderer>().material = stickers[20].stickerObj.GetComponent<MeshRenderer>().material;
                                break;
                            case 1:
                                tmpSticker.GetComponent<MeshRenderer>().material = stickers[21].stickerObj.GetComponent<MeshRenderer>().material;
                                break;
                            case 2:
                                tmpSticker.GetComponent<MeshRenderer>().material = stickers[22].stickerObj.GetComponent<MeshRenderer>().material;
                                break;
                            case 3:
                                tmpSticker.GetComponent<MeshRenderer>().material = stickers[23].stickerObj.GetComponent<MeshRenderer>().material;
                                break;
                            default:
                                throw new System.IndexOutOfRangeException("i and j escaped their loop bounds in AttachStickers()");
                        }
                        //tmpSticker2.GetComponent<MeshRenderer>().material = yellow;
                    }
                }
            }
        }
    }
}