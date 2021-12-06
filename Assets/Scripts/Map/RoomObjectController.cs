using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomObjectController : MonoBehaviour {
    [SerializeField] GameObject EastDoorObject = null;
    [SerializeField] GameObject WestDoorObject = null;
    [SerializeField] GameObject NorthDoorObject = null;
    [SerializeField] GameObject SouthDoorObject = null;
    [SerializeField] GameObject EastBridgeObject = null;
    [SerializeField] GameObject WestBridgeObject = null;
    [SerializeField] GameObject NorthBridgeObject = null;
    [SerializeField] GameObject SouthBridgeObject = null;
    [SerializeField] GameObject EastWallObject = null;
    [SerializeField] GameObject WestWallObject = null;
    [SerializeField] GameObject NorthWallObject = null;
    [SerializeField] GameObject SouthWallObject = null;

    List<GameObject> doorList = new List<GameObject>();

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    //상하좌우에 있는 오브젝트의 온오프를 변경 및 열리고 닫힐 문을 doorList에 추가
    public void DirectionalInfoUpdate(bool rightRoom, bool leftRoom, bool upRoom, bool downRoom) {
        EastWallObject.SetActive(true);
        WestWallObject.SetActive(true);
        NorthWallObject.SetActive(true);
        SouthWallObject.SetActive(true);

        if (rightRoom) {
            EastBridgeObject.SetActive(true);
            EastDoorObject.SetActive(false);
            EastWallObject.SetActive(false);
            doorList.Add(EastDoorObject);
        }
        if (leftRoom) {
            WestBridgeObject.SetActive(true);
            WestDoorObject.SetActive(false);
            WestWallObject.SetActive(false);
            doorList.Add(WestDoorObject);
        }
        if (upRoom) {
            NorthBridgeObject.SetActive(true);
            NorthDoorObject.SetActive(false);
            NorthWallObject.SetActive(false);
            doorList.Add(NorthDoorObject);
        }
        if (downRoom) {
            SouthBridgeObject.SetActive(true);
            SouthDoorObject.SetActive(false);
            SouthWallObject.SetActive(false);
            doorList.Add(SouthDoorObject);
        }
    }

    //문 닫기
    public void CloseDoor() {
        for (int i = 0; i < doorList.Count; i++)
        {
            doorList[i].SetActive(true);
        }
    }

    //문 열기
    public void OpenDoor() {
        for (int i = 0; i < doorList.Count; i++)
        {
            doorList[i].SetActive(false);
        }
    }
}
