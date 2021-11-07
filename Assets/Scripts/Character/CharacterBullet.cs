using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBullet : MonoBehaviour {

    Vector3 newDir;
    public float bulletSpd = 30f;

    // Start is called before the first frame update
    void Start() {
        newDir = transform.forward;
        GetComponent<Rigidbody>().velocity = newDir * bulletSpd;
        Destroy(gameObject, 3.5f);
    }

    // void OnTriggerEnter(Collider other) {
    //     if (other.transform.CompareTag("Wall") || other.transform.CompareTag("Monster")) {
    //         //벽또는 몬스터에 닿았을때
    //         GetComponent<Rigidbody>().velocity = Vector3.zero;
    //         Destroy(gameObject, 0.05f);
    //     }
    // }

    //|| collision.transform.CompareTag("Monster")
    void OnCollisionEnter(Collision collision) {
        if (collision.transform.CompareTag("Wall")) {
            //벽에 닿았을때
            if (SingletonDataManager.Instance.isBulletBounce) {
                //튕기는 스킬이 있다면
                newDir = Vector3.Reflect(newDir, collision.contacts[0].normal);
                GetComponent<Rigidbody>().velocity = newDir * bulletSpd;
            }
            //없다면
            else {
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                Destroy(gameObject);
            }
        }
        // if (collision.transform.CompareTag("Player")) {
        //     //플레이어에 닿았을때
        //     GetComponent<Rigidbody>().velocity = Vector3.zero;
        //     Destroy(gameObject, 0.05f);
        // }
    }

    // Update is called once per frame
    void Update() {

    }
}
