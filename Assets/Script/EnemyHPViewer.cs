using UnityEngine;
using UnityEngine.UI;

public class EnemyHPViewer : MonoBehaviour
{
    private EnemyStat enemyStat;
    public Slider hpSlider;
    void Start()
    {
        
    }

    public void Setup(EnemyStat enemyStat)
    {
        hpSlider = GetComponent<Slider>();
        this.enemyStat = enemyStat;
    }

    // Update is called once per frame
    void Update()
    {
       hpSlider.value= enemyStat.Hp/enemyStat.MaxHp;
    }
}
