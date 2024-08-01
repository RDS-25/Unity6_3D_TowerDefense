using UnityEngine;
using System.Collections;
using NUnit.Framework;

public class Follow : MonoBehaviour
{
    [SerializeField]
    private Transform target;                   
    [SerializeField]
    private Transform[] wayPoints;            
    [SerializeField]
    private float waitTime; 
    //이동속도            
    [SerializeField]
    private float unitPerSecond = 1;        
    [SerializeField]
    private bool isPlayOnAwake = true;  
    [SerializeField]
    private bool isLoop = true;       

    public int wayPointCount;         
    public int currentIndex = 0;      
    CapsuleCollider capsuleCollider;

    public bool ismove;
    private void Start()
    {
        FindWayPoint();
        wayPointCount = wayPoints.Length;
        unitPerSecond= transform.GetComponent<EnemyStat>().MoveSpeed;

        if (target == null) target = transform;
        if (isPlayOnAwake == true) Play();
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    
    void Update(){
        DestoryEnmey();
    }
    //돌아가는 것 초기화 
    void FindWayPoint(){
        // WayPoint 게임 오브젝트를 찾습니다.
        GameObject wayPointParent = GameObject.Find("WayPoint");

        if (wayPointParent != null)
        {
            // WayPoint의 자식 오브젝트 수를 가져옵니다.
            int childCount = wayPointParent.transform.childCount;

            // 자식 오브젝트들을 저장할 배열을 초기화합니다.
            wayPoints = new Transform[childCount];

            // 자식 오브젝트들을 배열에 추가합니다.
            for (int i = 0; i < childCount; i++)
            {
                wayPoints[i] = wayPointParent.transform.GetChild(i);
            }

            // 디버그 로그를 통해 확인합니다.
            Debug.Log("Found " + wayPoints.Length + " waypoints.");
        }
        else
        {
            Debug.LogError("WayPoint object not found!");
        }
    }

 

    void DestoryEnmey(){
        int Hp =  transform.GetComponent<EnemyStat>().Hp ;
        if(Hp <= 0){
        StopAllCoroutines();
        capsuleCollider.enabled = false;
        }
    }

    public void Play() => StartCoroutine(nameof(Process), ismove=true);

    private IEnumerator Process()
    {
        var wait = new WaitForSeconds(waitTime);

        while (true)
        {
            
            yield return StartCoroutine(MoveAToB(target.position, wayPoints[currentIndex].position));

            
            if (currentIndex < wayPointCount - 1)
            {
                currentIndex++;
            }
            else
            {
                if (isLoop == true) currentIndex = 0;
                else break;
            }

            
            yield return wait;
        }

        
    }

    private IEnumerator MoveAToB(Vector3 start, Vector3 end)
    {
        float percent = 0;

   
        float moveTime = Vector3.Distance(start, end) / unitPerSecond;

        // Debug.Log($"이동거리 : {Vector3.Distance(start, end)}, 움직인 시간 : {moveTime}");

        while (percent < 1)
        {
            percent += Time.deltaTime / moveTime;

            target.position = Vector3.Lerp(start, end, percent);
            transform.LookAt(wayPoints[currentIndex]);
            

            yield return null;
        }
    }
}
