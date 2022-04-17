using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Blood : MonoBehaviour
{

    public GameObject[] blodPick;

    private int a;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
           
            takeDamage();
        }


    }

    void takeDamage()
    {
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, -this.transform.up, out hit))
        {
            a = Random.Range(0, 361);
            int pickedBlod = Random.Range(0, blodPick.Length);
            
            GameObject obj = Instantiate(blodPick[pickedBlod], hit.point, Quaternion.LookRotation(hit.normal));
            obj.transform.position += obj.transform.forward / 1000;
            obj.transform.rotation = Quaternion.Euler(90 ,transform.rotation.y , a);
        }
        
    }




}
