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

    //총알 초기화
    public void InitBullet(float _bulletSpd, float _bulletRange, float _bulletDamage) {
        bulletSpd = _bulletSpd;
        bulletRange = _bulletRange;
        bulletDamage = _bulletDamage;

        //bulletRange초 후에 삭제
        Destroy(gameObject, bulletRange);
        
        newDir = transform.forward;
        GetComponent<Rigidbody>().velocity = newDir * bulletSpd;
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.transform.CompareTag("Monster")) {
            //몬스터의 최상위 부모클래스에 Damaged를 구현후 사용 ()
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
        //튜토리얼 훈련봇
        if (collision.transform.CompareTag("Training")) {
            collision.transform.GetComponent<TutorialDummyController>().Damaged();
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update() {

    }
}
