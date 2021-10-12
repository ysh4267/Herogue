using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerater : MonoBehaviour {
    //맵 사이즈
    const int XSize = 10;
    const int YSize = 10;

    const int Xgap = 48;
    const int Ygap = 32;

    [SerializeField] int mapSize = 0;

    //방으로 사용할 프리펩리스트와 만들어진 방 오브젝트 리스트
    [SerializeField] GameObject mapObjectPrefab;
    [SerializeField] List<GameObject> mapObjectList;

    //랜덤맵 생성을 위한 변수
    bool[,] mapGenPosition = new bool[XSize + 1, YSize + 1];
    List<(int, int)> mapGenPositionIndexList = new List<(int, int)>();

    // Start is called before the first frame update
    void Start() {
        InitiallizeMapGenPosition();

        //시작지점추가
        mapGenPosition[5, 5] = true;
        mapGenPositionIndexList.Add((5, 5));
        //오브젝트 추가
        GameObject tempGameObject = Instantiate(mapObjectPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        tempGameObject.GetComponent<MapGenData>().SetMapPoint(5, 5);
        mapObjectList.Add(tempGameObject);

        GenerateMapPoint(mapSize);
    }

    // Update is called once per frame
    void Update() {

    }

    //방 구조 생성
    void GenerateMapPoint(int mapSize) {
        for (int i = 0; i < mapSize; i++) {
            (int, int) tempPoint = SelecteMapPosition(); //랜덤값 선택
            mapGenPosition[tempPoint.Item1, tempPoint.Item2] = true; //맵 좌표값 정보 저장
            mapGenPositionIndexList.Add(tempPoint); //맵 좌표값 리스트

            //맵 오브젝트 생성
            GameObject tempGameObject = Instantiate(mapObjectPrefab, new Vector3((tempPoint.Item1 - 5) * Xgap, 0, (tempPoint.Item2 - 5) * Ygap), Quaternion.identity);
            tempGameObject.GetComponent<MapGenData>().SetMapPoint(tempPoint.Item1, tempPoint.Item2); //맵 데이터 입력
            mapObjectList.Add(tempGameObject);
        }

        foreach (var data in mapObjectList) {
            int tempX = data.GetComponent<MapGenData>().x;
            int tempY = data.GetComponent<MapGenData>().y;
            if (tempX > 0) {
                if (mapGenPosition[tempX - 1, tempY]) data.GetComponent<MapGenData>().leftRoom = true;
            }
            if (tempX < XSize) {
                if (mapGenPosition[tempX + 1, tempY]) data.GetComponent<MapGenData>().rightRoom = true;
            }
            if (tempY > 0) {
                if (mapGenPosition[tempX, tempY - 1]) data.GetComponent<MapGenData>().downRoom = true;
            }
            if (tempY < YSize) {
                if (mapGenPosition[tempX, tempY + 1]) data.GetComponent<MapGenData>().upRoom = true;
            }
        }

        foreach (var data in mapObjectList) {
            data.GetComponent<MapGenData>().DirectionalInfoUpdate();
        }
    }

    //초기화
    void InitiallizeMapGenPosition() {
        for (int i = 0; i < XSize; i++) {
            for (int j = 0; j < YSize; j++) {
                mapGenPosition[i, j] = false;
            }
        }

        mapGenPositionIndexList.Clear();
    }

    //랜덤한 방 위치 선정
    (int, int) SelecteMapPosition() {
        (int, int) tempPoint = mapGenPositionIndexList[Random.Range(0, mapGenPositionIndexList.Count)];
        int tempX = tempPoint.Item1;
        int tempY = tempPoint.Item2;
        int tempDirection = Random.Range(1, 5); // 1 : 왼쪽, 2 : 오른쪽, 3 : 위, 4 : 아래
        switch (tempDirection) {
            //왼쪽일때
            case 1:
                //더이상 왼쪽에 공간이 없으면 재귀
                if (tempX <= 0) return SelecteMapPosition();
                //그 왼쪽에 이미 배정된 방이 있으면 재귀
                if (mapGenPosition[tempX - 1, tempY]) return SelecteMapPosition();
                //둘다 해당 하지 않으면 왼쪽좌표값 리턴
                return (tempX - 1, tempY);
            case 2:
                if (tempX >= XSize) return SelecteMapPosition();
                if (mapGenPosition[tempX + 1, tempY]) return SelecteMapPosition();
                return (tempX + 1, tempY);
            case 3:
                if (tempY <= 0) return SelecteMapPosition();
                if (mapGenPosition[tempX, tempY - 1]) return SelecteMapPosition();
                return (tempX, tempY - 1);
            case 4:
                if (tempY >= YSize) return SelecteMapPosition();
                if (mapGenPosition[tempX, tempY + 1]) return SelecteMapPosition();
                return (tempX, tempY + 1);
            default:
                break;
        }
        return default;
    }

}
