using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour {
    [SerializeField] protected float enemyMaxHP;        //최대 체력
    [SerializeField] protected float enemyCurrentHP;    //현재 체력
    [SerializeField] protected int enemyAttackDamage;   //적 데미지
    [SerializeField] protected float enemyAttackRange;  //공격 사거리
    [SerializeField] protected float enemyAttackCoolTime;           //공격 딜레이
    [SerializeField] protected float enemyAttackCurrentCoolTime;    //현재 남은 공격 딜레이
    [SerializeField] protected float enemyMoveSpeed = 2f;           //이동속도
    [SerializeField] protected GameObject enemyParentRoom;  //적이 위치한 방 오브젝트
    [SerializeField] protected Animator enemyAnimator;     //애니메이터
    [SerializeField] protected Rigidbody enemyRigidBody;   //리지드바디
    protected GameObject Player;
    protected float distance; //플레이어와의 거리값
    protected bool canAtk = true; //공격쿨타임중인지 판별하기위한 불값
    protected int layerMask; //레이캐스트용 레이어 마스크

    protected virtual void InitMonster() { } //적 수치값 수정용 초기화함수

    // Start is called before the first frame update
    protected void Start() {
        Player = GameObject.FindGameObjectWithTag("Player");

        //공격 쿨타임을 코루틴을 활용하여 계속 실행시킴
        StartCoroutine(CalcCoolTime());
    }

    // Update is called once per frame
    void Update() {

    }

    //공격 사거리안에 Player를 감지하는 함수
    protected bool CanAtkStateFun() {
        RaycastHit hit;
        Vector3 targetDir = new Vector3(Player.transform.position.x - transform.position.x, 0f, Player.transform.position.z - transform.position.z); //Enemy에서 Player쪽 방향 벡터값

        layerMask = (1 << LayerMask.NameToLayer("Wall")) + (1 << LayerMask.NameToLayer("Player")); //마스크 설정

        Physics.Raycast(new Vector3(transform.position.x, 2.5f, transform.position.z), targetDir, out hit, 50f, layerMask); //Enemy에서 targetDir방향으로 30거리만큼만 레이캐스트
        distance = Vector3.Distance(Player.transform.position, transform.position); //Enemy와 Player사이의 거리값
        Debug.DrawRay(new Vector3(transform.position.x, 2.5f, transform.position.z), targetDir * 50f, Color.green);
        //레이캐스트가 아무것도 적중하지 못했을 시
        if (hit.transform == null) {
            return false;
        }
        //플레이어에게 레이캐스트가 적중하고 Enemy와 Player사이의 거리값이 공격사거리보다 작으면 true
        if (hit.transform.CompareTag("Player") && distance <= enemyAttackRange) {
            return true;
        }
        //30거리안에 플레이어가 없거나 레이캐스트가 벽에 막혔다면 false
        else {
            return false;
        }
    }

    //공격 쿨타임을 계산하는 코루틴
    protected virtual IEnumerator CalcCoolTime() {
        while (true) {
            yield return null;
            //공격이 시작됬다면 쿨타임 계산 시작
            if (!canAtk) {
                //Time.deltaTime만큼 attackCoolTimeCacl계산
                enemyAttackCurrentCoolTime -= Time.deltaTime;
                //attackCoolTimeCacl이 0에 근접하면 attackCoolTime값으로 초기화
                if (enemyAttackCurrentCoolTime <= 0) {
                    enemyAttackCurrentCoolTime = enemyAttackCoolTime;
                    canAtk = true; //공격이 다시 가능함
                }
            }
        }
    }

    public void Damaged(float _damage) {
        enemyCurrentHP -= _damage; //체력감소
    }

    //애니메이션 이벤트 함수
    public virtual void StartAttackHit() {

    }

    public virtual void EndAttackHit() {

    }
}
