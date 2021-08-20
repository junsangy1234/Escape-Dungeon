using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldColliderChk : MonoBehaviour
{
    public GameObject Player;
    public GameObject Skeleton;

    float KnockbackPower = 20;
    private void Awake()
    {
        Player = GameObject.Find("Player");
    }
    private void Start()
    {
        Player.GetComponent<CapsuleCollider>().enabled = false;
        Destroy(Player.GetComponent<Rigidbody>());
    }
    private void Update()
    {
        //if(Player.transform.position.x > 12)
        //{
        //    Player.transform.position = new Vector3(11, Player.transform.position.y, transform.position.z);
        //}
        //else if(Player.transform.position.x < -20)
        //{
        //    Player.transform.position = new Vector3(-19, Player.transform.position.y, transform.position.z);
        //}


        if (!PlayerState.instance.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("1H_Knocked_backward_Fly"))
        {
            if(ChangeCamera.instance.Is3D)
            {
                Player.GetComponent<Move3D>().enabled = true;
                Player.GetComponent<Move2D>().enabled = false;
            }
            else
            {
                Player.GetComponent<Move3D>().enabled = false;
                Player.GetComponent<Move2D>().enabled = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.layer)
        {
            case 8:
                { 
                    Move3D.instance.moveSpeed = 0;
                    Move2D.instance.moveSpeed = 0;

                    Player.GetComponent<CharacterController>().enabled = false;
                    Player.GetComponent<Move3D>().enabled = false;
                    Player.GetComponent<Move2D>().enabled = false;

                    Invoke("Knockback", 0.1f);
                    break;
                }
        }

    }

    void Knockback()
    {
        Player.GetComponent<Animator>().SetBool("isIdle", false);
        Player.GetComponent<Animator>().SetBool("isRun", false);
        Player.GetComponent<Animator>().SetBool("isHitLeft", false);
        Player.GetComponent<Animator>().SetBool("isHitRight", false);
        Invoke("SheildAni", 0.15f);

        Skeleton.layer = 14;

        Player.AddComponent<Rigidbody>();
        Player.GetComponent<CapsuleCollider>().enabled = true;

        Player.GetComponent<Rigidbody>().velocity = new Vector3(
        Player.GetComponent<Rigidbody>().velocity.x, 0, Player.GetComponent<Rigidbody>().velocity.z);

        Player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        Player.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        Player.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Extrapolate;
        Player.transform.Translate(Vector3.back * KnockbackPower);
        Invoke("Delay", 3.0f);
    }

    void Delay()
    {
        Skeleton.layer = 9;
        Player.GetComponent<CharacterController>().enabled = true;
        Player.GetComponent<CapsuleCollider>().enabled = false;
        Move3D.instance.moveSpeed = 10.0f;
        Move2D.instance.moveSpeed = 10.0f;

        Destroy(Player.GetComponent<Rigidbody>());
    }

    void SheildAni()
    {
        Player.GetComponent<Animator>().SetBool("isShield", true);
    }
}