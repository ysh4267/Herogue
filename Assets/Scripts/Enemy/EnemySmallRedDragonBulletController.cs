using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySmallRedDragonBulletController : MonoBehaviour {
    float bulletSpd = 10.0f;
    int bulletDamage;
    Vector3 newDir;
    int bounceCnt = 3;

    // Start is called before the first frame update
    void Start() {
        newDir = transform.forward;
        GetComponent<Rigidbody>().velocity = newDir * bulletSpd;
    }

    // Update is called once per frame
    void Update() {

    }

    public void InitBullet(int _bulletDamage, float _bulletSpd, int _bounceCnt, float bulletRange) {
        bulletDamage = _bulletDamage;
        bulletSpd = _bulletSpd;
        bounceCnt = _bounceCnt;
        Destroy(gameObject, bulletRange);
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.transform.CompareTag("Player")) {
            collision.transform.GetComponent<PlayerBase>().Damaged(bulletDamage);
            Destroy(gameObject);
        }
        if (collision.transform.CompareTag("Wall")) {
            bounceCnt--;
            if (bounceCnt > 0) {
                newDir = Vector3.Reflect(newDir, collision.contacts[0].normal);
                GetComponent<Rigidbody>().velocity = newDir * bulletSpd;
            }
            else {
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                Destroy(gameObject, 0.1f);
            }
        }
    }
}
