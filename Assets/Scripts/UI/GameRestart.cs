using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameRestart : MonoBehaviour
{
    [SerializeField] Button RestartButton = null;
    // Start is called before the first frame update
    void Start()
    {
        RestartButton.onClick.AddListener(()=>{
            Time.timeScale = 1.0f;
            SceneManager.LoadScene("Stage1");
        });
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
