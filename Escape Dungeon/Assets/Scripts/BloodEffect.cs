using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BloodEffect : MonoBehaviour
{
    public static Image image;

    public Sprite blood;

    public Color startColor = new Color(1, 1, 1, 0); //투명 //rgba?
    public Color endColor = new Color(1, 1, 1, 1); //불투명


    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.PlayerHp < GameManager.instance.PlayerMaxHp * 0.2f) //상시 체크 발동
        {
            image.sprite = blood;
            image.color = Color.Lerp(endColor, startColor, Mathf.PingPong(Time.time, 1.5f));
        }
        else
        {
            image.sprite = blood;
            image.color = startColor;
        }


    }
}
