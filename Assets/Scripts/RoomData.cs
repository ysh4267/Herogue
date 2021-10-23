using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomData {
    public enum RoomType {
        StartRoom,
        NormalRoom,
        BossRoom,
        ItemRoom
    }

    const int Xgap = 48;
    const int Ygap = 32;
    
    public int x, y;
    public bool isClear = false;
    public bool rightRoom = false;
    public bool leftRoom = false;
    public bool downRoom = false;
    public bool upRoom = false;
    public RoomType roomType = RoomType.NormalRoom;
    public GameObject roomObject = null;

    public void SetRoomPoint(int _x, int _y) {
        x = _x;
        y = _y;
    }

    public void DirectionalInfoUpdate() {
        roomObject.GetComponent<RoomCondition>().roomObjectController.DirectionalInfoUpdate(rightRoom, leftRoom, upRoom, downRoom);
    }

    public void InstantiateRoomPrefab(GameObject roomPrefab) {
        if (roomObject != null) MonoBehaviour.Destroy(roomObject);
        roomObject = MonoBehaviour.Instantiate(roomPrefab, new Vector3((x - 5) * Xgap, 0, (y - 5) * Ygap), Quaternion.identity);
        roomObject.GetComponent<RoomCordinate>().x = x;
        roomObject.GetComponent<RoomCordinate>().y = y;
        roomObject.GetComponent<RoomCordinate>().roomType = roomType;
        
    }

    public void IntializeRoomData(RoomData _mapGenData) {
        x = _mapGenData.x;
        y = _mapGenData.y;
        isClear = _mapGenData.isClear;
        rightRoom = _mapGenData.rightRoom;
        leftRoom = _mapGenData.leftRoom;
        downRoom = _mapGenData.downRoom;
        upRoom = _mapGenData.upRoom;
    }
}
