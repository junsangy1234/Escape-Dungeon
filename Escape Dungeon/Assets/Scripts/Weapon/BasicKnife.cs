using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicKnife : MonoBehaviour
{

    public int cnt = 0;

    Animator _ani;
    Transform tr;

    public Transform front;
    public GameObject weapon;
    public static BasicKnife instance;

    public bool isBasicKnife = false;
    bool isCanSwingSnd = false;

    private void Awake()
    {
        instance = this;
        weapon.GetComponent<BoxCollider>().enabled = false; 
        _ani = GetComponent<Animator>();
        tr = GetComponent<Transform>();

        cnt = 0;
    }

    private void Start()
    {
        Delay3();
           isBasicKnife = true;
        Spear.instance.isSpear = false;
    }

    // Update is called once per frame
    void Update()
    {
        BasicKnifeHit();
    }

    void BasicKnifeHit()
    {


        if ((Input.GetMouseButton(0) && cnt % 2 != 0) && !Animationlng() && Move3D.instance.moveSpeed != 0 && Move2D.instance.moveSpeed != 0)
        {
            //Invoke("Delay3", 1.1f);
            //Invoke("Delay2", 0.5f);
            //Invoke("Delay", 0.8f);
            _ani.SetBool("isHitLeft", true);
            //StartCoroutine(SwingSnd());
            PlayerState.instance.isAtk = true;
            Move3D.instance.moveSpeed = 4.0f;
            Move2D.instance.moveSpeed = 4.0f;
        }
        else if ((Input.GetMouseButton(0) && cnt % 2 == 0) && !Animationlng() && Move3D.instance.moveSpeed != 0 && Move2D.instance.moveSpeed != 0)   
        {
            //Invoke("Delay3", 1.1f);
            //Invoke("Delay2", 0.2f);
            //Invoke("Delay", 0.8f);
            _ani.SetBool("isHitRight", true);
            //StartCoroutine(SwingSnd());
            PlayerState.instance.isAtk = true;
            Move3D.instance.moveSpeed = 4.0f;
            Move2D.instance.moveSpeed = 4.0f;
        }
        else
        {
            _ani.SetBool("isHitLeft", false);
            _ani.SetBool("isHitRight", false);
            PlayerState.instance.isAtk = false;
        }
    }

    void Delay()
    {
        weapon.GetComponent<BoxCollider>().enabled = false;
    }
    void Delay2()
    {
        //if(Animationlng())
            weapon.GetComponent<BoxCollider>().enabled = true;
    }
    public void Delay3()
    {
        Move3D.instance.moveSpeed = 10.0f;
        Move2D.instance.moveSpeed = 10.0f;
    }

    bool Animationlng()
    {
        //Debug.Log(_ani.GetCurrentAnimatorStateInfo(0).normalizedTime);
        return _ani.GetCurrentAnimatorStateInfo(0).IsName("1H_sword_swing_high_left") || _ani.GetCurrentAnimatorStateInfo(0).IsName("1H_sword_swing_high_right") || _ani.GetCurrentAnimatorStateInfo(0).IsName("1H_walk_left_swing") || _ani.GetCurrentAnimatorStateInfo(0).IsName("1H_walk_right_swing");
    }

    void SwingSnd()
    {
        SoundManager.instance.PlaySfx(tr.position, SoundManager.instance.Swing, 0, SoundManager.instance.sfxVolum);
    }

   
}
