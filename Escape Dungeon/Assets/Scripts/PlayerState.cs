using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PlayerState : MonoBehaviour
{
    Transform tr;
    public bool Is3D = true;
    bool isDie = false;
    public ChangeCamera ChangecamSc;
    public GameObject Veiwmanager;

    public Camera ShakeCamera;

    public Text DieT;
    public Text DieR;

    //유한 상태기계 (FSM) 설정
    public enum PLAYERSTATE
    {
        AWAKE = 0, //준비
        IDEL,  //대기
        MOVE,  //이동
        ATTACK, //공격중
        DAMAGE, //피격 받았을때
        DEAD //캐릭터 죽음
    }
    public PLAYERSTATE playerState = PLAYERSTATE.AWAKE;

    public bool isAtk = false;

    public RuntimeAnimatorController BasicKnifeAnimator;
    public RuntimeAnimatorController SpearAnimator;

    public Transform EffectCreate;

    public static PlayerState instance;
    Animator _ani;


    private void Awake()
    {
        _ani = GetComponent<Animator>();
        instance = this;
        tr = GetComponent<Transform>();

        ChangecamSc = Veiwmanager.GetComponent<ChangeCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerState == PLAYERSTATE.DEAD)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene("Stage 1");
            }
        }

        if(GameManager.instance.PlayerHp <= 0)
        {
            playerState = PLAYERSTATE.DEAD;
        }

        if (BasicKnife.instance.isBasicKnife)
        {
            _ani.runtimeAnimatorController = BasicKnifeAnimator as RuntimeAnimatorController;
        }
        else if(Spear.instance.isSpear)
        {
            _ani.runtimeAnimatorController = SpearAnimator as RuntimeAnimatorController;
        }
        Is3D  = ChangecamSc.Is3D;
        switch (playerState)
        {
            case PLAYERSTATE.AWAKE:
                {
                    PlayerAwkae();
                    break;
                }
            case PLAYERSTATE.IDEL:
                {
                    if(Spear.instance.isSpear)
                    {
                        _ani.SetBool("isThrust", false);
                        _ani.SetBool("isCombo", false);
                        _ani.SetBool("isShield", false);
                        _ani.SetBool("isIdle", true);
                        _ani.SetBool("isRun", false);
                    }
                    else if(BasicKnife.instance.isBasicKnife)
                    {
                        _ani.SetBool("isIdle", true);
                        _ani.SetBool("isRun", false);
                        _ani.SetBool("isHitLeft", false);
                        _ani.SetBool("isHitRight", false);
                        _ani.SetBool("isShield", false);
                    }

                    if (Input.GetKey("a") || Input.GetKey("d"))
                    {
                        playerState = PLAYERSTATE.MOVE;
                    }
                    else if((Input.GetKey("w") || Input.GetKey("s") ) && Is3D)
                    {
                        playerState = PLAYERSTATE.MOVE;
                    }
                    else if (isAtk)
                    {
                        playerState = PLAYERSTATE.ATTACK;
                    }
                    else
                    {
                        playerState = PLAYERSTATE.IDEL;
                    }
                    break;
                }
            case PLAYERSTATE.MOVE:
                {
                    _ani.SetBool("isIdle", false);
                    _ani.SetBool("isRun", true);
                    _ani.SetBool("isShield", false);
                    _ani.SetBool("isHitLeft", false);
                    _ani.SetBool("isHitRight", false);


                    if (isAtk)
                    { 
                        playerState = PLAYERSTATE.ATTACK;
                    }
                    else if (!Input.GetKey("w") && !Input.GetKey("a") && !Input.GetKey("s") && !Input.GetKey("d"))
                    {
                        playerState = PLAYERSTATE.IDEL;
                    }
                    else if((!Input.GetKey("a") && !Input.GetKey("d"))&&!Is3D)
                    {
                        playerState = PLAYERSTATE.IDEL;
                    }
                    break;
                }
            case PLAYERSTATE.ATTACK:
                {
                    _ani.SetBool("isIdle", false);
                    _ani.SetBool("isRun", false);
                    _ani.SetBool("isShield", false);


                    if (!isAtk) playerState = PLAYERSTATE.IDEL;
                    break;
                }
            case PLAYERSTATE.DAMAGE:
                {
                    _ani.SetBool("isIdle", false);
                    _ani.SetBool("isRun", false);
                    _ani.SetTrigger("hit");


                    if (ChangeCamera.instance.Is3D)
                    {
                        ShakeCamera.gameObject.SetActive(true);
                        EZCameraShake.CameraShaker.Instance.ShakeOnce(3.0f, 3.0f, 0.0f, 0.5f);
                        Invoke("OnDamage", 1.0f);
                    }
                    


                    //EZCameraShake.CameraShaker.Instance.Shake(EZCameraShake.CameraShakePresets.Bump); //충돌
                    //EZCameraShake.CameraShaker.Instance.Shake(EZCameraShake.CameraShakePresets.BadTrip); //배 흔들림
                    //EZCameraShake.CameraShaker.Instance.Shake(EZCameraShake.CameraShakePresets.Earthquake); //지진



                    if (isAtk)
                    {
                        playerState = PLAYERSTATE.ATTACK;
                    }
                    else if (Input.GetKey("a") ||Input.GetKey("d"))
                    {
                        playerState = PLAYERSTATE.MOVE;
                    }
                    else if((Input.GetKey("w") || Input.GetKey("s")) && Is3D)
                    {
                        playerState = PLAYERSTATE.MOVE;
                    }
                    else if(GameManager.instance.PlayerHp <= 0)
                    {
                        playerState = PLAYERSTATE.DEAD;
                    }
                    else
                    {
                        playerState = PLAYERSTATE.IDEL;
                    }
                    break;
                }
            case PLAYERSTATE.DEAD:
                {
                    if(!isDie)
                    {
                        isDie = true;
                        _ani.SetBool("isRun", false);
                        _ani.SetBool("isShield", false);
                        _ani.SetBool("isHitLeft", false);
                        _ani.SetBool("isHitRight", false);
                        _ani.SetBool("isIdle", false);
                        _ani.SetBool("isDie", true);
                        this.GetComponent<CharacterController>().enabled = false;
                        SoundManager.instance.sfxVolum = 0;
                        Destroy(GameObject.Find("BGM"));
                        SoundManager.instance.bgmVolum = 1.0f;
                        SoundManager.instance.PlayBGM(SoundManager.instance.DeadBgm, 0, true);
                        StartCoroutine(TextFade());
                        DieR.color = new Vector4(1, 1, 0, 1);
                    }
                    
                    break;
                }
        }


        

        
    }
    
    //준비 단계 함수
    void PlayerAwkae()
    {
        playerState = PLAYERSTATE.IDEL;
    }


    void OnDamage()
    {
        ShakeCamera.gameObject.SetActive(false);
    }

    IEnumerator TextFade()
    {

        for (float i = 1f; i >= -0.1f; i -= 0.001f)
        {
            Color color = new Vector4(1, 1, 1, i);
            DieT.color = color;

            yield return new WaitForEndOfFrame();
        }


    }

}//end class
