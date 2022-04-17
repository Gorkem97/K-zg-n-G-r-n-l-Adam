using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Transform target;
    private NavMeshAgent naw_mesh_agent;
    private void Awake()
    {
        naw_mesh_agent = GetComponent<NavMeshAgent>();
        naw_mesh_agent.enabled = false;
    }
    // Update is called once per frame
    
    private void Follow_target()
    {
        naw_mesh_agent.enabled = true;
        naw_mesh_agent.destination = target.position;
        gameObject.GetComponent<SphereCollider>().radius = 30f;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Follow_target();
        }
    }
    
}
