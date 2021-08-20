using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enable2D3DBt : MonoBehaviour
{
    public static Enable2D3DBt instance;

    public int Enable2D3DStat = 3;

    private void Awake()
    {
        instance = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 11) 
        {
            Enable2D3DStat = 1;
        }
        else if (other.gameObject.layer == 12) 
        {
            Enable2D3DStat = 2;
        }
        else if (other.gameObject.layer == 13)
        {
            Enable2D3DStat = 3;
        }
    }
}
