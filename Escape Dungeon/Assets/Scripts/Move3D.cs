
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move3D : MonoBehaviour
{
    // Start is called before the first frame update

    CharacterController cc;
    Animator _ani;
    Camera mainCam;

    public static Move3D instance;

    public float moveSpeed;  //이동 스피드
    public float jumpSpeed; //점프 힘
    public float gravity = 30.0f;  //중력값
    public float yVelocty = 0;  //점프에 필요한 변수


    public Vector3 moveDir = Vector3.zero;

    public Transform cameraPos;  //메인 카메라 위치

    private void Awake()
    {
        instance = this;

        //yVelocty = GameManager.instance.LastY_Velocty;
        yVelocty = 0;

        cc = GetComponent<CharacterController>();
        _ani = GetComponent<Animator>();
        mainCam = Camera.main;
    }
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        MoveStart3D();
        Jump3D();
    }


    //이동 함수 정의
    void MoveStart3D()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");


        moveDir.x = x;
        moveDir.y = 0;
        moveDir.z = z;


          

        //로컬의 이동벡터를 씬의 글로벌값으로 변환
        moveDir = this.transform.TransformDirection(moveDir);



        Vector3 lookForward = new Vector3(mainCam.transform.forward.x, this.transform.forward.y, mainCam.transform.forward.z).normalized;
        Vector3 lookRight = new Vector3(mainCam.transform.right.x, this.transform.forward.y, mainCam.transform.right.z).normalized;
        moveDir = lookForward * z + lookRight * x;

        moveDir *= moveSpeed;   //moveDir = moveDir * moveSpeed;

        
        yVelocty -= gravity * Time.deltaTime;   //중력이 실시간으로 y값으로 끌어내림

        
        moveDir.y = yVelocty;

        cc.Move(moveDir * Time.deltaTime);  //캐릭터컨트롤러 이동함수
        if (cc.isGrounded) yVelocty = 0;
        if (moveDir.x != 0 && moveDir.z != 0)
        {
            this.transform.forward = Vector3.Lerp(this.transform.forward,new Vector3(moveDir.x, 0f, moveDir.z),Time.deltaTime*2f);
        }
        
        // this.transform.rotation = Camera.main.transform.rotation;
        //this.transform.rotation = Quaternion.Euler(new Vector3(0, Camera.main.transform.rotation.eulerAngles.y,0));
    }

    //점프 함수 정의
    void Jump3D()
    {
        if (cc.isGrounded)
        {
            if (Input.GetButtonDown("Jump"))   //"Jump" = 스페이스바
            {
                yVelocty = jumpSpeed;
                cc.Move(new Vector3(0,0.5f,0));
            }
        }
    }

    //카메라 관련 업데이트 함수
    private void LateUpdate()
    {
        //Camera.main.transform.position = cameraPos.position;
    }






}//end class

