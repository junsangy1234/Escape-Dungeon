using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneManager : MonoBehaviour
{

    public static StoneManager instance;

    private void Awake()
    {
        instance = this;
    }

    
}
