using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {
    [SerializeField] PlayerBase playerBase;
    [SerializeField] Rigidbody rigidBody;

    void Start() {

    }

    void FixedUpdate() {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) {

            rigidBody.velocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * playerBase.moveSpeed; //x, z벡터로 캐릭터를 이동시킴 (가시성을 위해 z의 y표기)
            //rb.rotation = Quaternion.LookRotation(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"))); //캐릭터의 앞을 x, z각도만큼 돌림
        }
    }

}
