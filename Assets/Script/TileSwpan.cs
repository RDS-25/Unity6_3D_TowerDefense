using UnityEngine;

public class TileSwpan : MonoBehaviour
{
    public bool IsBuildTower;
    // public bool IsRest;
    public LayerMask layerMask;
    public Transform Tower;
    void Awake()
    {
        IsBuildTower = false;
    }

    void Update(){
        checkTower();
    }
    void checkTower(){
        float checkDistance = 2.0f;
        RaycastHit hit;
        Vector3 direction = Vector3.up; // 위쪽 방향

        if (Physics.Raycast(transform.position, direction, out hit, checkDistance, layerMask))
        {
            // 레이캐스트가 다른 오브젝트에 충돌했을 때
            IsBuildTower= true;
            Tower = hit.collider.transform;
        }
        else
        {
            // 레이캐스트가 아무것도 충돌하지 않았을 때
            IsBuildTower= false;
        }
    }
}
