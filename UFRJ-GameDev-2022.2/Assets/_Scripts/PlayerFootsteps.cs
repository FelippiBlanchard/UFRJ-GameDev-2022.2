using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
{
    [SerializeField] private AudioSource footstepAudio;
    [SerializeField] private float jogDelay = 0.15f;
    [SerializeField] private float sprintDelay = 0.1f;
    [SerializeField] private float crouchDelay = 0.3f;

    public void PlayJogAudio()
    {
        if (!footstepAudio.isPlaying)
        {
            footstepAudio.PlayDelayed(0.15f);
        }
    }

    public void PlaySprintAudio()
    {
        if (!footstepAudio.isPlaying)
        {
            footstepAudio.PlayDelayed(0.1f);
        }
    }

    public void PlayCrouchAudio()
    {
        if (!footstepAudio.isPlaying)
        {
            footstepAudio.PlayDelayed(0.3f);
        }
    }
}
