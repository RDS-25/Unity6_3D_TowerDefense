using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemystat",menuName ="ScriptableObj/Enemystat",order =int.MaxValue)]
public class EnemyStauts : ScriptableObject
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public List<EnemyData> EnemyDatas =new List<EnemyData>();
}
