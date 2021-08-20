using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public GameObject CoinObj;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            ItemManager.instance.HaveCoinCnt++;
            SoundManager.instance.PlaySfx(transform.position, SoundManager.instance.GetCoin, 0, SoundManager.instance.sfxVolum);
            Destroy(CoinObj);

        }
        if(other.gameObject.tag == "Ground")
        {
            StartCoroutine(DropCoinSnd());
        }
    }

    IEnumerator DropCoinSnd()
    {
        SoundManager.instance.PlaySfx(transform.position, SoundManager.instance.DropCoin, 0, SoundManager.instance.sfxVolum);

        yield return new WaitForSeconds(10000000000000000000);
    }
}
