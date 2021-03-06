using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoomGenerater : MonoBehaviour {
    //맵 사이즈
    const int XSize = 10;
    const int YSize = 10;

    const int Xgap = 48;
    const int Ygap = 32;

    [SerializeField] int roomSize = 0;

    //방으로 사용할 프리펩리스트와 만들어진 방 오브젝트 리스트
    [SerializeField] GameObject bossRoomObjectPrefab;
    [SerializeField] GameObject itemRoomObjectPrefab;
    [SerializeField] GameObject startRoomObjectPrefab;
    [SerializeField] List<GameObject> normalRoomObjectPrefab;
    List<RoomData> roomDataList = new List<RoomData>();
    List<GameObject> roomInstObjectList = new List<GameObject>();

    //랜덤맵 생성을 위한 변수
    bool[,] roomGenPosition = new bool[XSize + 1, YSize + 1];
    List<(int, int)> roomGenPositionIndexList = new List<(int, int)>();

    // Start is called before the first frame update
    void Start() {
        GenerateRoom();
    }

    // Update is called once per frame
    void Update() {

    }

    void GenerateRoom() {
        //초기화
        InitiallizeRoomGenPosition();
        InitiallizeRoomDataList();

        //센터에 방을 하나 만듬
        CreateStartRoom();
        //만들어진 스타트 룸 기준으로 문어발식 방 생성
        GenerateRoomPoint(roomSize);

        //보스생성
        CreateBossRoom(); 
        //일반방 생성  
        CreateNormalRoom(); 
        //Item방
        CreateItemRoom();   


        //모든 방을 특정하고 나서 화면상에 존재하는 모든 맵 오브젝트에 네비메쉬를 적용
        roomInstObjectList[0].GetComponent<NavMeshSurface>().BuildNavMesh();

        //상하좌우의 벽이나 문과같은 오브젝트 활성화
        foreach (var data in roomDataList) {
            data.DirectionalInfoUpdate();
        }
    }


    //모든방에서 네비매쉬를 따로 만들지 않아도 되어서 사용하지 않음
    void BuildRoomNavMash() {
        foreach (var item in roomInstObjectList)
        {
            item.GetComponent<NavMeshSurface>().BuildNavMesh();
        }
    }
    
    void CreateItemRoom() {
            foreach (RoomData roomData in roomDataList) {
            //시작지점이 아니라면
            if (roomData.roomType == RoomData.RoomType.NormalRoom) {
                //옆과 아랫칸이 비어있다면
                if (roomData.leftRoom == false && roomData.upRoom == false) {
                    //맵의 끝부분이 아니라면
                    if (roomData.x > 0 && roomData.y < YSize) {
                        //대각선이 비어있다면
                        if (roomGenPosition[roomData.x - 1, roomData.y + 1] == false) {
                            roomData.roomType = RoomData.RoomType.ItemRoom;
                            roomInstObjectList.Add(roomData.InstantiateRoomPrefab(itemRoomObjectPrefab));
                            return;
                        }
                    }
                    //맵의 끝부분이라면
                    else {
                        roomData.roomType = RoomData.RoomType.BossRoom;
                        roomInstObjectList.Add(roomData.InstantiateRoomPrefab(itemRoomObjectPrefab));
                        return;
                    }
                }
            }

        }
        //만족하는 방이 시작지점 뿐이었다면. 시작지점 오른쪽에 방을 추가함
        RoomData tempRoomData = new RoomData();
        tempRoomData.SetRoomPoint(6, 5);
        tempRoomData.roomType = RoomData.RoomType.ItemRoom;
        roomInstObjectList.Add(tempRoomData.InstantiateRoomPrefab(itemRoomObjectPrefab));
        roomDataList.Add(tempRoomData);
    }

    void CreateStartRoom() {
        //시작지점추가
        roomGenPosition[5, 5] = true;
        roomGenPositionIndexList.Add((5, 5));
        //데이터 추가
        RoomData tempRoomData = new RoomData();
        tempRoomData.SetRoomPoint(5, 5);
        tempRoomData.roomType = RoomData.RoomType.StartRoom;

        roomInstObjectList.Add(tempRoomData.InstantiateRoomPrefab(startRoomObjectPrefab));
        roomDataList.Add(tempRoomData);
    }

    //보스방 생성
    void CreateBossRoom() {
        foreach (RoomData roomData in roomDataList) {
            //시작지점이 아니라면
            if (roomData.roomType != RoomData.RoomType.StartRoom) {
                //옆과 아랫칸이 비어있다면
                if (roomData.rightRoom == false && roomData.downRoom == false) {
                    //맵의 끝부분이 아니라면
                    if (roomData.x < XSize && roomData.y > 0) {
                        //대각선이 비어있다면
                        if (roomGenPosition[roomData.x + 1, roomData.y - 1] == false) {
                            roomData.roomType = RoomData.RoomType.BossRoom;
                            roomInstObjectList.Add(roomData.InstantiateRoomPrefab(bossRoomObjectPrefab));
                            //roomData.DirectionalInfoUpdate();
                            return;
                        }
                    }
                    //맵의 끝부분이라면
                    else {
                        roomData.roomType = RoomData.RoomType.BossRoom;
                        roomInstObjectList.Add(roomData.InstantiateRoomPrefab(bossRoomObjectPrefab));
                        //roomData.DirectionalInfoUpdate();
                        return;
                    }
                }
            }

        }
        //만족하는 방이 시작지점 뿐이었다면. 시작지점 오른쪽에 방을 추가함
        RoomData tempRoomData = new RoomData();
        tempRoomData.SetRoomPoint(5, 6);
        tempRoomData.roomType = RoomData.RoomType.BossRoom;
        roomInstObjectList.Add(tempRoomData.InstantiateRoomPrefab(bossRoomObjectPrefab));
        roomDataList.Add(tempRoomData);
    }

    //노말방 생성
    void CreateNormalRoom() {
        foreach (var item in roomDataList) {
            if (item.roomType == RoomData.RoomType.NormalRoom) {
                roomInstObjectList.Add(item.InstantiateRoomPrefab(normalRoomObjectPrefab[Random.Range(0, normalRoomObjectPrefab.Count)])); //맵의 실질적인 오브젝트 프리펩 삽입
            }
        }
        
        //방들의 위치정보를 기반으로 길을 생성
        foreach (var data in roomDataList) {
            //data.DirectionalInfoUpdate();
        }
    }

    //방 구조 생성
    void GenerateRoomPoint(int roomSize) {
        //랜덤한 방을 생성
        for (int i = 0; i < roomSize; i++) {
            (int, int) tempPoint = SelecteRoomPosition(); //랜덤값 선택
            roomGenPosition[tempPoint.Item1, tempPoint.Item2] = true; //맵 좌표값 정보 저장
            roomGenPositionIndexList.Add(tempPoint); //맵 좌표값 리스트

            //비어있는 맵 데이터 클래스 생성
            RoomData tempRoomData = new RoomData();
            tempRoomData.SetRoomPoint(tempPoint.Item1, tempPoint.Item2); //맵의 좌표값 삽입
            roomDataList.Add(tempRoomData);
        }
        //방의 리스트정보로 방들간의 위치정보 업데이트
        foreach (var data in roomDataList) {
            int tempX = data.x;
            int tempY = data.y;
            if (tempX > 0) {
                if (roomGenPosition[tempX - 1, tempY]) data.leftRoom = true;
            }
            if (tempX < XSize) {
                if (roomGenPosition[tempX + 1, tempY]) data.rightRoom = true;
            }
            if (tempY > 0) {
                if (roomGenPosition[tempX, tempY - 1]) data.downRoom = true;
            }
            if (tempY < YSize) {
                if (roomGenPosition[tempX, tempY + 1]) data.upRoom = true;
            }
        }
    }

    //초기화
    void InitiallizeRoomGenPosition() {
        for (int i = 0; i < XSize; i++) {
            for (int j = 0; j < YSize; j++) {
                roomGenPosition[i, j] = false;
            }
        }

        roomGenPositionIndexList.Clear();
    }

    void InitiallizeRoomDataList() {
        roomDataList.Clear();
        roomInstObjectList.Clear();
        roomDataList = new List<RoomData>();
        roomInstObjectList = new List<GameObject>();
    }

    //랜덤한 방 위치 선정
    (int, int) SelecteRoomPosition() {
        (int, int) tempPoint = roomGenPositionIndexList[Random.Range(0, roomGenPositionIndexList.Count)];
        int tempX = tempPoint.Item1;
        int tempY = tempPoint.Item2;
        int tempDirection = Random.Range(1, 5); // 1 : 왼쪽, 2 : 오른쪽, 3 : 위, 4 : 아래
        switch (tempDirection) {
            //왼쪽일때
            case 1:
                //더이상 왼쪽에 공간이 없으면 재귀
                if (tempX <= 0) return SelecteRoomPosition();
                //그 왼쪽에 이미 배정된 방이 있으면 재귀
                if (roomGenPosition[tempX - 1, tempY]) return SelecteRoomPosition();
                //둘다 해당 하지 않으면 왼쪽좌표값 리턴
                return (tempX - 1, tempY);
            case 2:
                if (tempX >= XSize) return SelecteRoomPosition();
                if (roomGenPosition[tempX + 1, tempY]) return SelecteRoomPosition();
                return (tempX + 1, tempY);
            case 3:
                if (tempY <= 0) return SelecteRoomPosition();
                if (roomGenPosition[tempX, tempY - 1]) return SelecteRoomPosition();
                return (tempX, tempY - 1);
            case 4:
                if (tempY >= YSize) return SelecteRoomPosition();
                if (roomGenPosition[tempX, tempY + 1]) return SelecteRoomPosition();
                return (tempX, tempY + 1);
            default:
                break;
        }
        return default;
    }

}
