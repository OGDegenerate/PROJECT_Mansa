using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class health_system : MonoBehaviour
{
    public int health; //keeps track of Mansa's health
    public int maxHealth = 10000; //how much health Mansa has when he's at full health

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth; //sets Mansa's current health to FULL
    }

    public void TakeDamage(int amount) //this function will be called anytime Mansa takes damage. amount = how much damage Mansa takes
    {
        health -= amount;
        if(health <= 0) //if the damage takes mansa down to zero (or below), then he will be destroyed
        {
            Destroy(gameObject);
        }
    }


}

