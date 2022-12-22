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

    [SerializeField] Animator animator;

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

            transform.LookAt(_patrolPositions[index].position);
            transform.position = Vector3.MoveTowards(transform.position, _patrolPositions[index].position, _speedPatrol);
            animator.Play("Male_Walk");
            if (transform.position.x == _patrolPositions[index].position.x && transform.position.z == _patrolPositions[index].position.z)
            {
                _currentPatrolIndex++;
                animator.Play("Male Idle");
                yield return new WaitForSeconds(_timeStoppedAtPatrolPosition);
            }
            yield return null;
        }
    }

    private IEnumerator AttackTarget(GameObject target)
    {
        transform.LookAt(target.transform.position);
        target.GetComponent<EntityHealth>().TakeDamage(34);

        _attacking = true;
        animator.Play("Male Attack 1");
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
            transform.LookAt(other.transform.position);
            animator.Play("Male Sprint");
            Debug.Log(Vector3.Distance(transform.position, other.transform.position));

            if (Vector3.Distance(transform.position, other.transform.position) > 3)
            {
                transform.position = Vector3.MoveTowards(transform.position, other.transform.position, _speedFollowPlayer);    
            }
            else
            {
                animator.Play("Male Idle");
                StartCoroutine(AttackTarget(other.gameObject));
            }
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
/*
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player") && !_attacking)
        {
            StartCoroutine(AttackTarget(collision.collider.gameObject));
        }
    }
    */
    
}
