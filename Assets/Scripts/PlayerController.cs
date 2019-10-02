using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 6f;
    public float runningSpeed = 2f;

    public LayerMask groundMask;

    private Rigidbody2D playerRigidbody;
    private Animator animator;
    private Vector3 startPosition;

    private const string STATE_ALIVE = "isAlive";
    private const string STATE_ON_THE_GROUND = "isOnTheGround";

    private int healthPoints, manaPoints;
    public const int INITIAL_HEALTH = 100, 
        INITIAL_MANA = 15, 
        MAX_HEALTH = 200, 
        MAX_MANA = 30, 
        MIN_HEALTH = 10, 
        MIN_MANA = 0;

    void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    public void StartGame()
    {
        animator.SetBool(STATE_ALIVE, true);
        animator.SetBool(STATE_ON_THE_GROUND, true);
        
        healthPoints = INITIAL_HEALTH;
        manaPoints = INITIAL_MANA;

        Invoke("RestartPosition", 0.1f);
    }

    void RestartPosition()
    {
        transform.position = startPosition;
        playerRigidbody.velocity = Vector2.zero;
        GameObject camera = GameObject.Find("Main Camera");
        camera.GetComponent<CameraFollow>().ResetCameraPosition();
    }

    // Update is called once per frame
    void Update()
    {
        bool isTouching = IsTouchingTheGround();
        if (Input.GetButtonDown("Jump"))
        {
            if (isTouching)
            {
                Jump();
            }
        }

        Debug.DrawRay(transform.position, Vector2.down * 1.5f, Color.red);

        animator.SetBool(STATE_ON_THE_GROUND, isTouching);
    }

    void FixedUpdate()
    {
        if(GameManager.sharedInstance.currentGameState == GameState.inGame)
        {
            if(playerRigidbody.velocity.x < runningSpeed)
            {
                playerRigidbody.velocity = new Vector2(runningSpeed, playerRigidbody.velocity.y);
            }
        }
        else
        {
            playerRigidbody.velocity = new Vector2(0, playerRigidbody.velocity.y);
        }
    }

    void Jump()
    {
        if (GameManager.sharedInstance.currentGameState == GameState.inGame)
        {
            playerRigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    bool IsTouchingTheGround()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, 1.5f, groundMask);
    }

    public void Die()
    {
        animator.SetBool(STATE_ALIVE, false);
        GameManager.sharedInstance.GameOver();
    }

    public void CollectHealth(int points)
    {
        this.healthPoints = points;
        if (this.healthPoints >= MAX_HEALTH)
            healthPoints = MAX_HEALTH;
    }

    public void CollectMana(int points)
    {
        this.manaPoints = points;
        if (this.manaPoints >= MAX_MANA)
            manaPoints = MAX_MANA;
    }

    public int GetHealth()
    {
        return healthPoints;
    }

    public int GetMana()
    {
        return manaPoints;
    }
}
