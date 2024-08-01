using System.Collections.Generic;
using UnityEngine;

public class Char : MonoBehaviour 
{
    public float attackRange; // 공격 사거리
    public LayerMask enemyLayerMask; // 적 레이어 마스크
    public List<Transform> enemiesInRange = new List<Transform>(); // 사거리 내의 적 리스트
    public GameObject aim; // 공격할 적 
    private Animator animator; // Animator 컴포넌트 참조

    public int maxAmmo;
    public int currentAmmo;
    public string type;
    public int  attackDamage;
    public stat stat;

    public float baseAttackSpeed ;// 기본 공격속도
    public float currentAttackSpeed;// 현재 공격속도
    public float attackTimer = 0.0f; // 발사 타이머
    public float attackInterval =1f; // 발사 주기 1초기준
    public float rotationSpeed = 8f; // 회전 속도

    public bool isRest;

    private void Awake() {
    string objectName = transform.name.Replace("(Clone)", "").Trim();
    for (int i = 0; i < stat.CharacterDatas.Count; i++)
        {
            if (stat.CharacterDatas[i].Name == objectName)
            {
                maxAmmo = stat.CharacterDatas[i].MaxAmmo;
                currentAmmo = stat.CharacterDatas[i].MaxAmmo;
                type = stat.CharacterDatas[i].type.ToString();
                attackRange=stat.CharacterDatas[i].AttackRange;
                attackDamage=stat.CharacterDatas[i].Damage;
                baseAttackSpeed =stat.CharacterDatas[i].AttackSpeed;
                currentAttackSpeed = baseAttackSpeed;
            }
        }
        Debug.Log("Aawke");
    }
    void Start()
    {
        animator = GetComponent<Animator>(); // Animator 컴포넌트 가져오기
    }
    void Update()
    {
        
        UpdateAttackInterval();
        if(!isRest){
            Attackcycle();
            animator.enabled = true;
        }else if(isRest){
            animator.enabled = false;
        }
    }
    void UpdateAttackInterval(){
        attackInterval = 1.0f / currentAttackSpeed;
    }

    void Attackcycle(){
         // 사거리 내의 적 업데이트
        enemiesInRange = FindEnemiesInAttackRange();
        Transform closestEnemy = FindClosestEnemyInAttackRange();
        if (closestEnemy != null && currentAmmo > 0 )
        {
            var enemy = closestEnemy.GetComponent<EnemyStat>();
            if(enemy !=null && enemy.Hp > 0){
                // 가장 가까운 적에 대한 처리
                aim = closestEnemy.gameObject;
                animator.SetBool("isAttack", true);
                animator.SetFloat("AttackSpeed", CalculateAttackSpeed(attackInterval));
                SmoothLookAt(closestEnemy);
                Shoot();
                if(currentAmmo ==0){
                    reload();
                }
            }else{
                 // 적의 체력이 0 이하인 경우 공격하지 않음
                animator.SetBool("isAttack", false);
            }
        }
        else if(closestEnemy == null)
        {
            animator.SetBool("isAttack", false);
        }
    }

    void SmoothLookAt(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    void Shoot()
    {
        // 발사 타이머 감소
        attackTimer -= Time.deltaTime;
        
        // 타이머가 0 이하로 내려가면 총알을 발사하고 타이머 초기화
        if (attackTimer <= 0)
        {
            attackTimer = attackInterval; // 타이머 초기화
            currentAmmo--;
            aim.GetComponent<EnemyStat>().Hp-= attackDamage;
            // 총알이 없는 경우 처리 예시
            if (currentAmmo <= 0)
            {
                disablePar();
            }
        }
    }

    public List<Transform> FindEnemiesInAttackRange()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange, enemyLayerMask);
        List<Transform> enemies = new List<Transform>();

        foreach (Collider collider in hitColliders)
        {
            // 적 캐릭터를 찾았다면 Transform을 리스트에 추가
            if (collider.CompareTag("Enemy"))
            {
                enemies.Add(collider.transform);
            }
        }

        return enemies;
    }

    public Transform FindClosestEnemyInAttackRange()
    {
        Transform closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (Transform enemy in enemiesInRange)
        {
            float distance = Vector3.Distance(enemy.position, transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }

    // 사거리를 Gizmos로 시각화
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    void disablePar()
    {
        foreach (AnimatorControllerParameter parameter in animator.parameters)
        {
            // 파라미터 비활성화
            animator.SetBool(parameter.name, false); // bool 타입인 경우
        }
    }

    void reload(){
        animator.SetBool("isReload", true);
    }
    public void OnReloadComplete()
    {
        // 애니메이션 재생 완료 후 총알 재장전
        currentAmmo = maxAmmo; // 최대 총알 수로 재설정
        animator.SetBool("isReload", false); // 애니메이션 상태 초기화
        Debug.Log(transform.name +"실행완료");
    }

    public float CalculateAttackSpeed(float interval)
    {
        if (interval <= 0)
        {
            Debug.LogError("Shoot interval must be greater than zero.");
            return 0;
        }
        float speed = 1 / interval;
        return  Mathf.Round(speed *100f)/100f;
    }
}
