using UnityEngine;

public class ShaderHpbarController : MonoBehaviour
{
    public Material healthBarMaterial; // 체력 바에 적용할 머티리얼
    public float maxHealth; // 최대 체력 값
    
    public float currentHealth; // 현재 체력 값

    private void Start() {
        maxHealth=transform.GetComponentInParent<EnemyStat>().MaxHp;
        
        
    }
    void Update()
    {
        currentHealth=transform.GetComponentInParent<EnemyStat>().Hp;
        // 현재 체력 값을 최대 체력 값으로 나누어 비율 계산
        float healthPercentage = currentHealth / maxHealth;

        // 체력 비율을 Shader Graph의 HP 프로퍼티로 전달
        healthBarMaterial.SetFloat("_HP", healthPercentage);
        healthBarMaterial.SetFloat("_MaxHP", maxHealth);
    }
}
