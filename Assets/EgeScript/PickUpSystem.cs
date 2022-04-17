using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PickUpSystem : MonoBehaviour
{
    public GameObject hands; 
    bool can_pick_up; 
    GameObject object_that_pick_up; 
    private bool has_item; 
    public bool can_destruct;
    public float drop_forward_force, drop_upward_force;

    void Start()
    {
        can_pick_up = false;
        has_item = false;
        can_destruct = false;
    }
    
    // Update is called once per frame
    
    void Update()
    {
        if (can_pick_up)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                object_that_pick_up.GetComponent<Rigidbody>().isKinematic = true;
                object_that_pick_up.transform.position = hands.transform.position;
                object_that_pick_up.transform.parent = hands.transform;
                has_item = true;
                can_destruct = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.Q) && has_item)
        {
            object_that_pick_up.GetComponent<Rigidbody>().isKinematic = false;
            object_that_pick_up.GetComponent<Rigidbody>().AddForce(GameObject.FindGameObjectWithTag("Player").gameObject.transform.forward * drop_forward_force, ForceMode.Impulse);
            object_that_pick_up.GetComponent<Rigidbody>().AddForce(GameObject.FindGameObjectWithTag("Player").gameObject.transform.up * drop_upward_force, ForceMode.Impulse);

            float random = Random.Range(-1f, 1f);
            object_that_pick_up.GetComponent<Rigidbody>().AddTorque(new Vector3(random, random, random) * 10);
            has_item = false;
            object_that_pick_up.transform.parent = null;

        }
    }
    Collider GetClosestEnemyCollider(Vector3 unitPosition, Collider[] enemyColliders)
    {
        float bestDistance = 99999.0f;
        Collider bestCollider = null;

        foreach (Collider enemy in enemyColliders)
        {
            float distance = Vector3.Distance(unitPosition, enemy.transform.position);

            if (distance < bestDistance)
            {
                bestDistance = distance;
                bestCollider = enemy;
            }
        }

        return bestCollider;
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "pick_up_item")
        {

            can_pick_up = true;
            object_that_pick_up = other.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        can_pick_up = false;
    }
}
