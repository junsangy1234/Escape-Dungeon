using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPlayerState : MonoBehaviour
{
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

    public static PlayerState instance;
    Animator _ani;


    private void Awake()
    {
        _ani = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (playerState)
        {
            case PLAYERSTATE.AWAKE:
                {
                    PlayerAwkae();
                    break;
                }
            case PLAYERSTATE.IDEL:
                {
                        _ani.SetBool("isIdle", true);
                        _ani.SetBool("isRun", false);
                        _ani.SetBool("isHitLeft", false);
                        _ani.SetBool("isHitRight", false);
                        _ani.SetBool("isShield", false);

                    if (Input.GetKey("a") || Input.GetKey("d"))
                    {
                        playerState = PLAYERSTATE.MOVE;
                    }
                    else if ((Input.GetKey("w") || Input.GetKey("s")))
                    {
                        playerState = PLAYERSTATE.MOVE;
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

                    if (!Input.GetKey("w") && !Input.GetKey("a") && !Input.GetKey("s") && !Input.GetKey("d"))
                    {
                        playerState = PLAYERSTATE.IDEL;
                    }

                    break;
                }
            case PLAYERSTATE.ATTACK:
                {
                    break;
                }
            case PLAYERSTATE.DAMAGE:
                {
                    break;
                }
            case PLAYERSTATE.DEAD:
                {
                   
                    break;
                }
        }





    }

    //준비 단계 함수
    void PlayerAwkae()
    {
        playerState = PLAYERSTATE.IDEL;
    }
}
