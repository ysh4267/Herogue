using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//방의 모든 정보를 담는 클래스 (RoomGenerater에서 리스트로 관리)
//리스트에서는 오브젝트의 레퍼런스값을 갖고
//실제 오브젝트에는 복사된 정보만이 들어감
public class RoomData {
    public enum RoomType {
        StartRoom,
        NormalRoom,
        BossRoom,
        ItemRoom
    }

    //방의 좌표별 위치 프리셋값
    const int Xgap = 48;
    const int Ygap = 32;
    
    //좌표
    public int x, y;
    //클리어 여부
    public bool isClear = false;
    //상하좌우 방 여부
    public bool rightRoom = false;
    public bool leftRoom = false;
    public bool downRoom = false;
    public bool upRoom = false;
    //방의 타입
    public RoomType roomType = RoomType.NormalRoom;
    //실제 오브젝트
    public GameObject roomObject = null;

    //초기화용 함수들
    public void SetRoomPoint(int _x, int _y) {
        x = _x;
        y = _y;
    }

    public void DirectionalInfoUpdate() {
        roomObject.GetComponent<RoomCondition>().roomObjectController.DirectionalInfoUpdate(rightRoom, leftRoom, upRoom, downRoom);
    }

    //방 오브젝트 생성
    public GameObject InstantiateRoomPrefab(GameObject roomPrefab) {
        if (roomObject != null) MonoBehaviour.Destroy(roomObject);
        roomObject = MonoBehaviour.Instantiate(roomPrefab, new Vector3((x - 5) * Xgap, 0, (y - 5) * Ygap), Quaternion.identity);
        roomObject.GetComponent<RoomCordinate>().x = x;
        roomObject.GetComponent<RoomCordinate>().y = y;
        roomObject.GetComponent<RoomCordinate>().roomType = roomType;
        return roomObject;
    }
    //초기화
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
