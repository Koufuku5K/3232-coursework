using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitAudio : MonoBehaviour
{
    public AudioSource limitSFX;

    public void playLimitSFX()
    {
        limitSFX.Play();
    }
}
