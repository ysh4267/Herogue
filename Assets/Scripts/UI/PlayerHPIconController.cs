using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPIconController : MonoBehaviour {
    [SerializeField] Sprite emptyHeart;
    [SerializeField] Sprite fullHeart;
    [SerializeField] Sprite halfHeart;
    [SerializeField] GameObject heartImageObject;
    [SerializeField] GameObject heartUIContainer;

    List<GameObject> heartImageObjectList = new List<GameObject>();

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void HpImageUpdate(int MaxHp, int CurrentHp) {
        //체력아이콘 모두 초기화
        for (int i = 0; i < heartImageObjectList.Count; i++) {
            Destroy(heartImageObjectList[i]);
        }
        heartImageObjectList.Clear();

        //최대체력만큼 체력칸을 만듬
        for (int i = 0; i < MaxHp; i++) {
            heartImageObjectList.Add(Instantiate(heartImageObject, heartUIContainer.transform));
        }
        //체력칸 채우기
        for (int i = 0; i < (CurrentHp / 2); i++) {
            heartImageObjectList[i].GetComponent<Image>().sprite = fullHeart;
        }
        //홀수 일때 1개를 추가
        if (CurrentHp % 2 == 1) heartImageObjectList[(CurrentHp / 2)].GetComponent<Image>().sprite = halfHeart;
    }
}
