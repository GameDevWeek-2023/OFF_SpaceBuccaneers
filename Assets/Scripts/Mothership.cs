using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Mothership : MonoBehaviour
{
    [SerializeField] public GameManager.Player owner;
    [SerializeField] public int maxHealth;
    [SerializeField] private UnitClass mothershipObject;
    
    public UnityEvent OnDeath = new UnityEvent();

    public int CurrentHealth { get; set; }
    
    private void Awake()  
    {
        switch (owner)
        {
            case GameManager.Player.PlayerOne:
                GameManager.Instance.PlayerOneMothership = this;
                break;
            case GameManager.Player.PlayerTwo:
                GameManager.Instance.PlayerTwoMothership = this;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        CurrentHealth = maxHealth;
    }

    private void Start()
    {
        var unit = GetComponent<Unit>();
        unit.Owner = owner;
        unit.UnitClass = mothershipObject;
        unit.BehaviourScript = gameObject.AddComponent<StandStillAI>();
    }

    public int TakeDamage(int amount)
    {
        CurrentHealth -= amount;
        
        UIManager.Instance.UpdateMotherShipHealth(owner, (float )CurrentHealth / maxHealth);

        if (CurrentHealth <= 0)
        {
            Die();
        }

        return CurrentHealth;
    }
    
    private void Die()
    {
        OnDeath.Invoke();
        GameManager.Instance.MothershipDestroyed(this);
        // TODO: Add death animation
    }
}
