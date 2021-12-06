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
            //이동
            rigidBody.velocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * playerBase.moveSpeed; //x, z벡터로 캐릭터를 이동시킴 (가시성을 위해 z의 y표기)
        }
    }

}
