using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int enemyDamage = 10;
    public float runningSpeed = 1.5f;
    public bool facingRight;

    private Vector3 startPosition;

    private Rigidbody2D rigidBody;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        startPosition = this.transform.position;
    }
    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = startPosition;
    }

    private void FixedUpdate()
    {
        float currentRunningSpeed = runningSpeed;
        
        if(facingRight)
        {
            currentRunningSpeed = runningSpeed;
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            currentRunningSpeed = -runningSpeed;
            transform.eulerAngles = Vector3.zero;
        }

        if(GameManager.sharedInstance.currentGameState == GameState.inGame)
        {
            rigidBody.velocity = new Vector2(currentRunningSpeed, rigidBody.velocity.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Coin") || collision.CompareTag("Potion"))
        {
            return;
        }

        if(collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().CollectHealth(-enemyDamage);
            return;
        }

        facingRight = !facingRight;
    }
}
