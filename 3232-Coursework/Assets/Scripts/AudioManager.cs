using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource limitSFX;

    public void playLimitSFX()
    {
        limitSFX.Play();
    }
}
