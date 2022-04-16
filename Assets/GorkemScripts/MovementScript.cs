using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    public CharacterController controller;

    public GameObject swordtrace;

    public float speed = 6f ;
    public float attackforwardspeed = 40f;
    public float rollspeed = 2f;
    
    bool attackmi = false;
    bool hareketmi = false;
    bool rollmu = false;

    public float smoothness = 60f;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    void Start()
    {
        swordtrace.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!attackmi )
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

            if (direction.magnitude > 0)
            {
                if (!rollmu)
                {
                    float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                    float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                    transform.rotation = Quaternion.Euler(0, angle, 0);
                    controller.Move(direction * speed * Time.deltaTime);

                    if (Input.GetKeyDown("space"))
                    {
                        rollmu = true;
                        StartCoroutine(rollwait(1));
                    }
                }
            }

        }
        if (hareketmi)
        {
            transform.position += transform.forward * Time.deltaTime * attackforwardspeed;
        }

        if (Input.GetMouseButtonDown(0) & attackmi == false)
        {
            hareketmi = true;
            attackmi = true;
            AttackTurn();
            StartCoroutine(Attackwait(0.1f,0.6f));
        }

    }
    float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
            return Mathf.Atan2(a.x - b.x, a.y - b.y) * Mathf.Rad2Deg;
    }
    void AttackTurn()
    {

        Vector3 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);

        Vector3 mouseOnScreen = (Vector3)Camera.main.ScreenToViewportPoint(Input.mousePosition);

        float angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);

        transform.rotation = Quaternion.Euler(new Vector3(0f, angle-180, 0f));
    }
    IEnumerator Attackwait(float movewaittime,float attackwaittime)
    {
        swordtrace.SetActive(true);
        yield return new WaitForSeconds(movewaittime);
        hareketmi = false;
        swordtrace.SetActive(false);
        yield return new WaitForSeconds(attackwaittime);
        attackmi = false;
    }
    IEnumerator rollwait(float rollwait)
    {
        yield return new WaitForSeconds(rollwait);
        rollmu = false;
    }


    
}
