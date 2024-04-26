
using System;
using System.Collections.Generic;
using UnityEngine;

public class CustomMonoBehavior : MonoBehaviour
{
    [SerializeField] private String entityType;
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float curentHealth;
    [SerializeField] public List<String> allyTags = new List<string>();

    public float MaxHealth { get => maxHealth; set => maxHealth = value; }
    public float CurentHealth { get => curentHealth; set => curentHealth = value; }
    public string EntityType { get => entityType; set => entityType = value; }
    public List<string> AllyTags { get => allyTags; set => allyTags = value; }

    // Start is called before the first frame update
    void Awake()
    {
        curentHealth = maxHealth;
        allyTags.Add(gameObject.tag);
        allyTags.Add("1");
        allyTags.Add("2");
        allyTags.Add("3");
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