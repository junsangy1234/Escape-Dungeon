using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _2DChangeSkill : MonoBehaviour
{
    Transform tr;
    Rigidbody rbody;


    float bulletSpeed = 1000;

    // Start is called before the first frame update
    void Start()
    {
        tr = transform;
        rbody = GetComponent<Rigidbody>();

        rbody.AddForce(tr.forward * bulletSpeed);
        Destroy(tr.gameObject, 10.0f);
    }


    private void OnCollisionEnter(Collision collision)
    {

        //Destroy(tr.gameObject);

    }

    private void OnTriggerExit(Collider other)
    {
        switch (other.gameObject.layer)
        {
            case 8:
                {
                    {
                        if(ChangeCamera.instance.Is3D)
                        {
                            ChangeCamera.instance.Changecamera();
                            SkeletonEnemy.instance.is2DChangeHit = true;
                        }
                        else
                        {
                            SkeletonEnemy.instance.is2DChangeHit = true;
                        }
                    }
                    break;
                }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        

    }


}
