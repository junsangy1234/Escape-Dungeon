using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public bool isKey = false;
    public GameObject KeyObj;

    public static Key instance;
    private void Awake()
    {
        instance = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            isKey = true;
            SoundManager.instance.PlaySfx(transform.position, SoundManager.instance.GetCoin, 0, SoundManager.instance.sfxVolum);
            Destroy(KeyObj);

        }
    }
}
