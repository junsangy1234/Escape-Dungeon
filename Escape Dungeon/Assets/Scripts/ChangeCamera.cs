using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCamera : MonoBehaviour
{
    public bool Is3D = true;
    public GameObject Character;

    public Camera Cam3d;
    public Camera Cam2d;

    public static ChangeCamera instance;

    public GameObject Goblin;
    public GameObject GoblinStone;
    public GameObject DDaGGaLi;
    public GameObject SkeletonMiNi;


    public GameObject[] ALL2DObj;
    public GameObject[] ALL3DObj;


    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        
    }
    // Start is called before the first frame update
    public void Changecamera()
    {
        if (Is3D)
        {

            GameManager.instance.LastY_Velocty = Move3D.instance.yVelocty;

            for (int i = 0; i < ALL2DObj.Length; i++)
            {
                ALL2DObj[i].SetActive(true);
            }

            for (int i = 0; i < ALL3DObj.Length; i++)
            {
                ALL3DObj[i].SetActive(false);
            }

            Cam2d.transform.position = Cam3d.transform.position;
            Cam2d.transform.rotation = Cam3d.transform.rotation;
            Cam3d.GetComponent<Camera>().enabled = false;
            Cam2d.GetComponent<Camera>().enabled = true;

            

            Character.GetComponent<Move3D>().enabled = false;
            Character.GetComponent<Move2D>().enabled = true;

            Is3D = false;

            

        }
        else
        {

            GameManager.instance.LastY_Velocty = Move2D.instance.yVelocty;

            for (int i = 0; i < ALL2DObj.Length; i++)
            {
                ALL2DObj[i].SetActive(false);
            }

            for (int i = 0; i < ALL3DObj.Length; i++)
            {
                ALL3DObj[i].SetActive(true);
            }

            Cam3d.GetComponent<Camera>().enabled = true;
            Cam2d.GetComponent<Camera>().enabled = false;

            

            Character.GetComponent<Move3D>().enabled = true;
            Character.GetComponent<Move2D>().enabled = false;

            Is3D = true;

           

            
        }
    }

    

}
