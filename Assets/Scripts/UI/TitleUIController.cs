using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleUIController : MonoBehaviour
{
    [SerializeField] Button startButton = null;
    [SerializeField] Button quitButton = null;

    // Start is called before the first frame update
    void Start()
    {
        startButton.onClick.AddListener(()=>{
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        });
        quitButton.onClick.AddListener(()=>{
            Application.Quit();
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
