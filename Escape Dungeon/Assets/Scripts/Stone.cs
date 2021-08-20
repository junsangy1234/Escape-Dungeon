using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    Transform tr;
    Rigidbody rbody;

    bool isCan = true;

    float bulletSpeed = 2000;


    // Start is called before the first frame update
    void Start()
    {
        tr = transform;
        rbody = GetComponent<Rigidbody>();

        rbody.AddForce(tr.forward * bulletSpeed);
        rbody.AddForce(tr.right * 650);
        Destroy(tr.gameObject, 5.0f);
    }


    private void OnCollisionEnter(Collision collision)
    {

        Destroy(tr.gameObject);

    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.layer)
        {
            case 8:
                {
                    if (isCan)
                    {
                        isCan = false;
                        Invoke("CanDamage", 1.5f);
                        GameManager.instance.PlayerHp = GameManager.instance.PlayerHp - GoblinStoneEnemy.instance.Damage;

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
