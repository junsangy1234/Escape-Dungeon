using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClearStage : MonoBehaviour
{

    public static ClearStage instance;
    public Text ClearText;

    bool isClear = false;

    public int killCnt = 0;

    bool is2Stage = true;
    bool isBoss = true;


    public GameObject[] Moster;
    public int[] MonsterCnt;
    public int NowMonsterCnt=1;
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.instance.PlayBGM(SoundManager.instance.Stage1Bgm, 0, true);
    }
    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.instance.MainStage == 2 && is2Stage)
        {
            is2Stage = false;

            Destroy(GameObject.Find("BGM"));
            SoundManager.instance.PlayBGM(SoundManager.instance.Stage2Bgm, 0, true);
        }
        if (GameManager.instance.MainStage == 3 && isBoss)
        {
            isBoss = false;

            Destroy(GameObject.Find("BGM"));
            SoundManager.instance.PlayBGM(SoundManager.instance.BossBgm, 0, true);
        }
        ClearCheckStage();
    }


    void ClearCheckStage()
    {
        if (killCnt == 1 && !isClear)
        {
            isClear = true;

            ClearText.GetComponentInChildren<Text>().text ="1 - 1 성공!";
            StartCoroutine(TextFade());
            GameManager.instance.NowStageClear();
        }
        else if (killCnt == 4 && isClear)
        {
            isClear = false;

            ClearText.GetComponentInChildren<Text>().text = "1 - 2 성공!";
            StartCoroutine(TextFade());
            GameManager.instance.NowStageClear();
        }
        else if (killCnt == 9 && !isClear)
        {
            isClear = true;

            ClearText.GetComponentInChildren<Text>().text = "1 - 3 성공!";
            StartCoroutine(TextFade());
            GameManager.instance.NowStageClear();
        }
        else if (killCnt == 17 && isClear)
        {
            isClear = false;

            ClearText.GetComponentInChildren<Text>().text = "1 - 4 성공!";
            StartCoroutine(TextFade());
            GameManager.instance.NowStageClear();
        }
        else if (killCnt == 25 && !isClear)
        {
            isClear = true;

            ClearText.GetComponentInChildren<Text>().text = "1 - 5 성공!";
            StartCoroutine(TextFade());
            GameManager.instance.NowStageClear();
        }
        else if (killCnt == 28 && isClear)
        {
            isClear = false;

            ClearText.GetComponentInChildren<Text>().text = "2 - 1 성공!";
            StartCoroutine(TextFade());
            GameManager.instance.NowStageClear();
        }
        else if (killCnt == 31 && !isClear)
        {
            isClear = true;

            ClearText.GetComponentInChildren<Text>().text = "2 - 2 성공!";
            StartCoroutine(TextFade());
            GameManager.instance.NowStageClear();
        }
        else if (killCnt == 36 && isClear)
        {
            isClear = false;

            ClearText.GetComponentInChildren<Text>().text = "2 - 3 성공!";
            StartCoroutine(TextFade());
            GameManager.instance.NowStageClear();
        }
        else if (killCnt == 43 && !isClear)
        {
            isClear = true;

            ClearText.GetComponentInChildren<Text>().text = "2 - 4 성공!";
            StartCoroutine(TextFade());
            GameManager.instance.NowStageClear();
        }
        else if (killCnt == 50 && isClear)
        {
            isClear = false;

            ClearText.GetComponentInChildren<Text>().text = "2 - 5 성공!";
            StartCoroutine(TextFade());
            GameManager.instance.NowStageClear();
        }
        else if(killCnt == 51 && !isClear)
        {
            isClear = true;
            ClearText.GetComponentInChildren<Text>().text = "Boss Stage 성공!";
            StartCoroutine(TextFade());
            GameManager.instance.NowStageClear();
        }
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

    public void summonMoster()
    {
        for(int i = NowMonsterCnt;i<= MonsterCnt[GameManager.instance.StageCnt]; i++)
        {
            Moster[i - 1].SetActive(true);
        }
        NowMonsterCnt = MonsterCnt[GameManager.instance.StageCnt]+1;
    } 
}
