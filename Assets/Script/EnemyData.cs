using UnityEngine;

[System.Serializable]
public class EnemyData 
{
   public string Name;
   public int MoveSpeed;
   public enum Type{Normal,siege , Penetrate};
   public Type type;
   public int MaxHp;
}
