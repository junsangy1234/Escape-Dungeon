using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;
    public GameObject ShopUI;


    public Text CoinCntTx;
    public GameObject BuyFailInfoTx;


    private void Awake()
    {
        instance = this;
    }

    public void OpenShop()
    {
        Debug.Log("상점 열기");
        ShopUI.SetActive(true);

        CoinCntTx.text = "Coin : " + ItemManager.instance.HaveCoinCnt;
        BuyFailInfoTx.SetActive(false);

        Time.timeScale = 0;
    }

    public void CloseShop()
    {
        Debug.Log("상점 닫기");
        ShopUI.SetActive(false);

        Time.timeScale = 1;

        BasicKnife.instance.Delay3();

    }

    public void BuyBtClick()
    {
        if (ItemManager.instance.HaveCoinCnt >= 1) 
        {
            ItemManager.instance.HaveCoinCnt -= 1;
            ItemManager.instance.HavePotionCnt++;
            ItemManager.instance.HavePotionCntText.GetComponent<Text>().text = ItemManager.instance.HavePotionCnt.ToString();


            CoinCntTx.text = "Coin : " + ItemManager.instance.HaveCoinCnt;
        }
        else
        {
            BuyFailInfoTx.SetActive(true);
        }
    }
}
