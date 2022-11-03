using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundaries : MonoBehaviour
{
    private Vector2 screenBoundaries;
    private float playerWidth;
    private float playerHeight;

    // Start is called before the first frame update
    void Start()
    {
        screenBoundaries = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 
            Screen.height, Camera.main.transform.position.z));
        playerWidth = transform.GetComponent<SpriteRenderer>().bounds.size.x / 2;
        playerHeight = transform.GetComponent<SpriteRenderer>().bounds.size.y / 2;

    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, screenBoundaries.x * -1 + playerWidth, screenBoundaries.x - playerWidth);
        pos.y = Mathf.Clamp(pos.y, screenBoundaries.y * -1 + playerHeight, screenBoundaries.y - playerHeight);
        transform.position = pos;
    }
}
