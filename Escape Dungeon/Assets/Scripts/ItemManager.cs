using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    public Image Potion;
    public Image FadeBG;
    public Image BG;
    public Text HavePotionCntText;
    public Text CurrentCoin;

    public int HealHp = 10;

    public int HavePotionCnt;

    public int HaveCoinCnt = 0;


    bool isCanUse = true;

    public static ItemManager instance;
    // Start is called before the first frame update
    void Start()
    {
        HavePotionCntText.GetComponent<Text>().text = HavePotionCnt.ToString();
    }
    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        CurrentCoin.GetComponent<Text>().text = "Coin : " + HaveCoinCnt.ToString();

        ItemUse();
        if(HavePotionCnt > 0)
        {
            Potion.gameObject.SetActive(true);
        }
        else
        {
            Potion.gameObject.SetActive(false);
        }
    }

    IEnumerator PotionDelay()
    {

        for (float i = 0.8f; i >= -0.1f; i -= 0.002f)
        {
            BG.color = new Vector4(1, 0, 0, 0.2f);
            Color color = new Vector4(0, 0, 0, i);
            FadeBG.color = color;

            yield return new WaitForEndOfFrame();
        }
        BG.color = new Vector4(1, 1, 1, 0.2f);
        isCanUse = true;

    }

    void ItemUse()
    {
        if (isCanUse && HavePotionCnt > 0)
        {
            if (Input.GetButton("ItemUse"))
            {
                if(GameManager.instance.PlayerHp + HealHp >= GameManager.instance.PlayerMaxHp)
                {
                    isCanUse = false;
                    HavePotionCnt--;
                    HavePotionCntText.GetComponent<Text>().text = HavePotionCnt.ToString();
                    GameManager.instance.PlayerHp = GameManager.instance.PlayerMaxHp;
                    GameManager.instance.PlayerEnergyBar.GetComponent<EnergyBar>().SetValueCurrent(GameManager.instance.PlayerHp);
                    StartCoroutine(PotionDelay());
                }
                else
                {
                    isCanUse = false;
                    HavePotionCnt--;
                    HavePotionCntText.GetComponent<Text>().text = HavePotionCnt.ToString();
                    GameManager.instance.PlayerHp = GameManager.instance.PlayerHp + HealHp;
                    GameManager.instance.PlayerEnergyBar.GetComponent<EnergyBar>().SetValueCurrent(GameManager.instance.PlayerHp);
                    StartCoroutine(PotionDelay());
                }
            }
        }

    }
}
