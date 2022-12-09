using UnityEngine;

/// <summary>
/// DISCLAIMER: 
/// This script is not created by me. Refer to README for the reference.
/// </summary>
public class FishInstantiator : MonoBehaviour
{
    public GameObject fishPrefab;

    [Range(0, 300)]
    public int number;

    private void Start()
    {
        for (int i = 0; i < number; i++)
        {
            Instantiate(fishPrefab, Vector3.zero, Quaternion.identity);
        }
    }
}