using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNoise : MonoBehaviour
{
    public SphereCollider noiseCollider;

    [SerializeField] private float idleRadius;
    [SerializeField] private float crouchRadius;
    [SerializeField] private float jogRadius;
    [SerializeField] private float sprintRadius;

    public void setPlayerIdleNoise()
    {
        noiseCollider.radius = idleRadius;
    }

    public void setPlayerCrouchNoise()
    {
        noiseCollider.radius = crouchRadius;
    }

    public void setPlayerJogNoise()
    {
        noiseCollider.radius = jogRadius;
    }

    public void setPlayerSprintNoise()
    {
        noiseCollider.radius = sprintRadius;
    }
}
