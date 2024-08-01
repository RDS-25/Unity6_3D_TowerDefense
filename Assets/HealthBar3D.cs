using UnityEngine;

public class HealthBar3D : MonoBehaviour
{
    [SerializeField] Transform hpBarFill;
    [SerializeField] Transform hpBarBackground;
    [SerializeField] float maxHealth = 100f;
    [SerializeField] float currentHealth = 100f;
    [SerializeField] Color fullHealthColor = Color.green;
    [SerializeField] Color lowHealthColor = Color.red;

    private Vector3 originalScale;
    private Vector3 originalPosition;

    void Start()
    {
        originalScale = hpBarFill.localScale;
        originalPosition = hpBarFill.localPosition;
        UpdateHealthBar();
    }

    void Update()
    {
        // 임시로 데미지를 주는 예시입니다. 실제 게임 로직에 따라 데미지를 처리하세요.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(10f);
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        // 체력에 따라 체력바 크기 조절
        float healthPercentage = currentHealth / maxHealth;
        Vector3 newScale = originalScale;
        newScale.x = originalScale.x * healthPercentage;
        hpBarFill.localScale = newScale;

        // 체력에 따라 체력바 위치 조절
        Vector3 newPosition = originalPosition;
        newPosition.x = originalPosition.x - (originalScale.x - newScale.x) / 2.0f;
        hpBarFill.localPosition = newPosition;

        // 체력에 따라 체력바 색상 변경
        hpBarFill.GetComponent<Renderer>().material.color = Color.Lerp(lowHealthColor, fullHealthColor, healthPercentage);
    }



}
