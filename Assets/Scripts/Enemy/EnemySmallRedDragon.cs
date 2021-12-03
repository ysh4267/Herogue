using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySmallRedDragon : EnemyRangeFSM {
    [SerializeField] protected float enemyStopRange;

    // Start is called before the first frame update
    new void Start() {
        base.Start();

        InitMonster();
    }

    protected override void InitMonster() {
        // //몬스터를 초기화
        enemyCurrentHP = enemyMaxHP;
        navMeshAgent.speed = enemyMoveSpeed;
        navMeshAgent.stoppingDistance = enemyStopRange;
    }

    // Update is called once per frame
    void Update() {
        
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemyAttackRange);
    }
}
