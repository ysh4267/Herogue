using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialDialog : MonoBehaviour
{
    [SerializeField] GameObject dialogPanel;
    [SerializeField] Text dialogText;
    string[][] lines;
    int currentDialogIndex = 0;
    int currentLineIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        lines = new string[][]
        {
            // 튜토리얼 시작
            new string[] {
                "W, A, S, D를 눌러 캐릭터를 움직일 수 있습니다.",
                "방들을 돌아다니며 다음스테이지로 가는 길을 찾으세요.",
                "자 이제 앞의 방으로 이동해 보세요."
            },
            // 다음방 진입
            new string[] {
                "마우스를 클릭해 파이어볼을 발사 할 수 있습니다.",
                "앞의 허수아비를 공격해 보세요.",
                "방의 모든 몬스터를 쓰러트리기 전에는 방을 통과할 수 없습니다."
            },
            // 아이템방 진입
            new string[] {
                "마녀는 사과를 먹어 자신의 최대 체력을 늘릴 수 있습니다.",
                "몬스터에게 공격을 받으면 체력을 절반씩 잃습니다.",
            },
            // 마지막방 진입
            new string[] {
                "포탈을 통해 다음 스테이지로 이동할 수 있습니다.",
                "더 많은 지역을 탐사할지 다음스테이지로 빠르게 이동할지는 여러분들의 선택 입니다."
            }
        };
        dialogText.text = lines[currentDialogIndex][currentLineIndex];
        ActiveDialogWindow();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActiveDialogWindow() {
        Invoke("ZeroTimeScale", 0.5f);
    }

    void ZeroTimeScale() {
        dialogPanel.SetActive(true);
        Time.timeScale = 0;
    }
    
    void ReturnTimeScale() {
        Time.timeScale = 1;
    }

    public void DeactiveDialogWindow() {
        dialogPanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void NextDialog() {
        if (currentLineIndex >= lines[currentDialogIndex].Length - 1) {
            DeactiveDialogWindow();
            currentLineIndex = 0;
            currentDialogIndex++;
            if (currentDialogIndex >= lines.Length) return;
            dialogText.text = lines[currentDialogIndex][currentLineIndex];
            return;
        }
        if (currentDialogIndex >= lines.Length) return;
        currentLineIndex++;
        dialogText.text = lines[currentDialogIndex][currentLineIndex];
    }
}
