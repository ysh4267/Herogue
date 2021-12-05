using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomItemCreater : MonoBehaviour
{
    [SerializeField] GameObject itemSpawnPoint = null;
    [SerializeField] List<GameObject> itemPrefabList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(itemPrefabList[Random.Range(0, itemPrefabList.Count)], itemSpawnPoint.transform.position, Quaternion.identity, itemSpawnPoint.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
