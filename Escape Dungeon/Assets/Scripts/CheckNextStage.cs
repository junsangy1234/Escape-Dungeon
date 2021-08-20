using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckNextStage : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 16) // mainStage
        {
            other.gameObject.layer = 17;
            GameManager.instance.NextMainStage();

        }
        else if(other.gameObject.layer == 15)
        {
            other.gameObject.layer = 17;
            GameManager.instance.NextSubStage();
        }

        if(other.gameObject.layer == 21)
        {
            GameStart.instance.OnTrap();
            SoundManager.instance.PlaySfx(transform.position, SoundManager.instance.MetalDoor, 0, SoundManager.instance.sfxVolum);
        }
    }
}
