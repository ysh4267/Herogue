using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBlueBossGolem : EnemyBase {
    public enum State {
        Idle,
        Move,
        MeleeAttack,
        NormalRangeAttack,
        RangeAttack1,
        RangeAttack2,
        Groggy,
        Dead
    };

    [SerializeField] protected NavMeshAgent navMeshAgent;
    [SerializeField] protected float rangeAttack1CoolTime;
    protected float rangeAttack1CoolTimer = 0.0f;
    [SerializeField] protected float rangeAttack2CoolTime;
    protected float rangeAttack2CoolTimer = 0.0f;
    [SerializeField] protected float groggyTime;
    protected float groggyTimer = 0.0f;

    State currentState = State.Idle;
    [SerializeField] protected float maxGroggyGague = 0.0f;
    float groggyGague = 0.0f;
    int normalAtkCount = 0;
    bool isAttacking = false;

    // Start is called before the first frame update
    new protected void Start() {
        base.Start();

        StartCoroutine(FSM());
    }

    // Update is called once per frame
    void Update() {

    }

    protected override void InitMonster() {
        navMeshAgent.stoppingDistance = enemyAttackRange;
        navMeshAgent.speed = enemyMoveSpeed;
    }

    protected virtual IEnumerator FSM() {
        yield return null;
        //플레이어가 방 진입전에 while루프를 돌면서 대기
        while (!enemyParentRoom.GetComponent<RoomCondition>().playerInThisRoom) {
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(3.0f);

        InitMonster();

        //Idle 상태로 루프를돌면서 조건에 맞춰 상태정의를 함
        while (true) {
            if (rangeAttack1CoolTimer < rangeAttack1CoolTime) {
                rangeAttack1CoolTimer += Time.deltaTime;
            }
            if (rangeAttack2CoolTimer < rangeAttack2CoolTime) {
                rangeAttack2CoolTimer += Time.deltaTime;
            }
            yield return StartCoroutine(currentState.ToString());
        }
    }

    protected virtual IEnumerator Idle() {
        yield return null;
        //애니메이션 상태가 반복해서 재지정 되어서 애니메이션의 시작부분만 반복하지 않게하기위한 조건문
        if (!enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle")) {
            enemyAnimator.SetTrigger("Idle");
        }

        //Enemy가 죽으면
        if (enemyCurrentHP <= 0) {
            currentState = State.Dead;
        }
        else if (isAttacking) {
            currentState = State.Idle;
        }
        else if (rangeAttack1CoolTimer > rangeAttack1CoolTime) {
            rangeAttack1CoolTimer = 0.0f;
            currentState = State.RangeAttack1;
        }
        else if (rangeAttack2CoolTimer > rangeAttack2CoolTime) {
            rangeAttack2CoolTimer = 0.0f;
            currentState = State.RangeAttack2;
        }
        //일반공격을 4번 쏘고
        else if (normalAtkCount <= 3) {
            normalAtkCount++;
            currentState = State.NormalRangeAttack;
        }
        //모든 공격패턴을 다 사용했다면
        else {
            normalAtkCount = 0;
            currentState = State.MeleeAttack;
        }
    }

    //protected virtual void AtkEffect() {}

    //코루틴 Attack 함수
    protected virtual IEnumerator MeleeAttack() {
        yield return null;
        isAttacking = true;
        transform.LookAt(Player.transform.position);
        navMeshAgent.isStopped = true; //공격시작모션에는 멈춤
        navMeshAgent.SetDestination(Player.transform.position);

        yield return new WaitForSeconds(0.5f); //0.5초의 플레이어가 회피가능한 시간
        navMeshAgent.isStopped = false; //공격시작

        if (!enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("MeleeAttack")) {
            enemyAnimator.SetTrigger("MeleeAttack"); //애니메이션 트리거 변경
        }

        currentState = State.Idle;
    }

    //코루틴 Move 함수
    protected virtual IEnumerator Move() {
        yield return null;

        navMeshAgent.isStopped = false;
        //애니메이션 상태가 반복해서 재지정 되어서 애니메이션의 시작부분만 반복하지 않게하기위한 조건문
        if (!enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Walk")) {
            enemyAnimator.SetTrigger("Walk");
        }

        //Enemy가 죽으면
        if (enemyCurrentHP <= 0) {
            currentState = State.Dead;
        }
        else if (CanAtkStateFun()) {
            currentState = State.MeleeAttack;
        }
        //위의 경우가 아니라면 Player를 향해 이동한다
        else {
            navMeshAgent.SetDestination(Player.transform.position);
        }
    }

    protected virtual IEnumerator Dead() {
        yield return null;
        navMeshAgent.isStopped = true;

        if (!enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Dead")) {
            enemyAnimator.SetTrigger("Dead");
        }
        enemyParentRoom.GetComponent<RoomCondition>().monsterListInRoom.Remove(this.gameObject);
        Destroy(this.transform.gameObject, 0.5f);
    }

    protected virtual IEnumerator RangeAttack1() {
        yield return null;
        navMeshAgent.isStopped = true;
        isAttacking = true;

        transform.LookAt(Player.transform.position);

        if (!enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("RangeAttack1")) {
            enemyAnimator.SetTrigger("RangeAttack1");
        }

        currentState = State.Idle;
    }

    protected virtual IEnumerator RangeAttack2() {
        yield return null;
        navMeshAgent.isStopped = true;
        isAttacking = true;

        transform.LookAt(Player.transform.position);

        if (!enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("RangeAttack2")) {
            enemyAnimator.SetTrigger("RangeAttack2");
        }

        currentState = State.Idle;
    }

    protected virtual IEnumerator Groggy() {
        yield return null;
        navMeshAgent.isStopped = true;
        isAttacking = false;

        if (!enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("GetHit") || !enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Dizzy")) {
            enemyAnimator.SetTrigger("Groggy");
        }

        //그로기 타이머
        if (groggyTimer > groggyTime) {
            groggyTimer = 0.0f;
            currentState = State.Idle;
        }
        else {
            groggyTimer += Time.deltaTime;
        }
    }

    protected virtual IEnumerator NormalRangeAttack() {
        navMeshAgent.isStopped = true;
        yield return new WaitForSeconds(0.4f);

        isAttacking = true;

        transform.LookAt(Player.transform.position);

        if (!enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("NormalRangeAttack")) {
            enemyAnimator.SetTrigger("NormalRangeAttack");
        }

        currentState = State.Idle;
    }

    public override void StartAttackHit() {

    }

    public override void EndAttackHit() {
        isAttacking = false;
        navMeshAgent.isStopped = true;
    }

    public void RangeAttack1Event() {

    }

    public void RangeAttack2Event(int patternNum) {

    }

    public void NormalRangeAttackEvent() {
        

    }
}
