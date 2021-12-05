using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDummyController : MonoBehaviour
{
    [SerializeField] Animator animator = null;
    [SerializeField] TutorialRoomDoorController tutorialRoomDoorController = null;

    public int HP = 5;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damaged() {
        HP--;
        if (HP <= 0) {
            animator.SetTrigger("Died");
            Destroy(this.gameObject, 0.5f);
            tutorialRoomDoorController.OpenDoor();
            return;
        }
        animator.SetTrigger("Hit");
    }
}
