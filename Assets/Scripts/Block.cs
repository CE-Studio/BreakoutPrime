using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public Core core;

    public int hits = 1;

    void Start()
    {
        core = GameObject.FindWithTag("Core").GetComponent<Core>();
    }

    void Update()
    {
        
    }

    public void Hit(int hitsTaken = 1)
    {
        hits--;
        if (hits <= 0)
        {
            Destroy(gameObject);
        }
    }
}
