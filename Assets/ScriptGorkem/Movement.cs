
            using System.Collections;
            using System.Collections.Generic;
            using UnityEngine;
            using UnityEngine.UI;

public class Movement : MonoBehaviour
    {
        public CharacterController controller;

        public GameObject swordtrace;

        Vector3 lastdirection;
        Transform adana;

        public float speed = 6f;
        public float yercekimihizi = 6f;
        public float attackforwardspeed = 40f;
        public float rollspeed = 2f;
        public float dashspeed = 30f;
        public float Staminawaittime = 0.5f;

        public float Health = 100;
        public float healthgainspeed = 0.01f;
        public float Stamina = 100;
        public float staminagainspeed = 0.2f;
        public bool staminawait;

        public GameObject youdead;
        public Slider HealthSlid;
        public Slider StaminaSlid;


        bool attackmi = false;
        bool hareketmi = false;
        bool rollmu = false;
        bool dashmi = false;

        public float smoothness = 60f;
        public float turnSmoothTime = 0.1f;
        float turnSmoothVelocity;

    public Animator anim;
        void Start()
        {
            staminawait = false;
            swordtrace.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            HealthSlid.value = Health / 100;
            StaminaSlid.value = Stamina / 100;
            if (!attackmi && !rollmu && !dashmi)
            {
                float horizontal = Input.GetAxisRaw("Horizontal");
                float vertical = Input.GetAxisRaw("Vertical");
                Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;
                lastdirection = direction;

                if (Input.GetKeyDown("y"))
                {
                    GotHit(31);
                }
                if (Input.GetMouseButtonDown(0) && Stamina > 0)
                {
                    StaminaGo(20);
                    StartCoroutine(StaminaWaiting(Staminawaittime));
                    hareketmi = true;
                    attackmi = true;
                    AttackTurn();
                    StartCoroutine(Attackwait(0.1f, 0.6f));
                }

                if (direction.magnitude > 0)
                {
                    float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                    float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                    transform.rotation = Quaternion.Euler(0, angle, 0);
                    controller.Move(direction * speed * Time.deltaTime);

                anim.SetBool("yuruyormu",true);

                    if (Input.GetKeyDown("space") && Stamina > 0)
                    {
                        StartCoroutine(StaminaWaiting(Staminawaittime));
                        rollmu = true;
                        StaminaGo(18);
                        StartCoroutine(rollwait(0.5f, angle));
                    }
                    if (Input.GetKeyDown("left shift") && Stamina > 0)
                    {
                        StartCoroutine(StaminaWaiting(Staminawaittime));
                        StaminaGo(30);
                        dashmi = true;
                        StartCoroutine(dashwait(0.35f));
                    }
                }
            if (direction.magnitude<=0)
            {
                anim.SetBool("yuruyormu", false);
            }
            }
            if (rollmu)
            {
                controller.Move(lastdirection * rollspeed * Time.deltaTime);
            }
            if (hareketmi)
            {
                controller.Move(transform.forward * Time.deltaTime * attackforwardspeed);
            }
            if (dashmi)
            {
                controller.Move(lastdirection * dashspeed * Time.deltaTime);
            }

        }
        private void FixedUpdate()
        {
            if (!hareketmi)
            {
                Vector3 dus = new Vector3(0, -1, 0);
                controller.Move(dus * yercekimihizi * Time.deltaTime);
            }

            if (Health < 100 && Health > 0)
            {
                Health += healthgainspeed * Time.deltaTime;
            }
            if (Health > 100)
            {
                Health = 100;
            }
            if (Stamina < 100 && Stamina >= -10)
            {
                if (staminawait == false)
                {
                    Stamina += staminagainspeed * Time.deltaTime;

                }
            }
            if (Stamina > 100)
            {
                Stamina = 100;
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

            transform.rotation = Quaternion.Euler(new Vector3(0f, angle - 180, 0f));
    }
        public void GotHit(float HowMuchDamage)
        {
            Health -= HowMuchDamage;
            if (Health <= 0)
            {
                Health = 0;
                youdead.SetActive(true);
            }
        }
        void StaminaGo(float HowmanyStamina)
        {
            Stamina -= HowmanyStamina;
            if (Stamina < -10)
            {
                Stamina = -10;
            }
        }
        IEnumerator Attackwait(float movewaittime, float attackwaittime)
        {
            anim.SetTrigger("atakmi");
            swordtrace.SetActive(true);
            yield return new WaitForSeconds(movewaittime);

            hareketmi = false;
            swordtrace.SetActive(false);
            yield return new WaitForSeconds(attackwaittime);
            attackmi = false;
        }
        IEnumerator rollwait(float rollwait, float angle)
        {
            transform.rotation = Quaternion.Euler(0, angle, -180);
            yield return new WaitForSeconds(rollwait);
            transform.rotation = Quaternion.Euler(0, angle, +180);
            rollmu = false;
        }
        IEnumerator dashwait(float dashwait)
        {
            yield return new WaitForSeconds(dashwait);
            dashmi = false;
        }
        IEnumerator StaminaWaiting(float howmuchstaminawait)
        {
            staminawait = true;
            yield return new WaitForSeconds(howmuchstaminawait);
            staminawait = false;
        }



    }