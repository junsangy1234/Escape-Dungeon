using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckInRoomHall : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 18)
        {
            GameManager.instance.InRoomORHall = 1;
        }
        if(other.gameObject.layer == 19)
        {
            GameManager.instance.InRoomORHall = 0;
        }

        if(other.gameObject.layer == 20)
        {
            ShopManager.instance.OpenShop();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 20)
        {
            //ShopManager.instance.CloseShop();
        }
    }
}
