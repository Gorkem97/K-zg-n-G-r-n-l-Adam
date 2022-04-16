using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSystem : MonoBehaviour
{
    public GameObject hands; //reference to your hands/the position where you want your object to go
    bool can_pick_up; //a bool to see if you can or cant pick up the item
    GameObject object_that_pick_up; // the gameobject onwhich you collided with
    private bool has_item; // a bool to see if you have an item in your hand
    public bool can_destruct;
    public float drop_forward_force, drop_upward_force;         // Start is called before the first frame update

    // Start is called before the first frame update
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
            object_that_pick_up.GetComponent<Rigidbody>().AddForce(hands.transform.forward * drop_forward_force, ForceMode.Impulse);
            object_that_pick_up.GetComponent<Rigidbody>().AddForce(hands.transform.up * drop_upward_force, ForceMode.Impulse);

            float random = Random.Range(-1f, 1f);
            object_that_pick_up.GetComponent<Rigidbody>().AddTorque(new Vector3(random, random, random) * 10);
            has_item = false;
            object_that_pick_up.transform.parent = null;

        }
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
