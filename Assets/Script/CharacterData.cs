using UnityEngine;

[System.Serializable]
public class CharacterData 
{
   public string Name;
   public int MaxAmmo;
   public enum Type{Normal,Siege , Penetrate};
   public Type type;
   public int Damage;
   public float AttackSpeed;
   public float AttackRange;
}
