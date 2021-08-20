using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bone : MonoBehaviour
{
    Transform tr;
    private void Awake()
    {
        tr = GetComponent<Transform>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(tr.gameObject); //자기자신(총알) 제거
    }

}
