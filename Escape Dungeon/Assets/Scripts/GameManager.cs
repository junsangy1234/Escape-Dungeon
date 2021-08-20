using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public int Damage;


    public int PlayerHp;
    public int PlayerMaxHp;
    public GameObject PlayerEnergyBar;

    public int PlayerLevel ;
    public Text PlayerLevelView;
    public Text ClearText;

    public int[] PlayerLimitExp;
    public int PlayerCurrentExp;
    public int PlayerMaxExp;
    public GameObject PlayerExpBar;

    public int Cooltime;
    public int CurrentCooltime;
    public GameObject CoolTimeBar;
    public bool canCoolTime = true;

    public int Enable2D3DStat;
    bool is3D = true;
    bool can2D3DChange = true;


    bool isCoolTime = false;

    int CoinRand=0;

    public int MainStage = 0;
    public int SubStage = 0;
    public GameObject[] Stage1_Door;
    public GameObject[] Stage2_Door;
    public GameObject Boss_Door;
    public int DoorNumber = 0;
    bool StageBtUsed = false;


    public int StageCnt = 0;
    public int[] CoinDropCnt;
    public GameObject CoinObj;
    public GameObject[] CoinSummonPoint;


    public int InRoomORHall = 0; // Room:0 Hall:1

    public GameObject LevelUpEffect;
    GameObject Effect;

    public GameObject Change2D3DBt;

    

    public static GameManager instance;


    public float LastY_Velocty = 0;
    private void Awake()
    {
        instance = this;

        //HP
        PlayerEnergyBar.GetComponent<EnergyBar>().SetValueMax(PlayerMaxHp);
        PlayerEnergyBar.GetComponent<EnergyBar>().SetValueMin(0);
        PlayerEnergyBar.GetComponent<EnergyBar>().SetValueCurrent(PlayerMaxHp);
        //EXP
        PlayerExpBar.GetComponent<EnergyBar>().SetValueMax(PlayerMaxExp);
        PlayerExpBar.GetComponent<EnergyBar>().SetValueMin(0);
        PlayerExpBar.GetComponent<EnergyBar>().SetValueCurrent(PlayerCurrentExp);
        //쿨타임
        CoolTimeBar.GetComponent<EnergyBar>().SetValueMax(Cooltime);
        CoolTimeBar.GetComponent<EnergyBar>().SetValueMin(0);
        CoolTimeBar.GetComponent<EnergyBar>().SetValueCurrent(Cooltime);

        //2D3D COLOR 찾음

        
    }

    //경험치계산
    public int ExpCal (int exp)
    {
        PlayerCurrentExp += exp;
        if( PlayerLevel < PlayerLimitExp.Length) //PlayerLimitExp.Length  = 만렙
        {
            if(PlayerCurrentExp >= PlayerLimitExp[PlayerLevel - 1])
            {
                PlayerLevel += 1;
                Ability.insatnce.SkillPoint += 2;
                PlayerCurrentExp = Mathf.Abs(PlayerLimitExp[PlayerLevel - 2] - PlayerCurrentExp); //이렇게 해야지 만약 90%에서 30%경험치 얻으면 다음레벨 20%로 됨
                PlayerMaxExp = PlayerLimitExp[PlayerLevel - 1];

                SoundManager.instance.PlaySfx(transform.position, SoundManager.instance.levelUp, 0, SoundManager.instance.sfxVolum * 0.3f);
                //CreateEffect();

            }
        }
        //만렙 이상
        else if( PlayerLevel >= PlayerLimitExp.Length )
        {
            Debug.Log("만렙입니다.");
        }

        //현재 레벨 뷰 업데이트
        PlayerLevelView.text = "Lv"+PlayerLevel.ToString();
        //경험치 바 뷰 업데이트
        PlayerExpBar.GetComponent<EnergyBar>().SetValueMax(PlayerMaxExp);
        PlayerExpBar.GetComponent<EnergyBar>().SetValueCurrent(PlayerCurrentExp);

        return exp;
    }

        // Start is called before the first frame update
        void Start()
    {
        SoundManager.instance.PlayBGM(SoundManager.instance.Stage1Bgm, 0, true);

        //Cursor.lockState = CursorLockMode.Locked;
        PlayerLevel = 1;
    }

    // Update is called once per frame
    void Update()
    {

        Enable2D3DStat = Enable2D3DBt.instance.Enable2D3DStat;
        is3D = ChangeCamera.instance.Is3D;

        if(CurrentCooltime < Cooltime)
        {
            StartCoroutine(CoolTime());
        }
        else if(CurrentCooltime == Cooltime)
        {
            canCoolTime = true;
        }

        



        if(Enable2D3DStat == 1) // ALL
        {
            can2D3DChange = true;
        }
        else if(Enable2D3DStat == 2) // 2D
        {
            if (is3D)
            {
                can2D3DChange = true;
            }
            else
            {
                can2D3DChange = false;
            }
        }
        else // 3D
        {
            if (is3D)
            {
                can2D3DChange = false;
            }
            else
            {
                can2D3DChange = true;
            }
        }


        if (can2D3DChange)
        {
            Change2D3DBt.SetActive(false);
        }
        else
        {
            Change2D3DBt.SetActive(true);
        }




        if ((Input.GetButtonDown("Change2D3D") && canCoolTime)&& can2D3DChange)
        {
            canCoolTime = false;
            CurrentCooltime = 0;
            CoolTimeBar.GetComponent<EnergyBar>().SetValueMax(Cooltime);
            CoolTimeBar.GetComponent<EnergyBar>().SetValueCurrent(CurrentCooltime);
            ChangeCamera.instance.Changecamera();
        }


    }


    IEnumerator CoolTime()
    {
        if(!isCoolTime)
        {
            isCoolTime = true;

            CurrentCooltime += 1;
            CoolTimeBar.GetComponent<EnergyBar>().SetValueMax(Cooltime);
            CoolTimeBar.GetComponent<EnergyBar>().SetValueCurrent(CurrentCooltime);

            isCoolTime = false;

            yield return new WaitForSeconds(0.1f);
        }   
    }

    public void NextMainStage()
    {
        StageBtUsed = false;

        MainStage++;
        SubStage = 1;
        DoorNumber = 0;
        StageCnt++;
        if(MainStage == 3)
        {
            ClearText.GetComponentInChildren<Text>().text = "Boss Stage 입장";
        }
        else ClearText.GetComponentInChildren<Text>().text = MainStage.ToString() + " - " + SubStage.ToString() + " 입장";
        StartCoroutine(TextFade());


        Debug.Log(MainStage + " - " + SubStage + " 입장 ");

        
        if (MainStage == 2)
        {
            Stage1_Door[4].transform.rotation = Quaternion.Euler(Stage1_Door[DoorNumber].transform.rotation.x, 0, Stage1_Door[DoorNumber].transform.rotation.z);
        }
        else if (MainStage == 3)
        {
            Stage2_Door[4].transform.rotation = Quaternion.Euler(Stage1_Door[DoorNumber].transform.rotation.x, 0, Stage1_Door[DoorNumber].transform.rotation.z);
        }




        ClearStage.instance.summonMoster();
    }

    public void NextSubStage()
    {
        StageBtUsed = false;

        SubStage++;
        StageCnt++;
        ClearText.GetComponentInChildren<Text>().text = MainStage.ToString() + " - " + SubStage.ToString() + " 입장";
        StartCoroutine(TextFade());

        Debug.Log(MainStage + " - " + SubStage + " 입장 ");


        if (MainStage == 1)
        {
            
            Stage1_Door[DoorNumber - 1].transform.rotation = Quaternion.Euler(Stage1_Door[DoorNumber].transform.rotation.x, 0, Stage1_Door[DoorNumber].transform.rotation.z);
            
        }
        else if (MainStage == 2)
        {
            Stage2_Door[DoorNumber - 1].transform.rotation = Quaternion.Euler(Stage1_Door[DoorNumber].transform.rotation.x, 0, Stage1_Door[DoorNumber].transform.rotation.z);
            
        }
        


        ClearStage.instance.summonMoster();
    }
    public void NowStageClear()
    {

        if (StageBtUsed) return;
        StageBtUsed = true;

        Debug.Log(MainStage + " - " + SubStage + " 클리어 ");

        //Stage1_Door[DoorNumber].transform.rotation = Quaternion.Lerp(Stage1_Door[DoorNumber].transform.rotation, Quaternion.Euler(Stage1_Door[DoorNumber].transform.rotation.x,130, Stage1_Door[DoorNumber].transform.rotation.z),Time.deltaTime);
        if(MainStage == 1)
        {
            Stage1_Door[DoorNumber].transform.rotation = Quaternion.Euler(Stage1_Door[DoorNumber].transform.rotation.x, 130, Stage1_Door[DoorNumber].transform.rotation.z);
            
        }
        else if(MainStage == 2)
        {
            Stage2_Door[DoorNumber].transform.rotation = Quaternion.Euler(Stage1_Door[DoorNumber].transform.rotation.x, 130, Stage1_Door[DoorNumber].transform.rotation.z);
            
        }
        else if (MainStage == 3)
        {
           // Boss_Door.transform.rotation = Quaternion.Euler(Stage1_Door[DoorNumber].transform.rotation.x, 130, Stage1_Door[DoorNumber].transform.rotation.z);
            
        }
        DoorNumber++;

        CoinRand = Random.RandomRange(CoinDropCnt[StageCnt - 1], CoinDropCnt[StageCnt - 1]+3);

        for(int i=1; i <= CoinRand; i++)
        {
            SummonCoin();
        }


    }

    public void ChangedView()
    {

    }
    
    IEnumerator TextFade()
    {

        for (float i = 1f; i >= -0.1f; i -= 0.01f)
        {
            Color color = new Vector4(1, 1, 1, i);
            ClearText.color = color;

            yield return new WaitForEndOfFrame();
        }

    }

    void CreateEffect()
    {
        GameObject effect = Instantiate(LevelUpEffect, PlayerState.instance.EffectCreate.position, PlayerState.instance.EffectCreate.rotation);
        Effect = effect;
        Invoke("DeleteEffect", 1.0f);
        
    }

    void DeleteEffect()
    {
        Destroy(Effect);
    }

    void SummonCoin()
    {
        Debug.Log("코인생성");
        Instantiate(CoinObj, CoinSummonPoint[StageCnt-1].transform.position,Quaternion.Euler(0,0,0));
    }
}
