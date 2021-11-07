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

    // Start is called before the first frame update
    void Start() {
        attackSpd = 1.5f;
    }

    // Update is called once per frame
    void Update() {

    }
}
