using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonDataManager : MonoBehaviour {
    public static SingletonDataManager Instance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<SingletonDataManager>();
                if (instance == null) {
                    var instanceContainer = new GameObject("SingletonDataManager");
                    instance = instanceContainer.AddComponent<SingletonDataManager>();
                }
            }
            return instance;
        }
    }
    private static SingletonDataManager instance;

    public bool isBulletBounce = false;
}
