using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatEntity : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float curentHealth;

    public float MaxHealth { get => maxHealth; set => maxHealth = value; }
    public float CurentHealth { get => curentHealth; set => curentHealth = value; }

    // Start is called before the first frame update
    void Start()
    {
        curentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateHealth(float value)
    {
        curentHealth -= value;

        if (curentHealth <= 0) {gameObject.SetActive(false); curentHealth = maxHealth;}
        else if (curentHealth > maxHealth) curentHealth = maxHealth;
    }
}
