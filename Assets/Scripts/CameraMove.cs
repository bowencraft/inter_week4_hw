using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject PlayerObject;
    public float CameraSpeed = 0.005f;
    public int CameraOffset = 3;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = transform.position;

        newPos.x = Mathf.Lerp(newPos.x, PlayerObject.transform.position.x, CameraSpeed);
        newPos.y = Mathf.Lerp(newPos.y, PlayerObject.transform.position.y + CameraOffset, CameraSpeed);

        transform.position = newPos;

    }
}
