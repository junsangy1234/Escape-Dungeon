using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GoblinStoneEnemy : MonoBehaviour
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

    public int hp = 10;
    public int maxHp = 10;
    public int Damage = 1;
    public float speed = 8.0f;
    public float rotSpeed = 10.0f;
    public float attackRange = 15f; //공격 범위
    public float traceRange = 20f; //추적 범위

    public int GetExp;

    int AtkCooTime = 3;
    int CurrentAtkCoolTime = 0;

    bool isCan = false;
    bool isCanAtk = false;

    bool isAtk = false;
    bool CanHit = true;
    bool LastIs3D = true;

    public GameObject GoblinStone;

    public GameObject StoneObj;
    public Transform CreateStone;

    Transform target;  //타켓


    CharacterController cc;
    Animator _ani;
    Transform tr;

    public static GoblinStoneEnemy instance;

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
        ChangeTo3D();


        if (CurrentAtkCoolTime < AtkCooTime)
        {
            StartCoroutine(CanAtk());
        }
        else
        {
            isCanAtk = true;
        }

 
        switch (enemystate)
        {
            case ENEMYSTATE.IDLE:
                {

                    _ani.SetBool("isIdle", true);
                    _ani.SetBool("isRun", false);
                    _ani.SetBool("isAtk", false);


                    float distance = Vector3.Distance(target.position, tr.position);

                    if (distance < traceRange)
                    {
                        enemystate = ENEMYSTATE.MOVE;
                        if (distance <= attackRange)
                        {
                            enemystate = ENEMYSTATE.ATTACK;
                        }
                    }
                    break;
                }
            case ENEMYSTATE.MOVE:
                {

                    if (isAtk) traceRange = 100;

                    _ani.SetBool("isIdle", false);
                    _ani.SetBool("isRun", true);
                    _ani.SetBool("isAtk", false);

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
                    else if (distance > traceRange)
                    {
                        enemystate = ENEMYSTATE.IDLE;
                    }

                    break;
                }
            case ENEMYSTATE.ATTACK:
                {

                    if (isCanAtk)
                    {
                        _ani.SetBool("isIdle", false);
                        _ani.SetBool("isRun", false);

                        CurrentAtkCoolTime = 0;
                        isCanAtk = false;

                        _ani.SetBool("isAtk", true);
                    }
                    else
                    {
                        float distance = Vector3.Distance(target.position, tr.position);
                        Vector3 dir = target.position - tr.position;

                        dir.y = 0.0f;
                        dir.Normalize();

                        tr.rotation = Quaternion.Lerp(tr.rotation, Quaternion.LookRotation(dir), rotSpeed * Time.deltaTime);

                        isAtk = true;
                        enemystate = ENEMYSTATE.IDLE;
                    }
                  

                    break;
                }
            case ENEMYSTATE.DAMAGE:
                {

                    float distance = Vector3.Distance(target.position, tr.position);

                    BasicKnife.instance.cnt++;


                    _ani.SetBool("isIdle", false);
                    _ani.SetBool("isRun", false);
                    _ani.SetBool("isAtk", false);

                    if (hp <= 0)
                    {
                        enemystate = ENEMYSTATE.DEATH;
                    }
                    else
                    {
                        enemystate = ENEMYSTATE.IDLE;
                    }
                    break;
                }
            case ENEMYSTATE.DEATH:
                {
                    SoundManager.instance.PlaySfx(tr.position, SoundManager.instance.GoblinDeath, 0, SoundManager.instance.sfxVolum);
                    GameManager.instance.ExpCal(GetExp);
                    Enemy.instance.EnemyDeath();
                    Destroy(gameObject);
                    break;
                }
        }

        if (!ChangeCamera.instance.Is3D)
        {
            cc.enabled = false;
            this.transform.position = new Vector3(0, cc.transform.position.y, cc.transform.position.z);
            cc.enabled = true;
        }
    }
    public void EnemyDamage()
    {
        if (!CanHit) return;

        SoundManager.instance.PlaySfx(tr.position, SoundManager.instance.GoblinHit, 0, SoundManager.instance.sfxVolum);
        SoundManager.instance.PlaySfx(tr.position, SoundManager.instance.GoblinHit2, 0, SoundManager.instance.sfxVolum);
        CanHit = false;

        hp = hp - GameManager.instance.Damage;
        enemystate = ENEMYSTATE.DAMAGE;
        _ani.SetTrigger("hit");

        enemystate = ENEMYSTATE.DAMAGE;

        Invoke("ChangeCanHit", 1.1f);
    }

    void IdleState()
    {
        enemystate = ENEMYSTATE.IDLE;

        _ani.SetBool("isIdle", true);
        _ani.SetBool("isAtk", false);
        _ani.SetBool("isRun", false);

        Invoke("ToResume", 2.0f);
    }
    void ToResume()
    {
        if (_ani.GetBool("isIdle"))
        {
            _ani.SetBool("isIdle", false);
            _ani.SetBool("isRun", true);
        }
    }

    void StoneAtk()
    {
        Instantiate(StoneObj, CreateStone.position,CreateStone.rotation);
    }



    IEnumerator CanAtk()
    {
        if(!isCan)
        {
            isCan = true;

            CurrentAtkCoolTime++;

            yield return new WaitForSeconds(1);

            isCan = false;
        }
    }
    void ChangeCanHit()
    {
        CanHit = true;
    }

    void ChangeTo3D()
    {
        if (LastIs3D != ChangeCamera.instance.Is3D)
        {
            LastIs3D = ChangeCamera.instance.Is3D;
            if (LastIs3D)
            {
                GoblinStone.GetComponent<CharacterController>().enabled = false;
                GoblinStone.AddComponent<Rigidbody>();
                GoblinStone.GetComponent<CapsuleCollider>().enabled = true;
                Invoke("OffRigidbody", 0.2f);
            }
        }
    }

    void OffRigidbody()
    {
        GoblinStone.GetComponent<CharacterController>().enabled = true;
        GoblinStone.GetComponent<CapsuleCollider>().enabled = false;
        Destroy(GoblinStone.GetComponent<Rigidbody>());
    }

    void GoblinAtkSnd()
    {
        SoundManager.instance.PlaySfx(transform.position, SoundManager.instance.GoblinAtk2, 0, SoundManager.instance.sfxVolum);
    }
}
