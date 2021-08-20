using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndDoor : MonoBehaviour
{
    public GameObject OpenDoor;
    public GameObject CloseDoor;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
           if(Key.instance.isKey)
            {
                SoundManager.instance.PlaySfx(transform.position, SoundManager.instance.MetalDoor, 0, SoundManager.instance.sfxVolum);
                KadukiBlackHair.instance.isMove = true;
                        Move3D.instance.moveSpeed = 0;
                CloseDoor.GetComponent<MeshRenderer>().enabled = false;
                CloseDoor.GetComponent<MeshCollider>().enabled = false;
                OpenDoor.GetComponent<MeshRenderer>().enabled = true;
                OpenDoor.GetComponent<MeshCollider>().enabled = true;
            }

        }
    }
}
