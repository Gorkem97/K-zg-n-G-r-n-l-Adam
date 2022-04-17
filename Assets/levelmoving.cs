using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class levelmoving : MonoBehaviour
{
    public int level = 2;
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag ==  "Player")
        {
            Levelload();
        }
    }
    public void Levelload()
    {
        SceneManager.LoadScene(level);
    }
}
