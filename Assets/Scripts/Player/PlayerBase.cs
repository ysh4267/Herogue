using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour {
    public int playerMaxHP;     //최대체력칸
    public int playerCurrentHP; //최대체력칸 * 2가 최대치인 현재체력
    public float attackDmg = 1f;
    //공격 속도
    public float attackSpd;
    public float bulletSpd; //투사체 속도
    public float bulletRange; //투사체 속도
    public float moveSpeed; //이동 속도
    //피격무적
    public bool isInvincibility = false;

    public GameObject PlayerModel;
    //UI체력아이콘 컨트롤러(옵저버화 예정)
    public PlayerHPIconController playerHPIconController;

    WaitForSeconds Delay100 = new WaitForSeconds(0.1f);
    WaitForSeconds Delay500 = new WaitForSeconds(0.5f);

    // Start is called before the first frame update
    void Start() {
        playerHPIconController.HpImageUpdate(playerMaxHP, playerCurrentHP);
    }

    // Update is called once per frame
    void Update() {

    }

    public void Damaged(int monsterDamage) {
        if (!isInvincibility) {
            StartCoroutine(DamagedInvince());
        }
        playerCurrentHP -= monsterDamage;
        if (playerCurrentHP <= 0) {
            playerCurrentHP = 0;
            //gameOver
        }
        playerHPIconController.HpImageUpdate(playerMaxHP, playerCurrentHP);
    }

    IEnumerator DamagedTimer () {
        isInvincibility = true;
        yield return Delay500;
        isInvincibility = false;
    }

    IEnumerator DamagedInvince() {
        StartCoroutine(DamagedTimer());
        while (isInvincibility) {
            PlayerModel.SetActive(!PlayerModel.activeSelf);
            yield return Delay100;
        }
        PlayerModel.SetActive(true);
    }

    public void HpGain() {
        playerMaxHP++;
        playerCurrentHP = playerMaxHP * 2;
        playerHPIconController.HpImageUpdate(playerMaxHP, playerCurrentHP);
    }
}
