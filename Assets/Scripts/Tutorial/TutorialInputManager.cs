using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class TutorialInputManager : MonoBehaviour
{
    [SerializeField] TutorialDialog dialogManager = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            dialogManager.NextDialog();
        }
    }
}
