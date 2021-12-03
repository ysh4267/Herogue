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
        for (int i = 0; i < heartImageObjectList.Count; i++) {
            Destroy(heartImageObjectList[i]);
        }
        heartImageObjectList.Clear();

        for (int i = 0; i < MaxHp; i++) {
            heartImageObjectList.Add(Instantiate(heartImageObject, heartUIContainer.transform));
        }
        for (int i = 0; i < (CurrentHp / 2); i++) {
            heartImageObjectList[i].GetComponent<Image>().sprite = fullHeart;
        }
        if (CurrentHp % 2 == 1) heartImageObjectList[(CurrentHp / 2)].GetComponent<Image>().sprite = halfHeart;
    }
}
