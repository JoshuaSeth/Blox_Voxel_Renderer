using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
            transform.position+=(new Vector3(0,0,1f) * Time.deltaTime * 20);
        if (Input.GetKey(KeyCode.S))
            transform.position +=(new Vector3(0, 0, -1f) * Time.deltaTime * 20);
        if (Input.GetKey(KeyCode.A))
            transform.Translate(Vector3.left * Time.deltaTime * 20);
        if (Input.GetKey(KeyCode.D))
            transform.Translate(Vector3.right * Time.deltaTime * 20);

        if (Camera.main.orthographic)
            Camera.main.orthographicSize += Input.mouseScrollDelta.normalized.y * (Camera.main.orthographicSize / 50);
        else
            Camera.main.transform.Translate(transform.up * Input.mouseScrollDelta.normalized.y * (transform.position.y/100));

        if (Input.GetKeyDown(KeyCode.U) && !Camera.main.orthographic)
            Camera.main.fieldOfView += 1;
        if (Input.GetKeyDown(KeyCode.Y) && !Camera.main.orthographic)
            Camera.main.fieldOfView -= 1;

    }
}
