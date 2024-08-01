using UnityEngine;
using System.Collections.Generic;

public class AniController : MonoBehaviour
{
    private Animator Anim;
    private static readonly int MoveState = Animator.StringToHash("Base Layer.move");
    private static readonly int DissolveState = Animator.StringToHash("Base Layer.dissolve");
    [SerializeField] private SkinnedMeshRenderer[] MeshR;
    public bool DissolveFlg = false;

    private const int Dissolve = 1;
    private const int Move = 2;

    public bool isIdle;

    private Dictionary<int, bool> PlayerStatus = new Dictionary<int, bool>
    {
        {Dissolve, false },
    };
    private float Dissolve_value = 1;

    void Start()
    {
        Anim = this.GetComponent<Animator>();
    }

    void Update()
    {
        var followComponent = transform.GetComponent<Follow>();
        var enemyStatComponent = transform.GetComponent<EnemyStat>();

        if (followComponent.ismove && enemyStatComponent.Hp > 0)
        {
            Anim.Play(MoveState);
        }

        STATUS();

        if (enemyStatComponent.Hp <= 0 && !DissolveFlg)
        {
            Anim.CrossFade(DissolveState, 0.1f, 0, 0);
            DissolveFlg = true;
            followComponent.ismove = false;
        }

        if (DissolveFlg)
        {
            PlayerDissolve();
        }
    }

    private void STATUS()
    {
        var enemyStatComponent = transform.GetComponent<EnemyStat>();

        if (DissolveFlg && enemyStatComponent.Hp <= 0)
        {
            PlayerStatus[Dissolve] = true;
        }
        else if (!DissolveFlg)
        {
            PlayerStatus[Dissolve] = false;
        }
    }

    private void PlayerDissolve()
    {
        Dissolve_value -= Time.deltaTime;

        for (int i = 0; i < MeshR.Length; i++)
        {
            MeshR[i].material.SetFloat("_Dissolve", Dissolve_value);
        }

        if (Dissolve_value <= 0)
        {
            DissolveFlg = false; // Optionally stop further updates if needed
        }
    }
}
