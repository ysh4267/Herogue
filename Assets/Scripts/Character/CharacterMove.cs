using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour {
    [SerializeField] Rigidbody rb;
    [SerializeField] Animator Anim; //모션 애니메이션
    public float moveSpeed = 25f; //이동속도
    public float attackSpd;

    void Start() {
        attackSpd = 1.5f;
    }

    void FixedUpdate() {
        AnimationStateUpdate();
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) {

            rb.velocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * moveSpeed; //x, z벡터로 캐릭터를 이동시킴 (가시성을 위해 z의 y표기)
            //rb.rotation = Quaternion.LookRotation(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"))); //캐릭터의 앞을 x, z각도만큼 돌림
        }
    }

    void AnimationStateUpdate() {
        if (Input.GetMouseButton(0)) {
            Anim.SetFloat("AttackSpd", attackSpd);
            Anim.SetBool("Walk", false);
            Anim.SetBool("Attack", true);
        }
        else if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) {
            //움직일때
            Anim.SetBool("Walk", true);
            Anim.SetBool("Attack", false);
        }
        else {
            Anim.SetBool("Attack", false);
            Anim.SetBool("Walk", false);
        }
    }
}
