using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class EnemyRangeFSM : EnemyBase {
    public enum State {
        Idle,
        Move,
        Attack,
        Dead
    };
    [SerializeField] protected float enemyBulletRange = 7.0f;
    [SerializeField] protected int enemyAttackBulletBounce = 3;
    [SerializeField] protected float enemyAttackBulletSpd = 10.0f;
    [SerializeField] protected LineRenderer lineRenderer = null; //투사체 경고선
    [SerializeField] protected Transform boltGenPosition = null; //투사체 발사위치
    [SerializeField] protected GameObject enemyBullet = null; //몬스터가 발사할 투사체
    [SerializeField] protected NavMeshAgent navMeshAgent = null;
    protected State currentState = State.Idle;

    WaitForSeconds Delay2000 = new WaitForSeconds(2.0f);

    // Start is called before the first frame update
    new protected void Start() {
        base.Start();

        lineRenderer.startColor = new Color(1, 0, 0, 0.5f);
        lineRenderer.endColor = new Color(1, 0, 0, 0.5f);
        lineRenderer.startWidth = 0.3f;
        lineRenderer.endWidth = 0.3f;

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
        navMeshAgent.isStopped = true;
        //Enemy가 죽으면
        if (enemyCurrentHP <= 0) {
            currentState = State.Dead;
        }
        //공격 사거리 안에 플레이어가 있는가
        else if (CanAtkStateFun()) {
            //공격 쿨타임이 돌았으면 공격
            if (canAtk) {
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
        //애니메이션 상태가 반복해서 재지정 되어서 애니메이션의 시작부분만 반복하지 않게하기위한 조건문
        if (!enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack")) {
            enemyAnimator.SetTrigger("Attack");
        }
        navMeshAgent.isStopped = true; //공격시작모션에는 멈춤
        //플레이어를 바라봄
        transform.LookAt(Player.transform.position);
        //경고선을 먼저 쏨
        ShootDangerMarker();
        //회피용 딜레이
        yield return Delay2000;
        DeleteDangerMarker();
        ShootBolt();
        yield return Delay2000;
        currentState = State.Idle;
    }

    void ShootBolt() {        
        GameObject eBullet = Instantiate(enemyBullet, boltGenPosition.position, transform.rotation);
        eBullet.GetComponent<EnemySmallRedDragonBulletController>().InitBullet(enemyAttackDamage, enemyAttackBulletSpd, enemyAttackBulletBounce, enemyBulletRange);
    }

    void ShootDangerMarker() {
        Vector3 NewPosition = boltGenPosition.position;
        Vector3 NewDir = transform.forward;
        //라인렌더러의 지점 갯수
        lineRenderer.positionCount = 1;
        //라인렌더러의 시작지점
        lineRenderer.SetPosition(0, NewPosition);
        int _layerMask = 1 << LayerMask.NameToLayer("Wall"); //마스크 설정

        for (int i = 1; i <= enemyAttackBulletBounce; i++) {
            //레이캐스트로 라인렌더러가 벽에 닿는지점을 확인
            Physics.Raycast(NewPosition, NewDir, out RaycastHit hit, 50.0f, _layerMask);
            //라인렌더러의 지점갯수를 증가시키고 두번째 꼭짓점을 추가
            lineRenderer.positionCount++;
            if (hit.transform == null) { 
                lineRenderer.SetPosition(i, NewPosition + (NewDir * 50)); 
                break;
            }
            lineRenderer.SetPosition(i, hit.point);
            //다음 레이캐스트 시작위치를 지정
            NewPosition = new Vector3(hit.point.x, boltGenPosition.position.y, hit.point.z);
            NewDir = Vector3.Reflect(NewDir, hit.normal);
        }
    }

    void DeleteDangerMarker() {
        for (int i = 0; i < lineRenderer.positionCount; i++) {
            lineRenderer.SetPosition(i, Vector3.zero);
        }
        lineRenderer.positionCount = 0;
    }

    //코루틴 Move 함수
    protected virtual IEnumerator Move() {
        yield return null;
        //애니메이션 상태가 반복해서 재지정 되어서 애니메이션의 시작부분만 반복하지 않게하기위한 조건문
        if (!enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Move")) {
            enemyAnimator.SetTrigger("Move");
        }
        navMeshAgent.isStopped = false;
        //Enemy가 죽으면
        if (enemyCurrentHP <= 0) {
            currentState = State.Dead;
        }
        //공격쿨타임 계산함수와 공격사거리 함수가 동시에 만족하면 공격상태로 변경
        else if (CanAtkStateFun() && canAtk) {
            currentState = State.Attack;
        }
        //위의 경우가 아니라면 Player를 향해 이동한다
        else {
            navMeshAgent.SetDestination(Player.transform.position);
        }
    }

    
    protected virtual IEnumerator Dead() {
        yield return null;
        navMeshAgent.isStopped = true;
        //애니메이션 상태가 반복해서 재지정 되어서 애니메이션의 시작부분만 반복하지 않게하기위한 조건문
        if (!enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Dead")) {
            enemyAnimator.SetTrigger("Dead");
        }
        enemyParentRoom.GetComponent<RoomCondition>().monsterListInRoom.Remove(this.gameObject);
        Destroy(this.transform.gameObject, 0.5f);
    }
}
