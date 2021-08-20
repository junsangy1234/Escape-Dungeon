using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _2DChangeAtk : MonoBehaviour
{
    Transform tr;
    Rigidbody rbody;

    bool isCan = true;

    float bulletSpeed = 700;

    // Start is called before the first frame update
    void Start()
    {
        tr = transform;
        rbody = GetComponent<Rigidbody>();

        rbody.AddForce(tr.forward * bulletSpeed);
        Destroy(tr.gameObject, 8.0f);
    }


    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.layer)
        {
            case 8:
                {
                    {
                        if(isCan)
                        {
                            
                            isCan = false;
                            Invoke("CanDamage", 3f);
                            SoundManager.instance.PlaySfx(tr.position, SoundManager.instance.D2AtkHit, 0, SoundManager.instance.sfxVolum);
                            GameManager.instance.PlayerHp = GameManager.instance.PlayerHp - 50;

                            GameManager.instance.PlayerEnergyBar.GetComponent<EnergyBar>().SetValueMax(GameManager.instance.PlayerMaxHp);
                            GameManager.instance.PlayerEnergyBar.GetComponent<EnergyBar>().SetValueMin(0);
                            GameManager.instance.PlayerEnergyBar.GetComponent<EnergyBar>().SetValueCurrent(GameManager.instance.PlayerHp);

                            PlayerState.instance.playerState = PlayerState.PLAYERSTATE.DAMAGE;
                            Destroy(gameObject);
                        }
                        
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
