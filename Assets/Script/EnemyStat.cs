using UnityEngine;

public class EnemyStat : MonoBehaviour
{
    public int MaxHp;
    //현재 체력
    public int Hp;
    public int MoveSpeed;
    public string type;

    public EnemyStauts enemyStat;


     private void Awake() {
        if(enemyStat==null){
            Debug.Log("스크립터블 오브젝트 없음");
            return;
        }else{
            Init();
        }
    }

    void Start(){
    }
    void Init(){
        string objectName = transform.name.Replace("(Clone)", "").Trim();
        for (int i = 0; i < enemyStat.EnemyDatas.Count;i++ )
        {
              if (enemyStat.EnemyDatas[i].Name == objectName)
            {
                MaxHp = enemyStat.EnemyDatas[i].MaxHp;
                Hp = enemyStat.EnemyDatas[i].MaxHp;
                type = enemyStat.EnemyDatas[i].type.ToString();
                MoveSpeed=enemyStat.EnemyDatas[i].MoveSpeed;
            }
         
        }
        
    }
   
}
