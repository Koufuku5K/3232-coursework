using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 3f;
    public float mass = 0.01f;

    public Animator animator;
    public GameObject gameOverScreen;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 force = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0.0f);

        animator.SetFloat("Horizontal", force.x);
        animator.SetFloat("Vertical", force.y);
        animator.SetFloat("Magnitude", force.magnitude);

        transform.position = transform.position + (force * (1 / mass)) * Time.deltaTime * Time.deltaTime;
    }

    public void OnCollisionEnter2D(Collision2D col)
    {
        // If the player hits a slime, the player dies and game over
        if (col.gameObject.tag == "Slime")
        {
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            gameObject.SetActive(false);
            gameOverScreen.SetActive(true);
            //SceneManager.LoadScene("GameOver", LoadSceneMode.Additive);
        }
    }
}
