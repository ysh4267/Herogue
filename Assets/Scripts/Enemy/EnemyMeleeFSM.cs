using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMeleeFSM : EnemyBase {
    public enum State {
        Idle,
        Move,
        Attack,
    };
    [SerializeField] protected NavMeshAgent navMeshAgent;

    State currentState = State.Idle;

    // Start is called before the first frame update
    new protected void Start() {
        base.Start();

        StartCoroutine(FSM());
    }

    // Update is called once per frame
    void Update() {

    }

    protected virtual IEnumerator FSM() {
        yield return null;
        //플레이어가 방 진입전에 while루프를 돌면서 대기
        while (!enemyParentRoom.GetComponent<RoomCondition>().playerInThisRoom) {
            yield return new WaitForSeconds(0.5f);
        }

        InitMonster();

        //Idle 상태로 루프를돌면서 조건에 맞춰 상태정의를 함
        while (true) {
            yield return StartCoroutine(currentState.ToString());
        }
    }

    protected virtual IEnumerator Idle() {
        yield return null;
        //애니메이션 상태가 반복해서 재지정 되어서 애니메이션의 시작부분만 반복하지 않게하기위한 조건문
        if (!enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle")) {
            enemyAnimator.SetTrigger("Idle");
        }

        //공격 사거리 안에 플레이어가 있는가
        if (CanAtkStateFun()) {
            //공격 쿨타임이 돌았으면 공격
            if (canAtk) {
                Debug.Log("공격상태로 전환됨");
                currentState = State.Attack;
            }
            //사거리안에 플레이어는 있지만 공격 쿨타임은 아직이라면 대기
            else {
                currentState = State.Idle;
                transform.LookAt(Player.transform.position);
            }
        }
        //사거리 안에 플레이어가 없다면 이동
        else {
            currentState = State.Move;
        }
    }

    //protected virtual void AtkEffect() {}

    //코루틴 Attack 함수
    protected virtual IEnumerator Attack() {
        yield return null;

        navMeshAgent.stoppingDistance = 0f; //공격모션이 시작되면 캐릭터와의 거리제한을 일시적으로 0으로 설정
        navMeshAgent.isStopped = true; //공격시작모션에는 멈춤
        navMeshAgent.SetDestination(Player.transform.position); //플레이어를 향해 이동방향변경
        yield return new WaitForSeconds(0.25f); //0.25초의 플레이어가 회피가능한 시간

        navMeshAgent.isStopped = false; //다시 내비메쉬를 켬
        navMeshAgent.speed = 30f; //30의속도로 돌진
        canAtk = false; //공격쿨타임 적용

        if (!enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack")) {
            enemyAnimator.SetTrigger("Attack"); //애니메이션 트리거 변경
        }
        //AtkEffect(); //애니메이션 이펙트 Instantiate용 함수
        yield return new WaitForSeconds(0.5f);

        //모든 변수 초기화
        navMeshAgent.speed = enemyMoveSpeed;
        navMeshAgent.stoppingDistance = enemyAttackRange;
        currentState = State.Idle;
    }

    //코루틴 Move 함수
    protected virtual IEnumerator Move() {
        yield return null;
        //애니메이션 상태가 반복해서 재지정 되어서 애니메이션의 시작부분만 반복하지 않게하기위한 조건문
        if (!enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Walk")) {
            enemyAnimator.SetTrigger("Walk");
        }
        //공격쿨타임 계산함수와 공격사거리 함수가 동시에 만족하면 공격상태로 변경
        if (CanAtkStateFun() && canAtk) {
            currentState = State.Attack;
        }
        // //플레이어와 Enemy사이의 거리가 플레이어 인식거리보다 멀다면 5f의 속도로 맵의 중앙을 향해 무조건 이동함
        // else if (distance > playerRealizeRange) {
        //     navMeshAgent.SetDestination(transform.parent.position - Vector3.forward * 5f);
        // }
        //위의 경우가 아니라면 Player를 향해 이동한다
        else {
            navMeshAgent.SetDestination(Player.transform.position);
        }
    }

}
