using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneColliderChk : MonoBehaviour
{
    bool isCan = true;

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.layer)
        {
            case 8:
                {
                    if (isCan)
                    {
                        isCan = false;
                        Invoke("CanDamage", 2f);
                        GameManager.instance.PlayerHp = GameManager.instance.PlayerHp - Random.Range(5,15);

                        GameManager.instance.PlayerEnergyBar.GetComponent<EnergyBar>().SetValueMax(GameManager.instance.PlayerMaxHp);
                        GameManager.instance.PlayerEnergyBar.GetComponent<EnergyBar>().SetValueMin(0);
                        GameManager.instance.PlayerEnergyBar.GetComponent<EnergyBar>().SetValueCurrent(GameManager.instance.PlayerHp);

                        PlayerState.instance.playerState = PlayerState.PLAYERSTATE.DAMAGE;
                    }
                    break;
                }
        }

    }
    void CanDamage()
    {
        isCan = true;
    }
}