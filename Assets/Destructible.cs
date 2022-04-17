using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    [SerializeField]
    private GameObject destroyedVersion;
    [SerializeField]
    private float break_force;

    PickUpSystem pick_up_system;

    private void Awake()
    {

        pick_up_system = GameObject.FindGameObjectWithTag("Player").GetComponent<PickUpSystem>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("ground") && pick_up_system.can_destruct)
        {
            Break_thing();
            Destroy(gameObject);

        }
    }
    private void Break_thing()
    {
        GameObject broken_object = Instantiate(destroyedVersion, transform.position, transform.rotation);

        foreach (Rigidbody rb in broken_object.GetComponentsInChildren<Rigidbody>())
        {
            Vector3 force = (rb.transform.position - transform.position).normalized * break_force;
            rb.AddForce(force);
        }
        pick_up_system.can_destruct = false;
    }
}

