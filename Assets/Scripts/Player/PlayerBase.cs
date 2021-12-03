using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour {
    public int playerMaxHP;
    public int playerCurrentHP;
    public float attackDmg = 1f;
    public float attackSpd;
    public float bulletSpd = 30f; //투사체 속도
    public float bulletRange = 3.5f; //투사체 속도
    public float moveSpeed = 25f; //이동속도
    public bool isInvincibility = false;

    public GameObject PlayerModel;

    WaitForSeconds Delay100 = new WaitForSeconds(0.1f);
    WaitForSeconds Delay500 = new WaitForSeconds(0.5f);

    // Start is called before the first frame update
    void Start() {
        attackSpd = 1.5f;
        Damaged(1);
    }

    // Update is called once per frame
    void Update() {

    }

    public void Damaged(int monsterDamage) {
        if (!isInvincibility) {
            StartCoroutine(DamagedInvince());
        }
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
}
