using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractingRigidbody : MonoBehaviour
{
    [SerializeField]
    private float force_magnitude;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rb = hit.collider.attachedRigidbody;
        if (rb != null)
        {
            rb.isKinematic = false;
            Vector3 force_direction = hit.gameObject.transform.position - transform.position;
            force_direction.y = 0;
            force_direction.Normalize();

            rb.AddForceAtPosition(force_direction * force_magnitude, transform.position, ForceMode.Impulse);
        }
    }
}
