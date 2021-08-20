using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkeletonEnemy : MonoBehaviour
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

    int cnt = 0;

    int BoneSpeed = 1000;

    public int GetExp;

    public int hp = 10;
    public int maxHp = 10;
    public int Damage = 1;
    public float speed = 10.0f;
    public float rotSpeed = 10.0f;
    public float attackRange = 2.5f; //공격 범위
    public float traceRange = 10.0f; //추적 범위

    public GameObject[] Effect;

    bool isFight = false;
    bool isHit = false;
    bool isSummon = false;
    bool isBone = false;
    bool isChange2D = false;
    bool isSMoveSnd = false;
    bool isSHitSnd = false;
    public bool isDie = false;

    public bool is2DChangeHit = false;

    bool isCanSummon = false;
    bool isCanBone = false;
    bool isCanChnage2D = false;
    bool CanHit = true;

    int SummonCoolTime = 72;  //72
    int CurrentSummonCoolTime;
    int BoneCoolTime = 15;
    int CurrentBoneCoolTime;
    int Change2DCoolTime = 40;
    int CurrentChange2DCoolTime;

    public GameObject weapon;
    public GameObject shield;
    public GameObject bone;
    public GameObject Mini;
    public GameObject Change2D;
    public GameObject Change2DAtk;

    public GameObject BossHpBar;

    public GameObject MiniSummonEffect;

    public GameObject OpenDoor;
    public GameObject CloseDoor;

    Transform target;  //타켓

    CharacterController cc;
    Animator _ani;
    Transform tr;

    public Transform SummonPoint;
    public Transform BoneSpawnPoint1;
    public Transform BoneSpawnPoint2;
    public Transform BoneSpawnPoint3;
    public Transform BoneSpawnPoint4;
    public Transform Change2DPoint;
    public Transform Change2DAtkPoint;

    public static SkeletonEnemy instance;

    private void Awake()
    {
        BossHpBar.GetComponent<EnergyBar>().SetValueMax(maxHp);
        BossHpBar.GetComponent<EnergyBar>().SetValueMin(0);
        BossHpBar.GetComponent<EnergyBar>().SetValueCurrent(hp);
        instance = this;
        cc = GetComponent<CharacterController>();
        tr = GetComponent<Transform>();
        _ani = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }
    // Start is called before the first frame update
    void Start()
    {
        CurrentSummonCoolTime = 0;
        CurrentBoneCoolTime = 0;
        CurrentChange2DCoolTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(isDie)
        {
            OpenDoor.SetActive(false);
            CloseDoor.SetActive(true);
        }

        if (is2DChangeHit)
        {
            is2DChangeHit = false;
            Invoke("ChangeAtkDelay", 0.5f);
        }

        if(isFight)
        {
            BossHpBar.SetActive(true);

            traceRange = 200;
            if (CurrentSummonCoolTime < SummonCoolTime)
            {
                StartCoroutine(SummonDelay());
            }
            else
            {
                isCanSummon = true;
            }
            if (CurrentBoneCoolTime < BoneCoolTime)
            {
                StartCoroutine(BoneDelay());
            }
            else
            {
                isCanBone = true;
            }
            if (CurrentChange2DCoolTime < Change2DCoolTime)
            {
                StartCoroutine(Change2DDelay());
            }
            else
            {
                isCanChnage2D = true;
                attackRange = 12;
            }
        }
        


        switch (enemystate)
        {
            case ENEMYSTATE.IDLE:
                {
                    weapon.GetComponent<BoxCollider>().enabled = false; // 칼 비활성화

                    _ani.SetBool("isIdle", true);
                    _ani.SetBool("isRun", false);
                    _ani.SetBool("isAtk", false);
                    _ani.SetBool("isSummonAni", false);
                    _ani.SetBool("isBoneAni", false);

                    SkeletonFirstSnd();



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
                    _ani.SetBool("isIdle", false);
                    _ani.SetBool("isRun", true);
                    _ani.SetBool("isAtk", false);
                    _ani.SetBool("isSummonAni", false);
                    _ani.SetBool("isBoneAni", false);


                    float distance = Vector3.Distance(target.position, tr.position);

                    Vector3 dir = target.position - tr.position;
                    dir.y = 0.0f; //점프하면서 따라오는거 방지
                    dir.Normalize();  //거리 평준화 함수 ( 플레이어한테 부드럽게 따라옴 ) 

                    isFight = true;

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
                    isHit = true;

                    _ani.SetBool("isIdle", false);
                    _ani.SetBool("isRun", false);



                    if(isCanSummon)
                    {
                        CurrentSummonCoolTime = 0;
                        isCanSummon = false;

                        _ani.SetBool("isSummonAni", true);

                        Invoke("SummonSummonSummon", 0.3f);
                    }
                    if (isCanChnage2D && isCanBone)
                    {
                        isCanChnage2D = false;
                        isCanBone = false;
                        CurrentChange2DCoolTime = 0;
                        CurrentBoneCoolTime = 0;
                        Change2DSkill();
                        SoundManager.instance.PlaySfx(transform.position, SoundManager.instance.D2AtkFirst, 0, SoundManager.instance.sfxVolum);
                        attackRange = 6;
                    }
                    else if (isCanChnage2D)
                    {
                        isCanChnage2D = false;
                        CurrentChange2DCoolTime = 0;
                        Change2DSkill();
                        SoundManager.instance.PlaySfx(transform.position, SoundManager.instance.D2AtkFirst, 0, SoundManager.instance.sfxVolum);
                        attackRange = 6;
                    }
                    else if (isCanBone)
                    {
                        CurrentBoneCoolTime = 0;
                        isCanBone = false;

                        cnt++;

                        _ani.SetBool("isBoneAni", true);

                        Invoke("Bone", 0.2f);
                    }
                    

                    else
                    {
                        if (isHit)
                        {
                            _ani.SetBool("isAtk", true);
                            
                        }
                        else if (!isHit)
                        {
                            _ani.SetBool("isAtk", false);
                        }
                    }

                   

                    if (_ani.GetCurrentAnimatorStateInfo(0).IsName("Bone"))
                    {
                        Invoke("ShieldDelay", 0.1f);
                       
                    }
                    else shield.GetComponent<BoxCollider>().enabled = false;


                    float distance = Vector3.Distance(target.position, tr.position);
                    Vector3 dir = target.position - tr.position;

                    dir.y = 0.0f;
                    dir.Normalize();

                    tr.rotation = Quaternion.Lerp(tr.rotation, Quaternion.LookRotation(dir), rotSpeed * Time.deltaTime);

                    if (distance > attackRange)
                    {
                        isHit = false;
                        Invoke("IdleState", 0.27f);
                    }

                    break;
                }
            case ENEMYSTATE.DAMAGE:
                {
                    float distance = Vector3.Distance(target.position, tr.position);

                    BasicKnife.instance.cnt++;

                    BossHpBar.GetComponent<EnergyBar>().SetValueMax(maxHp);
                    BossHpBar.GetComponent<EnergyBar>().SetValueMin(0);
                    BossHpBar.GetComponent<EnergyBar>().SetValueCurrent(hp);

                    _ani.SetBool("isIdle", false);
                    _ani.SetBool("isRun", false);
                    _ani.SetBool("isAtk", false);
                    _ani.SetBool("isSummonAni", false);
                    _ani.SetBool("isBoneAni", false);

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
                    if(!isDie)
                    {
                        isDie = true;
                        Debug.Log("죽음");
                        _ani.SetBool("isIdle", false);
                        _ani.SetBool("isRun", false);
                        _ani.SetBool("isAtk", false);
                        _ani.SetBool("isSummonAni", false);

                        shield.GetComponent<BoxCollider>().enabled = false;
                        weapon.GetComponent<BoxCollider>().enabled = false;
                        this.GetComponent<CharacterController>().enabled = false;

                        GameManager.instance.ExpCal(6000);
                        _ani.SetBool("isDie", true);
                        SoundManager.instance.PlaySfx(tr.position, SoundManager.instance.BossSkeletonDie, 0, SoundManager.instance.sfxVolum);
                    }
                    
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
        CanHit = false;

        SoundManager.instance.PlaySfx(tr.position, SoundManager.instance.BossSkeletonDamage, 0, SoundManager.instance.sfxVolum);

        hp = hp - GameManager.instance.Damage;
        Debug.Log("적체력: " + hp);
        enemystate = ENEMYSTATE.DAMAGE;
        _ani.SetTrigger("hit");


        enemystate = ENEMYSTATE.DAMAGE;

        Invoke("ChangeCanHit", 1.1f);
    }
    void ChangeCanHit()
    {
        CanHit = true;
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
        if (_ani.GetBool("isIdle"))  //모션 검사
        {
            _ani.SetBool("isIdle", false);
            _ani.SetBool("isRun", true);
            //this.GetComponent<SWS.splineMove>().Resume();  //웨이포인트 재가동
        }
    }

    void SummonMini()
    {
        GameObject Skeleton = Instantiate(Mini);

        float x = Random.Range(this.tr.position.x - 6, tr.position.x + 6);

        float z = Random.Range(this.tr.position.z - 6, tr.position.z + 6);

        Skeleton.transform.position = new Vector3(SummonPoint.position.x - 3 + cnt*3,SummonPoint.position.y, SummonPoint.position.z - 3 + cnt * 3);

        Effect[cnt] = Instantiate(MiniSummonEffect, Skeleton.transform.position, Skeleton.transform.rotation);
        ++cnt;


    }

    IEnumerator SummonDelay()
    {
        if(!isSummon)
        {
            isSummon = true;

            CurrentSummonCoolTime++;

            yield return new WaitForSeconds(1.0f);

            isSummon = false;

        }
    }

    IEnumerator BoneDelay()
    {
        if (!isBone)
        {
            isBone = true;

            CurrentBoneCoolTime++;

            yield return new WaitForSeconds(1.0f);

            isBone = false;

        }
    }

    IEnumerator Change2DDelay()
    {
        if (!isChange2D)
        {
            isChange2D = true;

            CurrentChange2DCoolTime++;

            yield return new WaitForSeconds(1.0f);

            isChange2D = false;

        }
    }

    void ShieldDelay()
    {
        shield.GetComponent<BoxCollider>().enabled = true;
    }

    void Bone()
    {
        SoundManager.instance.PlaySfx(transform.position, SoundManager.instance.Sheild, 0, SoundManager.instance.sfxVolum);
        Bone1();
        Bone2();
        Bone3();
        Bone4();
    }

    void SummonSummonSummon()
    {
        SummonMini();
        SummonMini();
        SummonMini();
        SoundManager.instance.PlaySfx(transform.position, SoundManager.instance.Summon, 0, SoundManager.instance.sfxVolum);
        Invoke("Delay", 5.0f);
    }

    void Delay()
    {
        cnt = 0;
        for (int i = 0; i <= 2; i++)
        {
            Destroy(Effect[i]);
            Effect[i] = null;
        }
    }

    void Change2DSkill()
    {
        Instantiate(Change2D, Change2DPoint.position, Change2DPoint.rotation);
    }
    
    void Bone1()
    {
        GameObject Bone = Instantiate(bone, BoneSpawnPoint1.position, BoneSpawnPoint1.rotation);

        Bone.GetComponent<Rigidbody>().AddForce(bone.transform.forward * BoneSpeed);
        Destroy(Bone, 3.0f);
    }
    void Bone2()
    {
        GameObject Bone = Instantiate(bone, BoneSpawnPoint2.position, BoneSpawnPoint2.rotation);

        Bone.GetComponent<Rigidbody>().AddForce(-bone.transform.forward * BoneSpeed);
        Destroy(Bone, 3.0f);
    }
    void Bone3()
    {
        GameObject Bone = Instantiate(bone, BoneSpawnPoint3.position, BoneSpawnPoint3.rotation);

        Bone.GetComponent<Rigidbody>().AddForce(bone.transform.right * BoneSpeed);
        Destroy(Bone, 3.0f);
    }
    void Bone4()
    {
        GameObject Bone = Instantiate(bone, BoneSpawnPoint4.position, BoneSpawnPoint4.rotation);

        Bone.GetComponent<Rigidbody>().AddForce(-bone.transform.right * BoneSpeed);
        Destroy(Bone, 3.0f);
    }

    void ChangeAtkDelay()
    {
        Instantiate(Change2DAtk, Change2DAtkPoint.position, Change2DAtkPoint.rotation);
    }

    IEnumerator SkeletonFirstSnd()
    {
        if (!isSMoveSnd)
        {
            isSMoveSnd = true;
            SoundManager.instance.PlaySfx(transform.position, SoundManager.instance.BossSkeletonWalk, 0, SoundManager.instance.sfxVolum * 0.5f);

            yield return new WaitForSeconds(10.0f);
            isSMoveSnd = false;
        }

    }

    IEnumerator SkeletonHitSnd()
    {
        if (!isSHitSnd)
        {
            isSHitSnd = true;

            SoundManager.instance.PlaySfx(transform.position, SoundManager.instance.BossSkeletonHit, 0, SoundManager.instance.sfxVolum);


            yield return new WaitForSeconds(1.0f);

            isSHitSnd = false;

        }
    }


    void OnCollider()
    {
        weapon.GetComponent<BoxCollider>().enabled = true;
    }
    void OffCollider()
    {
        weapon.GetComponent<BoxCollider>().enabled = false;
    }


}
