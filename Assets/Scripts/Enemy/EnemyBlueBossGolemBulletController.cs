using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBlueBossGolemBulletController : MonoBehaviour {
    public int bulletDamage;

    // Start is called before the first frame update
    void Start() {
        Destroy(gameObject, 5.0f);

    }

    // Update is called once per frame
    void Update() {

    }

    private void OnCollisionEnter(Collision collision) {
        Debug.Log("Collide!");

        //플레이어와 부딪혔을때
        if (collision.transform.CompareTag("Player")) {
            collision.transform.GetComponent<PlayerBase>().Damaged(bulletDamage);
            Destroy(gameObject);
        }
        //벽에 닿았을때
        if (collision.transform.CompareTag("Wall")) {
            Debug.Log("Collide!Wall");
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            Destroy(gameObject, 0.1f);
        }
    }


    private void OnTriggerEnter(Collider other) {
        //플레이어와 부딪혔을때
        if (other.transform.CompareTag("Player")) {
            other.transform.GetComponent<PlayerBase>().Damaged(bulletDamage);
            Destroy(gameObject);
        }
        //벽에 닿았을때
        if (other.transform.CompareTag("Wall")) {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            Destroy(gameObject, 0.1f);
        }
    }
}
