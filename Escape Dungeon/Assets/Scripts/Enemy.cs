using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Enemy : MonoBehaviour
{
    public enum ENEMYSTATE
    {
        IDLE = 0,
        MOVE,
        ATTACK,
        DAMAGE,
        DEATH
    }
    ENEMYSTATE enemystate = ENEMYSTATE.IDLE;

    Vector3 currentVelocty; //현재 위치

    public int GetExp = 10;


    public int hp = 10;
    public int maxHp = 10;
    public int Damage;
    public float speed = 10.0f;
    public float rotSpeed = 10.0f;
    public float attackRange = 2.5f; //공격 범위
    public float traceRange = 10.0f; //추적 범위

    public bool isCanUpCnt = true;

    bool LastIs3D = true;

    bool CanHit = true;
    bool isHit = false;
    bool isAtk = false;
    bool isCanAtk = true;
    bool isAtkSnd = false;
    bool isFirstSnd = false;
    bool isSMoveSnd = false;

    public GameObject weapon;
    public GameObject Goblin;

    Transform target;  //타켓


    CharacterController cc;
    Animator _ani;
    Transform tr;

    public static Enemy instance;

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
        switch (enemystate)
        {
            case ENEMYSTATE.IDLE:
                {
                    _ani.SetBool("isIdle", true);
                    _ani.SetBool("isRun", false);
                    _ani.SetBool("isAtk", false);

                    if (this.gameObject.tag == "Goblin") StartCoroutine(GoblinFirstSnd());
                    if (this.gameObject.tag == "Skeleton") StartCoroutine(SkeletonFirstSnd());

                    isCanAtk = true;

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
                    isCanAtk = true;
                    if (isAtk) traceRange = 40;

                    if (this.gameObject.tag == "Skeleton") Invoke("AtkDelay", 0.5f);

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
                    isHit = true;

                    _ani.SetBool("isIdle", false);
                    _ani.SetBool("isRun", false);
                    if (isHit && isCanAtk)
                    {
                        isCanAtk = false;
                        _ani.SetBool("isAtk", true);
                        speed = 0;
                        rotSpeed = 0;
                    }
                    else if (!isHit)
                    {
                        _ani.SetBool("isAtk", false);
                        isCanAtk = true;
                    }

                    
                    


                    float distance = Vector3.Distance(target.position, tr.position);
                    Vector3 dir = target.position - tr.position;

                    dir.y = 0.0f;
                    dir.Normalize();

                    tr.rotation = Quaternion.Lerp(tr.rotation, Quaternion.LookRotation(dir), rotSpeed * Time.deltaTime);

                    if (distance > attackRange)
                    {
                        isCanAtk = true;
                        isHit = false;
                        isAtk = true;
                        Invoke("IdleState", 0.27f);
                    }

                    break;
                }
            case ENEMYSTATE.DAMAGE:
                {
                    float distance = Vector3.Distance(target.position, tr.position);

                    isCanAtk = true;
                    _ani.SetBool("isIdle", false);
                    _ani.SetBool("isRun", false);
                    _ani.SetBool("isAtk", false);

                    BasicKnife.instance.cnt++;


                    if ( hp <= 0 )
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
                    if (this.gameObject.tag == "Goblin") SoundManager.instance.PlaySfx(tr.position, SoundManager.instance.GoblinDeath, 0, SoundManager.instance.sfxVolum);
                    if (this.gameObject.tag == "Skeleton") SoundManager.instance.PlaySfx(tr.position, SoundManager.instance.SkeletonDie, 0, SoundManager.instance.sfxVolum * 0.6f);
                    GameManager.instance.ExpCal(GetExp);
                    Destroy(gameObject);
                    EnemyDeath();
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

        if (this.gameObject.tag == "Goblin")
        {
            SoundManager.instance.PlaySfx(tr.position, SoundManager.instance.GoblinHit, 0, SoundManager.instance.sfxVolum);
            SoundManager.instance.PlaySfx(tr.position, SoundManager.instance.GoblinHit2, 0, SoundManager.instance.sfxVolum);
        }
        if (this.gameObject.tag == "Skeleton")
        {
            SoundManager.instance.PlaySfx(tr.position, SoundManager.instance.SkeletonDamage, 0, SoundManager.instance.sfxVolum);
        }
        CanHit = false;
        hp = hp - GameManager.instance.Damage;


        _ani.SetTrigger("hit");
        OffCollider();

        enemystate = ENEMYSTATE.DAMAGE;



        Invoke("ChangeCanHit", 1.1f);
    }

    public void EnemyDeath()
    {
        if(isCanUpCnt)
        {
            ClearStage.instance.killCnt++;
        }
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


    void AtkDelay()
    {
        speed = 8;
        rotSpeed = 10;
    }

    void OnCollider()
    {
        weapon.GetComponent<BoxCollider>().enabled = true;
    }

    void OffCollider()
    {
        weapon.GetComponent<BoxCollider>().enabled = false;
    }

    void ChangeCanHit()
    {
        CanHit = true;
    }

    void ChangeTo3D()
    {
        if(LastIs3D != ChangeCamera.instance.Is3D)
        {
            LastIs3D = ChangeCamera.instance.Is3D;
            if (LastIs3D)
            {
                Goblin.GetComponent<CharacterController>().enabled = false;
                Goblin.AddComponent<Rigidbody>();
                Goblin.GetComponent<CapsuleCollider>().enabled = true;
                Invoke("OffRigidbody", 0.2f);
            }
        }
    }

    void OffRigidbody()
    {
        Goblin.GetComponent<CharacterController>().enabled = true;
        Goblin.GetComponent<CapsuleCollider>().enabled = false;
        Destroy(Goblin.GetComponent<Rigidbody>());
    }

    void GoblinAtkSnd()
    {
        SoundManager.instance.PlaySfx(transform.position, SoundManager.instance.GoblinAtk, 0, SoundManager.instance.sfxVolum);
    }

    void SkeletonAtkSnd()
    {
        SoundManager.instance.PlaySfx(transform.position, SoundManager.instance.SkeletonHit, 0, SoundManager.instance.sfxVolum);
    }

    //IEnumerator GoblinAtkSnd()
    //{
    //    if (!isAtkSnd)
    //    {
    //        isAtkSnd = true;
    //        SoundManager.instance.PlaySfx(transform.position, SoundManager.instance.GoblinAtk, 0, SoundManager.instance.sfxVolum);

    //        yield return new WaitForSeconds(1.0f);
    //        isAtkSnd = false;
    //    }

    //}

    IEnumerator GoblinFirstSnd()
    {
        if (!isFirstSnd)
        {
            isFirstSnd = true;
            SoundManager.instance.PlaySfx(transform.position, SoundManager.instance.GoblinIdle, 0, SoundManager.instance.sfxVolum * 0.5f);

            yield return new WaitForSeconds(1000000000.0f);
            isFirstSnd = false;
        }

    }
    IEnumerator SkeletonFirstSnd()
    {
        if (!isSMoveSnd)
        {
            isSMoveSnd = true;
            SoundManager.instance.PlaySfx(transform.position, SoundManager.instance.SkeletonWalk, 0, SoundManager.instance.sfxVolum * 0.5f);

            yield return new WaitForSeconds(1000000000.0f);
            isSMoveSnd = false;
        }

    }
    

}
