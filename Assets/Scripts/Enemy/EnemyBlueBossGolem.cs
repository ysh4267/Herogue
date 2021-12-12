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

    [SerializeField] protected GameObject enemyBulletPosition;
    [SerializeField] protected GameObject enemyMeleeAtkArea;
    [SerializeField] protected GameObject enemyBulletPrefab;
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
    int fireCount = 0;
    bool isAttacking = false;

    WaitForSeconds delay50 = new WaitForSeconds(0.05f);

    // Start is called before the first frame update
    new protected void Start() {
        base.Start();
        enemyMeleeAtkArea.GetComponent<EnemyBlueBossGolemMeleeAtkAreaController>().atkDamage = enemyAttackDamage;
        StartCoroutine(FSM());
    }

    // Update is called once per frame
    void Update() {

    }

    public override void Damaged(float _damage) {
        enemyCurrentHP -= _damage; //체력감소
        groggyGague += 1.0f;
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
        //그로기가 2순위
        else if (groggyGague > maxGroggyGague) {
            currentState = State.Groggy;
        }
        //공격중
        else if (isAttacking) {
            currentState = State.Idle;
        }
        //범위공격1
        else if (rangeAttack1CoolTimer > rangeAttack1CoolTime) {
            rangeAttack1CoolTimer = 0.0f;
            currentState = State.RangeAttack1;
        }
        //범위공격2
        else if (rangeAttack2CoolTimer > rangeAttack2CoolTime) {
            rangeAttack2CoolTimer = 0.0f;
            currentState = State.RangeAttack2;
        }
        //일반공격을 4번 쏘고
        else if (normalAtkCount <= 3) {
            normalAtkCount++;
            currentState = State.NormalRangeAttack;
        }
        //모든 공격패턴을 다 사용했다면 로켓펀치
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

        if (!enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("GetHit") && !enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Dizzy")) {
            enemyAnimator.SetTrigger("Groggy");
        }

        //그로기 타이머
        if (groggyTimer > groggyTime) {
            groggyTimer = 0.0f;
            groggyGague = 0;
            isAttacking = false;
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
        enemyMeleeAtkArea.SetActive(true);
    }

    public override void EndAttackHit() {
        isAttacking = false;
        enemyMeleeAtkArea.SetActive(false);
        navMeshAgent.isStopped = true;
    }

    public void RangeAttack1Event() {
        fireCount++;
        int roundNum = fireCount % 2 == 0 ? 20 : 30;


        for (int i = 0; i < roundNum; i++) {
            GameObject bulletObject = Instantiate(enemyBulletPrefab, enemyBulletPosition.transform.position, Quaternion.identity);

            //원의 방향을 삼각함수와 roundNum의 비율로 변경
            Vector3 newDir = new Vector3(Mathf.Cos(Mathf.PI * 2 * i / roundNum), 0, Mathf.Sin(Mathf.PI * 2 * i / roundNum));
            bulletObject.GetComponent<Rigidbody>().velocity = newDir * 10;

            Vector3 rotationVac = Vector3.forward * 360 * i / roundNum + Vector3.forward * 90;
            bulletObject.transform.Rotate(rotationVac);
        }

        if (fireCount < 3) {
            Invoke("RangeAttack1Event", 0.5f);
        }
        else {
            fireCount = 0;
        }
    }

    public void RangeAttack2Event(int patternNum) {
        int bulletNum = 30;
        switch (patternNum) {
            case 1:
                StartCoroutine(FireCircleShot(bulletNum));
                break;
            case 2:
                StartCoroutine(FireReverseCircleShot(bulletNum));
                break;
            case 3:
                StartCoroutine(FireCircleShot(bulletNum + 10));
                StartCoroutine(FireReverseCircleShot(bulletNum + 10));
                break;
            default:
                return;
        }
    }

    protected IEnumerator FireCircleShot(int bulletNum) {
        float rotation = 0f;
        Vector3 tempAngle = enemyBulletPosition.transform.rotation.eulerAngles;
        for (int i = 0; i < bulletNum; i++) {
            rotation = 720.0f * ((float)i / (float)bulletNum);
            GameObject bulletObject = Instantiate(enemyBulletPrefab, enemyBulletPosition.transform.position, Quaternion.identity);
            bulletObject.transform.Rotate(tempAngle + new Vector3(0, rotation, 0));
            bulletObject.GetComponent<Rigidbody>().velocity = bulletObject.transform.forward * 10;
            bulletObject.GetComponent<EnemyBlueBossGolemBulletController>().bulletDamage = enemyAttackDamage;
            yield return delay50;
        }
        yield return null;
    }

    protected IEnumerator FireReverseCircleShot(int bulletNum) {
        float rotation = 0f;
        Vector3 tempAngle = enemyBulletPosition.transform.rotation.eulerAngles;
        for (int i = 0; i < bulletNum; i++) {
            rotation = 720.0f * ((float)i / (float)bulletNum);
            GameObject bulletObject = Instantiate(enemyBulletPrefab, enemyBulletPosition.transform.position, Quaternion.identity);
            bulletObject.transform.Rotate(tempAngle + new Vector3(0, -rotation, 0));
            bulletObject.GetComponent<Rigidbody>().velocity = bulletObject.transform.forward * 10;
            bulletObject.GetComponent<EnemyBlueBossGolemBulletController>().bulletDamage = enemyAttackDamage;
            yield return delay50;
        }
        yield return null;
    }


    public void NormalRangeAttackEvent() {
        int roundNum = 5;

        for (int i = 0; i < roundNum; i++) {
            GameObject bulletObject = Instantiate(enemyBulletPrefab, enemyBulletPosition.transform.position, Quaternion.identity);
            Vector3 newDir = transform.rotation.eulerAngles + new Vector3(0, Random.Range(-7.0f, 7.0f), 0);
            bulletObject.transform.Rotate(newDir);
            bulletObject.GetComponent<Rigidbody>().velocity = bulletObject.transform.forward * 15;
            bulletObject.GetComponent<EnemyBlueBossGolemBulletController>().bulletDamage = enemyAttackDamage;
        }
    }
}
