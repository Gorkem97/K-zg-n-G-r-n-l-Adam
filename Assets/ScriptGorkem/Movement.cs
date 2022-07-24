
            using System.Collections;
            using System.Collections.Generic;
            using UnityEngine;
            using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
    {
        public CharacterController controller;

        public GameObject swordtrace;

    public AudioSource crush;

        Vector3 lastdirection;

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


    public Transform attackPoint;
    public float AttackRange;
    public LayerMask enemyLayers;

    public GameObject youdead;
    AudioSource hit;
        public Slider HealthSlid;
        public Slider StaminaSlid;

    public PickUpSystem pikap;

        bool attackmi = false;
        bool hareketmi = false;
        bool rollmu = false;
        bool dashmi = false;
    bool hitwait = false;

        public float smoothness = 60f;
        public float turnSmoothTime = 0.1f;
        float turnSmoothVelocity;

    public Animator anim;
        void Start()
        {
            staminawait = false;
            swordtrace.SetActive(false);
        hit = GameObject.Find("hit").GetComponent<AudioSource>();
        }

        // Update is called once per frame
        void Update()
        {
            HealthSlid.value = Health / 100;
            StaminaSlid.value = Stamina / 100;
        if (!attackmi && !rollmu && !dashmi && Health != 0.1f && !hitwait )
            {
                float horizontal = Input.GetAxisRaw("Horizontal");
                float vertical = Input.GetAxisRaw("Vertical");
                Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;
                lastdirection = direction;

                if (Input.GetKeyDown("y"))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
                if (Input.GetMouseButtonDown(0) && Stamina > 0 && pikap.has_item == true)
                {
                    StaminaGo(20);
                    anim.SetTrigger("atakmi");
                    StartCoroutine(Attackwait(0.1f, 0.7f));
                    Attack();
                    StartCoroutine(StaminaWaiting(Staminawaittime));
                    hareketmi = true;
                    attackmi = true;
                    AttackTurn();
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
                        StartCoroutine(dashwait(0.6f));
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

            if (Health < 100 && Health > 0.1f)
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
            anim.SetTrigger("hasar");
        if (!rollmu)
        {
            Health -= HowMuchDamage;
            hit.Play();
        }
        StartCoroutine(hitWaiting(0.8f));
            if (Health <= 0)
            {
                
                Health = 0.1f;
                anim.SetTrigger("olum");
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
            yield return new WaitForSeconds(movewaittime);
            hareketmi = false;
            yield return new WaitForSeconds(attackwaittime);
            attackmi = false;
            yield return new WaitForSeconds(0.7f);
            crush.Stop();
    }
        IEnumerator rollwait(float rollwait, float angle)
        {
            anim.SetTrigger("roll");
            yield return new WaitForSeconds(rollwait);
            rollmu = false;
        }
        IEnumerator dashwait(float dashwait)
        {
        anim.speed = 2;
            yield return new WaitForSeconds(dashwait);
        anim.speed = 1;
            dashmi = false;
        }
        IEnumerator StaminaWaiting(float howmuchstaminawait)
        {
            staminawait = true;
            yield return new WaitForSeconds(howmuchstaminawait);
            staminawait = false;
    }
    IEnumerator hitWaiting(float howmuchhitwait)
    {
        hitwait = true;
        yield return new WaitForSeconds(howmuchhitwait);
        hitwait = false;
    }
    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    void Attack()
    {
        
        Collider[] hitenemies = Physics.OverlapSphere(attackPoint.position, AttackRange, enemyLayers);
        
        foreach (Collider enemy in hitenemies)
        {
            
            if (enemy.tag == "Enemy" && Health != 0.1f)
            {
                enemy.GetComponent<EnemyMovement>().TakeDamage(20);
                crush.Play();
            }

        }
        

    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(attackPoint.position, AttackRange);
    }




}