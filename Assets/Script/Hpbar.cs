using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class Hpbar : MonoBehaviour
{
    [SerializeField] Transform hp;
    [SerializeField] Camera cam;
    public Slider healthSlider;  // HP 바를 나타낼 슬라이더
    public float maxHealth;
    public float currentHealth; 
    void Start()
    {
        cam= Camera.main;
        // Quaternion q_hp =Quaternion.LookRotation(hp.position -cam.transform.position);
        // Vector3 hp_angle =Quaternion.RotateTowards(hp.rotation, q_hp, 200).eulerAngles;

        // hp.rotation =quaternion.Euler(hp_angle.x,0,0);
        maxHealth= transform.GetComponent<EnemyStat>().MaxHp;  // 슬라이더의 최대 값을 최대 HP로 설정
        
        
    }

    void Update(){
        hpbar();
    }

    void hpbar(){
        healthSlider.value = currentHealth/maxHealth;
    }

    // Update is called once per frame
    private void LateUpdate() {
        hp.LookAt(transform.position + cam.transform.rotation * Vector3.forward, cam.transform.rotation * Vector3.up);
    }
}