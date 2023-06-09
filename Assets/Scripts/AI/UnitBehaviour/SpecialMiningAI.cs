using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpecialMiningAI : MonoBehaviour, IAIBehaviour
{ 
    private enum MiningState { GoingTo, Mining, Waiting, Setup };
    private MiningState _state = MiningState.Setup;
    private NavMeshAgent _agent;
    private Unit _unit;

    private GameObject _currentasteroid = null;
    Asteroid _currentAsteroidManager = null;

    public void UpdateState()
    {
        if (_unit.Dead || !_agent.enabled || _unit.Stunned) return;
        if (_state == MiningState.GoingTo)
        {
            if (_agent.remainingDistance < _unit.UnitClass.MiningRange)
            {
                _state = MiningState.Mining;
                StartCoroutine(MiningCoroutine());
            }
        }
        else if (_state == MiningState.Mining)
        {
            // do nothing because coroutine is running
        }
        else if (_state == MiningState.Waiting)
        {
            FindAndGoToClosestSpecialAsteroid();
        }
    }
    
    private IEnumerator MiningCoroutine()
    {
        while (true)
        {
            var remaining = _currentAsteroidManager.Mine(_unit.UnitClass.MiningRate);
            
            GameManager.Instance.AddResource(_unit.Owner, GameManager.ResourceType.Crystals, _unit.UnitClass.MiningRate);
            
            if (remaining <= 0 || _currentAsteroidManager.Dead)
            {
                _state = MiningState.Waiting;
                yield break;
            }
            yield return new WaitForSeconds(_unit.UnitClass.MiningTimeUnitLength);
        }
    }
    
    
    void IAIBehaviour.Start()
    {
        _unit = GetComponent<Unit>();
        _agent = GetComponent<NavMeshAgent>();
        FindAndGoToClosestSpecialAsteroid();
    } 
    
    private IEnumerator WaitForNavPath()
    {
        yield return new WaitUntil(() => _agent.hasPath);
        _state = MiningState.GoingTo;
    }

    private void FindAndGoToClosestSpecialAsteroid()
    {
        if (AsteroidManager.Instance.SpecialAsteroids.Count == 0)
        {
            _state = MiningState.Waiting;
            return;
        }
        GameObject tMin = null;
        var minDist = Mathf.Infinity;
        
        foreach (var asteroid in AsteroidManager.Instance.SpecialAsteroids)
        {
            if (_currentasteroid != null && _currentasteroid == asteroid.gameObject) continue;

            if (asteroid == null) continue;
            var tmpManager = asteroid.GetComponent<Asteroid>();
            if (tmpManager == null || tmpManager.Dead) continue;
    
            var dist = Vector3.Distance(asteroid.gameObject.transform.position, gameObject.transform.position);
            if (!(dist < minDist)) continue;
            tMin = asteroid.gameObject;
            minDist = dist;
        }

        if (tMin == null)
        {
            _state = MiningState.Waiting;
            return;
        }
        _currentasteroid = tMin;
        _currentAsteroidManager = tMin.GetComponent<Asteroid>();
        _agent.SetDestination(tMin.transform.position);
        StartCoroutine(WaitForNavPath());
    }
}
