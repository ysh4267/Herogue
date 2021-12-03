using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRedSlimeMeleeAtkAreaController : MonoBehaviour {
    [SerializeField] GameObject atkArea = null;
    public int damage;

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            other.GetComponent<PlayerBase>().Damaged(damage);
            atkArea.SetActive(false);
        }
    }
}
