using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float stageDuration = 10.0f; // 1스테이지의 기본 시간 (초)
    private float timer = 0.0f; // 경과 시간을 저장할 변수
    public int CurrentStage;
    public int MaxStage;
    private bool gameClear = false; // 스테이지 클리어 여부

    public TMP_Text timerText; // UI 텍스트 객체 연결 (타이머 표시용)
    public TMP_Text stageText; // UI 텍스트 객체 연결 (타이머 표시용)


    void Awake(){
        CurrentStage=1; // 스테이지 초기화
    }
    void Start()
    {
        
        MaxStage = 10;
        timer = 0.0f; // 타이머 초기화
        gameClear = false; // 게임 클리어 상태 초기화
    }

    void Update()
    {
       if (!gameClear)
        {
            // 타이머 업데이트
            timer += Time.deltaTime;
            UpdateTimerUI();

            // 시간이 stageDuration 이상 지나면 다음 스테이지로 진행
            if (timer >= stageDuration)
            {
                timer = 0.0f; // 타이머 초기화
                CurrentStage++; // 다음 스테이지로
                Debug.Log("Stage " + CurrentStage + " completed!");

                // 다음 스테이지로 진행하는 로직 추가
                if (CurrentStage > MaxStage)
                {
                    gameClear = true;
                    Debug.Log("Game Clear!");
                    // 게임 클리어 처리 또는 다음 단계로 이동하는 로직을 여기에 추가
                }
                else
                {
                    // 다음 스테이지 설정 예시: 새로운 씬 로드
                    // 예: SceneManager.LoadScene("Stage" + currentStage);
                }
            }
        }
    }

    void UpdateTimerUI()
    {
        // UI에 타이머 텍스트 업데이트
        if (timerText != null)
        {
            
            float remainingTime = Mathf.Max(stageDuration - timer, 0.0f); // 남은 시간 계산
            timerText.text = string.Format("Time Left: {0:F1} seconds", remainingTime);
            stageText.text="Stage: "+CurrentStage.ToString();
        }
    }

    void CompleteStage()
    {
        gameClear = true;
        Debug.Log("Stage 1 completed!");

        // 여기에 다음 스테이지로 넘어가는 로직 추가
        // 예를 들어, 새로운 씬을 로드하거나 다음 단계로 이어질 동작을 추가합니다.
        // 다음 스테이지로 이동하는 방법은 게임의 구조에 따라 다를 수 있습니다.
    }
}
