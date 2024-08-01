using UnityEngine;

public class TestScript : MonoBehaviour
{
    [SerializeField] Transform hpBarFill;
    [SerializeField] Transform hpBarBackground;
    [SerializeField] float maxHealth = 100f;
    [SerializeField] float currentHealth = 100f;
    [SerializeField] Color fullHealthColor = Color.green;
    [SerializeField] Color lowHealthColor = Color.red;

    private Vector3 originalScale;

    void Start()
    {
        originalScale = hpBarFill.localScale;
        UpdateHealthBar();
    }

    // Update is called once per frame
    void Update()
    {
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

        // 체력에 따라 체력바 색상 변경
        hpBarFill.GetComponent<Renderer>().material.color = Color.Lerp(lowHealthColor, fullHealthColor, healthPercentage);
    }
}
