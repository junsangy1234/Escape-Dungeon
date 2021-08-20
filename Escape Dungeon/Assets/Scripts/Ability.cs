using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ability : MonoBehaviour
{
    public GameObject SkillUI;
    public GameObject RealApply;
    public GameObject BG;

    public Text SkillPointText;
    public Text HpLevelText;
    public Text AtkLevelText;
    public Text CurrentHp;
    public Text CurrentAtk;
    public Text ApplyHp;
    public Text ApplyAtk;

    public int SkillPoint = 0;
    int CurrentHpLevel;
    int CurrentHp2;
    int CurrentAtkLevel;
    int CurrentSkillPoint;

    int HpLevel = 0;
    int AtkLevel = 0;

    int CuHp;
    int CuAtk;
    int Hp;
    int Atk;

    int cnt = 0;

    bool isSkill = false;


    public static Ability insatnce;

    private void Awake()
    {
        insatnce = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        BG.SetActive(false);
        RealApply.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        OpenUI();
    }

    public void OpenUI()
    {
        if (isSkill)
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                CloseUI();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                Invoke("SpeedBugFix", 1.0f);

                CuHp = GameManager.instance.PlayerMaxHp;
                CuAtk = GameManager.instance.Damage;

                //SkillUI.SetActive(true);
                BG.SetActive(true);
                RealApply.SetActive(false);
                SkillPointText.GetComponentInChildren<Text>().text = "SkillPoint : " + SkillPoint.ToString();
                CurrentAtk.GetComponentInChildren<Text>().text = "현재 공격력 : " + GameManager.instance.Damage.ToString();
                CurrentHp.GetComponentInChildren<Text>().text = "현재 체력 : " + GameManager.instance.PlayerMaxHp.ToString();
                ApplyAtk.GetComponentInChildren<Text>().text = "적용후 공격력 : " + GameManager.instance.Damage.ToString();
                ApplyHp.GetComponentInChildren<Text>().text = "적용후 체력 : " + GameManager.instance.PlayerMaxHp.ToString();
                CurrentHpLevel = HpLevel;
                CurrentAtkLevel = AtkLevel;
                CurrentSkillPoint = SkillPoint;
                CurrentHp2 = GameManager.instance.PlayerHp;

                isSkill = true;

                Time.timeScale = 0;
            }
        }
    }
    public void CloseUI()
    {
        //SkillUI.SetActive(false);
        BG.SetActive(false);
        RealApply.SetActive(false);
        Apply();
    }

    public void HpUp()
    {
        if(SkillPoint > 0)
        {
            cnt++;
            SkillPoint--;
            SkillPointText.GetComponentInChildren<Text>().text = "SkillPoint : " + SkillPoint.ToString();
            Hp = GameManager.instance.PlayerMaxHp + 5;
            GameManager.instance.PlayerMaxHp += 5;
            GameManager.instance.PlayerHp += 5;
            ApplyHp.GetComponentInChildren<Text>().text = "적용후 체력 : " + Hp.ToString();
            HpLevel++;
            HpLevelText.GetComponentInChildren<Text>().text = "HpLevel\n" + HpLevel.ToString();
            Atk = GameManager.instance.Damage;
        }
        
    }

    public void AtkUp()
    {
        if(SkillPoint > 0)
        {
            cnt++;
            SkillPoint--;
            SkillPointText.GetComponentInChildren<Text>().text = "SkillPoint : " + SkillPoint.ToString();
            Atk = GameManager.instance.Damage + 1;
            GameManager.instance.Damage += 1;
            ApplyAtk.GetComponentInChildren<Text>().text = "적용후 공격력 : " + Atk.ToString();
            AtkLevel++;
            AtkLevelText.GetComponentInChildren<Text>().text = "AttackLevel\n" + AtkLevel.ToString();
            Hp = GameManager.instance.PlayerMaxHp;
        }
        
    }

    public void Apply()
    {
        if(cnt != 0)
        {
            RealApply.SetActive(true);
            BG.SetActive(false);
        }
        else
        {
            RealApply.SetActive(false);
            BG.SetActive(false);

            isSkill = false;
            Time.timeScale = 1;
        }
    }

    public void Yes()
    {
        cnt = 0;
        GameManager.instance.PlayerMaxHp = Hp;
        GameManager.instance.Damage = Atk;
        CurrentAtkLevel = AtkLevel;
        CurrentHpLevel = HpLevel;
        CurrentSkillPoint = SkillPoint;
        AtkLevelText.GetComponentInChildren<Text>().text = "AttackLevel\n" + CurrentAtkLevel.ToString();
        HpLevelText.GetComponentInChildren<Text>().text = "HpLevel\n" + CurrentHpLevel.ToString();
        SkillPointText.GetComponentInChildren<Text>().text = "SkillPoint : " + SkillPoint.ToString();

        GameManager.instance.PlayerEnergyBar.GetComponent<EnergyBar>().SetValueMax(GameManager.instance.PlayerMaxHp);
        GameManager.instance.PlayerEnergyBar.GetComponent<EnergyBar>().SetValueCurrent(GameManager.instance.PlayerHp);

        //SkillUI.SetActive(false);
        BG.SetActive(false);
        RealApply.SetActive(false);

        isSkill = false;

        Time.timeScale = 1;
    }

    public void No()
    {
        cnt = 0;

        GameManager.instance.PlayerMaxHp = CuHp;
        GameManager.instance.PlayerHp = CurrentHp2;
        GameManager.instance.Damage = CuAtk;
        AtkLevel = CurrentAtkLevel;
        HpLevel = CurrentHpLevel;
        SkillPoint = CurrentSkillPoint;
        AtkLevelText.GetComponentInChildren<Text>().text = "AttackLevel\n" + CurrentAtkLevel.ToString();
        HpLevelText.GetComponentInChildren<Text>().text = "HpLevel\n" + CurrentHpLevel.ToString();
        SkillPointText.GetComponentInChildren<Text>().text = "SkillPoint : " + SkillPoint.ToString();

        GameManager.instance.PlayerEnergyBar.GetComponent<EnergyBar>().SetValueMax(GameManager.instance.PlayerMaxHp);

        //SkillUI.SetActive(false);
        BG.SetActive(false);
        RealApply.SetActive(false);

        isSkill = false;

        Time.timeScale = 1;
    }

    void SpeedBugFix()
    {
        Move3D.instance.moveSpeed = 10.0f;
        Move2D.instance.moveSpeed = 10.0f;
    }

}//end class
