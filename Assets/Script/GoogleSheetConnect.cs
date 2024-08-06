using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GoogleSheetConnect : MonoBehaviour
{
    public stat charstat;
    const string URL="https://docs.google.com/spreadsheets/d/1oboPiWBprG7bQLPACEvGmP8T56EwadVNiuRiER_lvhA/export?format=tsv&range=A2:F4"; 
    const string URL2="https://script.google.com/macros/s/AKfycbxrqUAVoSWQ8x9incire_UgDoP-RbRJiXeLgCSwmN9sxjWCasMips4q1eUPdP6Kvp5QPw/exec";
    IEnumerator Start()
    {
        UnityWebRequest www=UnityWebRequest.Get(URL);

        yield return www.SendWebRequest();

        string data = www.downloadHandler.text;
        SetSO(data);
    }
    void SetSO(string tsv){
        string[] row =tsv.Split('\n');
        int rowSize = row.Length;
        int columnSize =row[0].Split('\t').Length;

        for(int i = 0; i < rowSize; i++){
            string[] column = row[i].Split('\t');
            for(int j =0; j < columnSize; j++){
                charstat.CharacterDatas[i].Name = column[0];
                charstat.CharacterDatas[i].MaxAmmo =int.Parse(column[1]);
            }
        }
    }


}
