using System.Collections.Generic;
using UnityEngine;

public class EnemySwpan : MonoBehaviour
{
    public List<GameObject> EnemyList;
    public float spawnInterval = 2.0f; // 소환 간격
    public int CurrentStage;
    public Transform swpanwPos;
    void Awake()
    {
         //오브젝트 초기화 
         GameObject[] prefabs =Resources.LoadAll<GameObject>("EnemyPrefabs");
         foreach (GameObject prefab in prefabs)
         {
            EnemyList.Add(prefab);
            Debug.Log("Loaded prefab: " + prefab.name);
         }
    }

    void Start()
    {
        
        InvokeRepeating("SpawnObject", 0f, spawnInterval);

    }
    void Update(){
        CurrentStage = transform.GetComponent<Timer>().CurrentStage;
    }

    void SpawnObject()
    {
        // 인덱스 유효성 검사
        if (CurrentStage <= 0 || CurrentStage > EnemyList.Count)
        {
            Debug.Log("Invalid stage or no enemy to spawn. CurrentStage: " + CurrentStage);
            return;
        }

        Debug.Log("CurrentStage: " + CurrentStage);
        // 추가로 스테이지가 달라지면 에너미도 달라지게 하기
        Quaternion newRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y+180 , transform.rotation.eulerAngles.z);
        Instantiate(EnemyList[CurrentStage - 1], swpanwPos.position, newRotation);
        
    }
}
