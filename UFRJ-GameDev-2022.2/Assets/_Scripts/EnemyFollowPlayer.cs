using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowPlayer : MonoBehaviour
{

    private bool isFollowing;

    private Coroutine stopFollowCoroutine;

    [SerializeField] private float stopFollowDelay;

    public void StartFollow()
    {
        if (stopFollowCoroutine != null)
        {
            StopCoroutine(stopFollowCoroutine);

            stopFollowCoroutine = null;
        }

        if (!isFollowing)
        {
            isFollowing = true;
            // TODO fazer o inimigo se movimentar até o player
            Debug.Log("Following");
        }
    }

    public void StopFollow()
    {
        stopFollowCoroutine = StartCoroutine(StopFollowAfterTime(stopFollowDelay));
    }

    IEnumerator StopFollowAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        isFollowing = false;
        // TODO fazer o inimigo voltar para posição de ronda
        Debug.Log("Stopped Following");
    }
}
