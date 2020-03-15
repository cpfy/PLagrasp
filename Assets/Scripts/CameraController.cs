using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float movespeed;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (Camera.main.fieldOfView <= 100)
                Camera.main.fieldOfView += 2;
            if (Camera.main.orthographicSize <= 20)
                Camera.main.orthographicSize += 0.5F;
        }
        //Zoom in
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (Camera.main.fieldOfView > 2)
                Camera.main.fieldOfView -= 2;
            if (Camera.main.orthographicSize >= 1)
                Camera.main.orthographicSize -= 0.5F;
        }

        if (Input.GetMouseButton(1))
        {
            // 获取鼠标的x和y的值，乘以速度和Time.deltaTime是因为这个可以是运动起来更平滑  
            float h = -Input.GetAxis("Mouse X") * movespeed * Time.deltaTime ;
            float v = -Input.GetAxis("Mouse Y") * movespeed * Time.deltaTime ;
            // 设置当前摄像机移动，y轴并不改变  
            // 需要摄像机按照世界坐标移动，而不是按照它自身的坐标移动，所以加上Spance.World
            this.transform.Translate(h, v, 0, Space.World);
        }  
    }
}
