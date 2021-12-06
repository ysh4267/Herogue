using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour {
    [SerializeField] PlayerBase playerBase;
    [SerializeField] Animator Anim; //모션 애니메이션

    void FixedUpdate() {
        AnimationStateUpdate();
        
    }

    void AnimationStateUpdate() {
        //클릭시에 공격
        if (Input.GetMouseButton(0)) {
            Anim.SetFloat("AttackSpd", playerBase.attackSpd);
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
