using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MonoBehaviour
{

    public int cnt = 0;

    Animator _ani;


    public Transform front;
    public GameObject weapon;
    public static Spear instance;
   

    public bool isSpear = false;

    private void Awake()
    {
        instance = this;
        weapon.GetComponent<BoxCollider>().enabled = false;
        _ani = GetComponent<Animator>();
    }
    private void Start()
    {
        isSpear = true;
        BasicKnife.instance.isBasicKnife = false;
    }

    // Update is called once per frame
    void Update()
    {
        SpearHit();
    }

    void SpearHit()
    {

        if (Input.GetMouseButton(0) && !Animationlng() && Move3D.instance.moveSpeed != 0 && Move2D.instance.moveSpeed != 0)
        {
            Invoke("Delay3", 1.5f);
            Invoke("Delay2", 0.5f);
            Invoke("Delay", 0.8f);
            if(cnt % 3 == 0)
            {
                _ani.SetBool("isCombo", true);  
            }
            else
            {
                _ani.SetBool("isThrust", true);
            }
            PlayerState.instance.isAtk = true;
            Move3D.instance.moveSpeed = 7.0f;
            Move2D.instance.moveSpeed = 7.0f;
        }

        else
        {
            _ani.SetBool("isThrust", false);
            _ani.SetBool("isCombo", false);
            PlayerState.instance.isAtk = false;
        }
    }

    void Delay()
    {
        weapon.GetComponent<BoxCollider>().enabled = false;
    }
    void Delay2()
    {
        if (Animationlng())
            weapon.GetComponent<BoxCollider>().enabled = true;
    }
    void Delay3()
    {
        Move3D.instance.moveSpeed = 10.0f;
    }

    bool Animationlng()
    {
        //Debug.Log(_ani.GetCurrentAnimatorStateInfo(0).normalizedTime);
        return _ani.GetCurrentAnimatorStateInfo(0).IsName("Atk") || _ani.GetCurrentAnimatorStateInfo(0).IsName("Spear_2Trusts_Uppercut_onPlace 0");
    }
}
