using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraLook : MonoBehaviour
{
    public float sens = 700;   //감도 설정
    float rotationX = 0;   //수평 방향 회전 (좌우 회전)
    float rotationY = 0;   //수직 방향 회전 (상하 회전)

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");

        rotationX += x * sens * Time.deltaTime;
        rotationY += y * sens * Time.deltaTime;

        if (rotationY < 0)
        {
            rotationY = 0;
        }
        else if (rotationY > 0)
        {
            rotationY = 0;
        }

        //회전은 오일러 공식
        transform.eulerAngles = new Vector3(-rotationY, rotationX, 0);
    }

}
