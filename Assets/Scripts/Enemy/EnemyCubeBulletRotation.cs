using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//몬스터 투사체 회전
public class EnemyCubeBulletRotation : MonoBehaviour {
    public float speed;
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        transform.Rotate(new Vector3(speed, speed, 0), Space.World);
        transform.Rotate(new Vector3(speed, speed, speed), Space.Self);
    }
}
