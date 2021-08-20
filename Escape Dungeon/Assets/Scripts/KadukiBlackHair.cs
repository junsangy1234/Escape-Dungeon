using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class KadukiBlackHair : MonoBehaviour
{
    enum ENEMYSTATE
    {
        IDLE = 0,
        MOVE,
        ATTACK,
        DAMAGE,
        DEATH
    }
    ENEMYSTATE enemystate = ENEMYSTATE.IDLE;

    Vector3 currentVelocty; //현재 위치

    public float speed = 10.0f;
    public float rotSpeed = 10.0f;
    public float attackRange = 2.5f; //공격 범위
    public float traceRange = 10.0f; //추적 범위

    public bool isMove = false;

    public Text EndingText;
    public Text EndingText1;

    Transform target;  //타켓

    bool isAtk = false;

    CharacterController cc;
    Animator _ani;
    Transform tr;

    public static KadukiBlackHair instance;

    private void Awake()
    {
        instance = this;
        cc = GetComponent<CharacterController>();
        tr = GetComponent<Transform>();
        _ani = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(enemystate == ENEMYSTATE.ATTACK)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene("Stage 1");
            }
        }
        if (isMove)
        {
            isMove = false;
            enemystate = ENEMYSTATE.MOVE;
        }
        switch (enemystate)
        {
            case ENEMYSTATE.IDLE:
                {

                    break;
                }
            case ENEMYSTATE.MOVE:
                {

                    if (this.gameObject.tag == "Skeleton") Invoke("AtkDelay", 0.5f);

                    _ani.SetBool("isWalk", true);

                    float distance = Vector3.Distance(target.position, tr.position);

                    Vector3 dir = target.position - tr.position;
                    dir.y = 0.0f; //점프하면서 따라오는거 방지
                    dir.Normalize();  //거리 평준화 함수 ( 플레이어한테 부드럽게 따라옴 ) 


                    cc.SimpleMove(dir * speed);

                    tr.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), rotSpeed * Time.deltaTime);

                    if (distance <= attackRange)
                    {
                        enemystate = ENEMYSTATE.ATTACK;
                    }

                    break;
                }
            case ENEMYSTATE.ATTACK:
                {
                    if (!isAtk)
                    {
                        isAtk = true;
                        SoundManager.instance.PlayBGM(SoundManager.instance.VictoryBgm, 0, true);
                        GameObject.Find("Player").GetComponent<CharacterController>().enabled = false;
                        _ani.SetBool("isClear", true);
                        _ani.SetBool("isWalk", false);
                        StartCoroutine(TextFade());
                        EndingText1.color = new Vector4(1, 1, 0, 1);
                        

                    }
                   

                    break;
                }
            case ENEMYSTATE.DAMAGE:
                {

                    break;
                }
            case ENEMYSTATE.DEATH:
                {

                    break;
                }
        }

    }

    IEnumerator TextFade()
    {

        for (float i = 1f; i >= -0.1f; i -= 0.001f)
        {
            Color color = new Vector4(1, 1, 1, i);
            EndingText.color = color;

            yield return new WaitForEndOfFrame();
        }
        

    }
}
