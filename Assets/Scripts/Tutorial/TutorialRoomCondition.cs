using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialRoomCondition : MonoBehaviour
{
    [SerializeField] TutorialDialog dialogManager = null;
    bool isOpened = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other) {
        if (isOpened) return;
        if (other.gameObject.CompareTag("Player")) {
            dialogManager.ActiveDialogWindow();
            if (transform.GetComponent<RoomCordinate>().x == 5 && transform.GetComponent<RoomCordinate>().y == 6) {
                transform.GetComponent<TutorialRoomDoorController>().CloseDoor();
            }
            isOpened = true;
        }
    }
}
