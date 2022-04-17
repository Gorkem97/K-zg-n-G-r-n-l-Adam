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
    public List<GameObject> pickupsInRange = new List<GameObject>();
    public bool is_handed;

    private void Awake()
    {
        can_pick_up = false;
        has_item = false;
        can_destruct = false;
        is_handed = false;
    }
    private void FixedUpdate()
    {

        trigger_can_pick_up();

    }
    private void trigger_can_pick_up()
    {
        if (is_handed == false)
        {

            float best_dist = 99999f;
            GameObject nearest_object = null;
            foreach (GameObject pickup in pickupsInRange)
            {
                if (pickup != null)
                {
                    float dist = Vector3.Distance(pickup.transform.position, gameObject.transform.position);
                    if (dist < best_dist)
                    {
                        best_dist = dist;
                        nearest_object = pickup;
                    }
                }
                
            }
            Debug.Log(nearest_object);
            can_pick_up = true;
            object_that_pick_up = nearest_object;

        }
    }

        // Update is called once per frame
        void Update()
    {
        if (can_pick_up)
        {
            if (object_that_pick_up != null)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    object_that_pick_up.GetComponent<Rigidbody>().isKinematic = true;
                    object_that_pick_up.transform.position = hands.transform.position;
                    object_that_pick_up.transform.parent = hands.transform;
                    has_item = true;
                    is_handed = true;
                    can_pick_up = false;
                    can_destruct = true;
                }

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
            is_handed = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != null)
        {
            switch (other.gameObject.tag)
            {
                case "pick_up_item":
                    pickupsInRange.Add(other.gameObject);
                    break;
            }
        }

    }
    private void OnTriggerExit(Collider other)
    {

        switch (other.gameObject.tag)
        {
            case "pick_up_item":
                can_pick_up = false;
                pickupsInRange.Remove(other.gameObject);
                break;
        }

    }
    

    
}
