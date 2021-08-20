using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    public GameObject[] Trap;
    public static GameStart instance;
    private void Awake()
    {
        instance = this;
    }
    public void OnTrap()
    {
        Trap[0].SetActive(false);
        Trap[1].SetActive(false);
    }
}
