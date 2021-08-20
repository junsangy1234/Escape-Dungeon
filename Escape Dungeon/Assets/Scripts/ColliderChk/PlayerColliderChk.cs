using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColliderChk : MonoBehaviour
{
    public GameObject weapon;

    bool isCan = true;

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.layer)
        {
            case 9:
                {
                    if(ChangeCamera.instance.Is3D)
                    {
                        if (isCan)
                        {
                            isCan = false;
                            Invoke("Can", 1.1f);

                            other.gameObject.SendMessage("EnemyDamage", SendMessageOptions.DontRequireReceiver);

                            BasicKnife.instance.weapon.GetComponent<BoxCollider>().enabled = false;

                            Debug.Log("맞음");

                        }
                    }
                    else
                    {
                            other.gameObject.SendMessage("EnemyDamage", SendMessageOptions.DontRequireReceiver);

                            Debug.Log("맞음");
                    }
                    
                    break;
                   
                }
            case 10:
                {
                    if (ChangeCamera.instance.Is3D)
                    {
                        if (isCan)
                        {

                            isCan = false;

                            other.gameObject.SendMessage("EnemyDamage", SendMessageOptions.DontRequireReceiver);
                            Invoke("Can", 1.1f);


                            BasicKnife.instance.weapon.GetComponent<BoxCollider>().enabled = false;

                        }
                    }
                    else
                    {
                        other.gameObject.SendMessage("EnemyDamage", SendMessageOptions.DontRequireReceiver);

                        Debug.Log("맞음");
                    }
                   
                        

                }
                break;
        }
    }

    void Can()
    {
        isCan = true;
    }

}

