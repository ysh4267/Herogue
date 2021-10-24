using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBullet : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        GetComponent<Rigidbody>().velocity = transform.forward * 20f;
        Destroy(gameObject, 1.5f);
    }

    // Update is called once per frame
    void Update() {

    }
}
