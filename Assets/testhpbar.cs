using UnityEngine;
using UnityEngine.UI;

public class testhpbar : MonoBehaviour
{
    public Transform target;  // 고정할 오브젝트
    public RectTransform uiElement;  // HP 바 UI 요소 (RectTransform)
    private Camera mainCamera;

    public Vector3 offset;  // 오브젝트 위치로부터의 오프셋
    private float maxHealth;
    private float currentHealth; 

    public Slider hpbar;
    

    void Start()
    {
         mainCamera = Camera.main;
         target=transform;
         maxHealth= transform.GetComponent<EnemyStat>().MaxHp;  // 슬라이더의 최대 값을 최대 HP로 설정
         currentHealth =maxHealth;
         
    }

    // Update is called once per frame
    void Update()
    {
        currentHealth =transform.GetComponent<EnemyStat>().Hp;
        if (target != null && uiElement != null)
        {
            Vector3 targetPositionWithOffset = target.position + offset;
            // 월드 공간의 오브젝트 위치를 스크린 공간 좌표로 변환
            Vector3 screenPos = mainCamera.WorldToScreenPoint(targetPositionWithOffset);
            // 스크린 공간 좌표를 UI 요소의 위치로 설정
            uiElement.position = screenPos;
        }
        HandleHp();
    }

    void HandleHp(){
        hpbar.value= currentHealth/maxHealth;
    }

}
