using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRedSlime : EnemyMeleeFSM {
    [SerializeField] GameObject meleeAtkArea; //Sphere Collider를 이용한 공격 사거리 콜라이더
    [SerializeField] EnemyRedSlimeMeleeAtkAreaController atkAreaController;

    //플레이어 인식범위와 공격사거리를 OnDrawGizmos 함수를 통해 시각화함
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemyAttackRange);
    }

    // Start is called before the first frame update
    new void Start() {
        base.Start();
        InitMonster();
    }

    protected override void InitMonster() {
        // //몬스터를 초기화
        enemyCurrentHP = enemyMaxHP;
        navMeshAgent.speed = enemyMoveSpeed;
        navMeshAgent.stoppingDistance = enemyAttackRange;
        atkAreaController.damage = enemyAttackDamage;
    }

    // Update is called once per frame
    void Update() {

    }

    public override void StartAttackHit() {
        meleeAtkArea.SetActive(true);
    }

    public override void EndAttackHit() {
        meleeAtkArea.SetActive(false);
    }

}
