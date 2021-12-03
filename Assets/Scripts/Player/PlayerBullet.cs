using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour {

    Vector3 newDir;
    public float bulletSpd;
    public float bulletRange;
    public float bulletDamage;

    // Start is called before the first frame update
    void Start() {
        
    }

    public void InitBullet(float _bulletSpd, float _bulletRange, float _bulletDamage) {
        bulletSpd = _bulletSpd;
        bulletRange = _bulletRange;
        bulletDamage = _bulletDamage;

        Destroy(gameObject, bulletRange);
        newDir = transform.forward;
        GetComponent<Rigidbody>().velocity = newDir * bulletSpd;
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.transform.CompareTag("Monster")) {
            collision.transform.GetComponent<EnemyBase>().Damaged(bulletDamage);
            Destroy(gameObject);
        }
        //벽에 닿았을때
        if (collision.transform.CompareTag("Wall")) {
            //튕기는 스킬이 있다면
            if (SingletonDataManager.Instance.isBulletBounce) {
                newDir = Vector3.Reflect(newDir, collision.contacts[0].normal);
                GetComponent<Rigidbody>().velocity = newDir * bulletSpd;
            }
            //없다면
            else {
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                Destroy(gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update() {

    }
}
