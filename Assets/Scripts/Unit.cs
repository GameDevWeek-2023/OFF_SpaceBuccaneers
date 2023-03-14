using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Unit : MonoBehaviour
{
    public enum UnitOwner
    {
        Player,
        Enemy
    }
    

    public UnitClass UnitClass { get; private set; }
    public UnitOwner Owner { get; private set; }
    public IAIBehaviour BehaviourScript { get; private set; }

    private int _currentHealth;
    private NavMeshAgent _navMeshAgent;
    private AttackScript _attackScript;
    
    public void Initialize(UnitClass unitClass, UnitOwner owner)
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _attackScript = GetComponent<AttackScript>();
        
        UnitClass = unitClass;
        Owner = owner;
        
        _currentHealth = UnitClass.Health;
        
        _navMeshAgent.speed = UnitClass.MoveSpeed;
        _navMeshAgent.stoppingDistance = UnitClass.AttackRange - 0.5f;
        
        _attackScript.Initialize(UnitClass.Attack, UnitClass.AttackCooldown, UnitClass.AttackRange);

        BehaviourScript = UnitClass.behaviour switch
        {
            UnitClass.AIBehaviourType.Passive => gameObject.AddComponent<PassiveAI>(),
            UnitClass.AIBehaviourType.Mining => gameObject.AddComponent<MiningAI>(),
            UnitClass.AIBehaviourType.Aggressive => gameObject.AddComponent<AggressiveAI>(),
            UnitClass.AIBehaviourType.StandStill => gameObject.AddComponent<StandStillAI>(),
            _ => BehaviourScript 
        };
        AIManager.Instance.AddUnit(gameObject);
        BehaviourScript.Start();
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}