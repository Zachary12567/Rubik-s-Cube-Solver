using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour {

    public Camera cam;
    public GameObject center;

    private float speed;

    private Transform camTrans;
    private Transform centerTrans;

	void Start () {
        speed = 2.5f;

        camTrans = cam.GetComponent<Transform>();
        centerTrans = center.GetComponent<Transform>();
	}
	
	void Update () {
        if (Input.GetMouseButton(0)) {
            centerTrans.Rotate(new Vector3(Input.GetAxis("Mouse Y") * speed, Input.GetAxis("Mouse X") * speed, 0));

            //X = transform.rotation.eulerAngles.x;
            //Y//// = transform.rotation.eulerAngles.y;
            centerTrans.rotation = Quaternion.Euler(centerTrans.rotation.eulerAngles.x, centerTrans.rotation.eulerAngles.y, 0);
        }


        camTrans.LookAt(centerTrans);
	}
}
