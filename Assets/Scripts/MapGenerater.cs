using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerater : MonoBehaviour
{
    //맵 사이즈
    const int XSize = 10;
    const int YSize = 10;

    const int Xgap = 46;
    const int Ygap = 34;

    [SerializeField] int mapSize = 0;

    //방으로 사용할 프리펩리스트와 만들어진 방 오브젝트 리스트
    [SerializeField] GameObject mapObjectPrefab;
    [SerializeField] List<GameObject> mapObjectList;

    //랜덤맵 생성을 위한 변수
    bool[,] mapGenPosition = new bool[XSize, YSize];
    List<(int, int)> mapGenPositionIndexList = new List<(int, int)>();

    // Start is called before the first frame update
    void Start()
    {
        InitiallizeMapGenPosition();

        //시작지점
        mapGenPosition[5, 5] = true;
        mapGenPositionIndexList.Add((5, 5));
        Instantiate(mapObjectPrefab, new Vector3(0, 0, 0), Quaternion.identity);

        GenerateMapPoint(mapSize);
    }

    // Update is called once per frame
    void Update()
    {

    }

    //방 구조 생성
    void GenerateMapPoint(int mapSize)
    {
        for (int i = 0; i < mapSize; i++)
        {
            (int, int) tempPoint = SelecteMapPosition();
            mapGenPosition[tempPoint.Item1, tempPoint.Item2] = true;
            mapGenPositionIndexList.Add(tempPoint);
            mapObjectList.Add(Instantiate(mapObjectPrefab, new Vector3((tempPoint.Item1 - 5) * Xgap, 0, (tempPoint.Item2 - 5) * Ygap), Quaternion.identity));
        }
        

    }

    //초기화
    void InitiallizeMapGenPosition()
    {
        for (int i = 0; i < XSize; i++)
            for (int j = 0; j < YSize; j++)
                mapGenPosition[i, j] = false;

        mapGenPositionIndexList.Clear();
    }

    //랜덤한 방 위치 선정
    (int, int) SelecteMapPosition()
    {
        (int, int) tempPoint = mapGenPositionIndexList[Random.Range(0, mapGenPositionIndexList.Count)];
        int tempX = tempPoint.Item1;
        int tempY = tempPoint.Item2;
        int tempDirection = Random.Range(1, 5); // 1 : 왼쪽, 2 : 오른쪽, 3 : 위, 4 : 아래
        switch (tempDirection)
        {
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
