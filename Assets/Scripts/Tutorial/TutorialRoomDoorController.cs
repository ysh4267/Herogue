using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialRoomDoorController : MonoBehaviour
{
    [SerializeField] List<GameObject> DoorObjectList = new List<GameObject>();
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenDoor() {
        foreach (var item in DoorObjectList)
        {
            item.SetActive(false);
        }
    }

    public void CloseDoor() {
        foreach (var item in DoorObjectList)
        {
            item.SetActive(true);
        }
    }
}
