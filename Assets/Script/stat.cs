
using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "stat",menuName ="ScriptableObj/stat",order =int.MaxValue)]
public class stat : ScriptableObject 
{
    public List<CharacterData> CharacterDatas =new List<CharacterData>();
}
