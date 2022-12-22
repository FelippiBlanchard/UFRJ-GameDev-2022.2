using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyFollowPlayer : MonoBehaviour
{

    [SerializeField] private float _speedFollowPlayer;
    [SerializeField] private float _speedPatrol;
    [SerializeField] private float _timeStoppedAtPatrolPosition;
    [SerializeField] private float _timeStoppedAfterAttack;

    [SerializeField] private Transform[] _patrolPositions;
    private int _currentPatrolIndex;
    private bool _patrolling = true;
    private bool _attacking;
    private Coroutine _currentCOPatrol;
    
    [SerializeField] UnityEvent onEnemyTrigger;
    [SerializeField] UnityEvent onEnemyUntrigger;

    private void Start()
    {
        StartPatrol();
    }

    private void StopPatrol()
    {
        StopCoroutine(_currentCOPatrol);
    }
    private void StartPatrol()
    {
        _currentCOPatrol = StartCoroutine(CO_Patrol());
    }

    private IEnumerator CO_Patrol()
    {
        if (_patrolPositions.Length <= 0)
        {
            _patrolling = false;
        }
        while (_patrolling)
        {
            var index = _currentPatrolIndex % _patrolPositions.Length;
         
            transform.position = Vector3.MoveTowards(transform.position, _patrolPositions[index].position, _speedPatrol);  
            
            if (transform.position.x == _patrolPositions[index].position.x && transform.position.z == _patrolPositions[index].position.z)
            {
                _currentPatrolIndex++;
                yield return new WaitForSeconds(_timeStoppedAtPatrolPosition);
            }
            yield return null;
        }
    }

    private IEnumerator AttackTarget(GameObject target)
    {
        target.GetComponent<EntityHealth>().TakeDamage(34);

        _attacking = true;
        yield return new WaitForSeconds(_timeStoppedAfterAttack);
        _attacking = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopPatrol();
            onEnemyTrigger.Invoke();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !_attacking)
        {
            transform.position = Vector3.MoveTowards(transform.position, other.transform.position, _speedFollowPlayer); 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartPatrol();
            onEnemyUntrigger.Invoke();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player") && !_attacking)
        {
            StartCoroutine(AttackTarget(collision.collider.gameObject));
        }
    }
}
