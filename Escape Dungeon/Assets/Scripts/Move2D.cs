using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Move2D : MonoBehaviour
{
    CharacterController cc;
    Animator _ani;

    public GameObject Player;

    public float moveSpeed = 10;
    public float jumpForce = 13;
    public float gravity = 30.0f;
    public float yVelocty = 0;


    public float x;
    float lastx = 1;
    public Transform cameraPos;

    public Camera Camera2D;
    Vector3 moveDir = Vector3.zero;
    public int FrontBackView = 0;

    public static Move2D instance;


    float Cam2D_Pos_X = 0;
    private void Awake()
    {
        instance = this;

        //yVelocty = GameManager.instance.LastY_Velocty;
        yVelocty = 0;

        cc = GetComponent<CharacterController>();
        _ani = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        cc.transform.forward = new Vector3(0, 0, 1);
        FrontBackView = 0;
        
    }
    
    // Update is called once per frame
    void Update()
    {
        ViewInput();
        MoveStart2D();
        Jump2D();
        
        
    }

    void MoveStart2D()
    {
        x = Input.GetAxis("Horizontal");

        moveDir.x = 0;
        moveDir.z = Mathf.Abs( x);
        moveDir.y = 0;

        moveDir *= moveSpeed;

        moveDir = this.transform.TransformDirection(moveDir);

        

        yVelocty -= gravity * Time.deltaTime;
        moveDir.y = yVelocty;

        cc.Move(moveDir * Time.deltaTime);
        if (cc.isGrounded) yVelocty = 0;
        if (!Animationlng())
        {


            if (moveDir.z != 0)
            {
                //cc.transform.forward = Vector3.Lerp(new Vector3(0, 0, cc.transform.forward.z), new Vector3(0, 0, -x),Time.deltaTime * 5f);
                cc.transform.forward = new Vector3(0, 0, -x);
                lastx = -x;
            }
            else
            {

                //cc.transform.forward = Vector3.Lerp(new Vector3(0, 0, cc.transform.forward.z), new Vector3(0, 0, lastx), Time.deltaTime * 5f);
                cc.transform.forward = new Vector3(0, 0, lastx);
            }
        }

       
        cc.enabled = false;
        Player.transform.position = new Vector3(0, cc.transform.position.y, cc.transform.position.z);
        cc.enabled = true;
        
        

    }

    void Jump2D()
    {
        if (cc.isGrounded)
        {
            if (Input.GetButtonDown("Jump"))   //"Jump" = 스페이스바
            {
                yVelocty = jumpForce;
                cc.Move(new Vector3(0, 0.5f, 0));
            }
        }
    }

    private void LateUpdate()
    {

        if(GameManager.instance.InRoomORHall == 0)
        {
            Cam2D_Pos_X = -17.5f;
        }
        else
        {
            Cam2D_Pos_X = -6;
        }
        if(FrontBackView == 0)
        {
            Camera2D.transform.position = Vector3.Lerp(Camera2D.transform.position, new Vector3(Cam2D_Pos_X, transform.position.y + 5f, cc.transform.position.z), Time.deltaTime * 5);
            Camera2D.transform.rotation = Quaternion.Lerp(Camera2D.transform.rotation, Quaternion.Euler(0, 90, 0), Time.deltaTime * 5);
        }
        else if(FrontBackView == 1)
        {
            Camera2D.transform.position = Vector3.Lerp(Camera2D.transform.position, new Vector3(Cam2D_Pos_X, transform.position.y + 5f, cc.transform.position.z-5), Time.deltaTime * 5);
            Camera2D.transform.rotation = Quaternion.Lerp(Camera2D.transform.rotation, Quaternion.Euler(0, 90, 0), Time.deltaTime * 5);
        }
        else
        {
            Camera2D.transform.position = Vector3.Lerp(Camera2D.transform.position, new Vector3(Cam2D_Pos_X, transform.position.y + 5f, cc.transform.position.z+5), Time.deltaTime * 5);
            Camera2D.transform.rotation = Quaternion.Lerp(Camera2D.transform.rotation, Quaternion.Euler(0, 90, 0), Time.deltaTime * 5);
        }
    
    }

    void ViewInput()
    {
        if (Input.GetButton("FrontView"))
        {
            FrontBackView = 1;
        }
        else if (Input.GetButton("BackView"))
        {
            FrontBackView = -1;
        }
        else
        {
            FrontBackView = 0;
        }
    }

    bool Animationlng()
    {
        //Debug.Log(_ani.GetCurrentAnimatorStateInfo(0).normalizedTime);
        return _ani.GetCurrentAnimatorStateInfo(0).IsName("1H_sword_swing_high_left") || _ani.GetCurrentAnimatorStateInfo(0).IsName("1H_sword_swing_high_right") || _ani.GetCurrentAnimatorStateInfo(0).IsName("1H_walk_left_swing") || _ani.GetCurrentAnimatorStateInfo(0).IsName("1H_walk_right_swing");
    }

}
