using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public Animator enemianim;
    public float vurmayibekle;
    public float vuruszamani;
    public float bitiszamani;

    public bool isplayerin = false;

    public Transform attackPoint;
    public float AttackRange;
    public LayerMask enemyLayers;

    public GameObject hasargoster;

    bool atakvarmi = false;
    public Movement hasarvuram;
    [SerializeField] private Transform target;
    private NavMeshAgent naw_mesh_agent;
    private void Awake()
    {
       
        naw_mesh_agent = GetComponent<NavMeshAgent>();
        naw_mesh_agent.enabled = false;
    }
    // Update is called once per frame
    private void Update()
    {

        isplayerin = false;
        Attack();
    }
    private void Follow_target()
    {
        enemianim.SetTrigger("kosma");
        naw_mesh_agent.enabled = true;
        naw_mesh_agent.destination = target.position;
        gameObject.GetComponent<SphereCollider>().radius = 30f;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !atakvarmi && hasarvuram.Health != 0.1f)
        {
            Follow_target();
        }
    }

    void Attack()
    {
        Collider[] hitenemies = Physics.OverlapSphere(attackPoint.position, AttackRange, enemyLayers);

        foreach (Collider player in hitenemies)
        {
                isplayerin = false;
                Debug.Log("Allah!" + player.name);
                if (player.name == "Player" && !atakvarmi && hasarvuram.Health != 0.1f)
                {
                   StartCoroutine(AttackTime(vurmayibekle, vuruszamani, bitiszamani));
                }
                if (player.name == "Player" && hasarvuram.Health != 0.1f)
                {
                    isplayerin = true;
                }

        }
        
        if ( hasarvuram.Health == 0.1f)
        {
            enemianim.SetTrigger("durma");
        }

    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(attackPoint.position, AttackRange);
    }
    IEnumerator AttackTime(float beklenenvurma, float ilkvurma,float vurmadanbekleme)
    {
        atakvarmi = true;
        yield return new WaitForSeconds(beklenenvurma);
        enemianim.SetTrigger("vurma");
        yield return new WaitForSeconds(ilkvurma);
        if (isplayerin == true)
        {
            hasarvuram.GotHit(40);
        }
        hasargoster.SetActive(true);
        yield return new WaitForSeconds(vurmadanbekleme);
        hasargoster.SetActive(false);
        atakvarmi = false;

    }

}
