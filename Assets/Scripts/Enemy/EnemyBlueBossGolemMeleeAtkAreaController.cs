using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBlueBossGolemMeleeAtkAreaController : MonoBehaviour
{
    public int atkDamage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other) {
        if (other.transform.CompareTag("Player")) {
            other.transform.GetComponent<PlayerBase>().Damaged(atkDamage);
            this.gameObject.SetActive(false);
        }
    }
}
