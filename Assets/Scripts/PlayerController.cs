﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 6f;
    public float runningSpeed = 2f;

    public float jumpRaycastDistance = 1.5f;

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

    public const int SUPERJUMP_COST = 5;
    public const float SUPERJUMP_FORCE = 1.5f;

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
        if (Input.GetButtonDown("Jump"))
        {
             Jump(false);
        }
        if (Input.GetButtonDown("SuperJump"))
        {
            Jump(true);
        }

        Debug.DrawRay(transform.position, Vector2.down * jumpRaycastDistance, Color.red);

        animator.SetBool(STATE_ON_THE_GROUND, IsTouchingTheGround());
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

    void Jump(bool superJump)
    {
        float jumpForceFactor = jumpForce;
        if(superJump && manaPoints >= SUPERJUMP_COST && IsTouchingTheGround())
        {
            manaPoints -= SUPERJUMP_COST;
            jumpForceFactor *= SUPERJUMP_FORCE;
        }

        if (GameManager.sharedInstance.currentGameState == GameState.inGame)
        {
            if (IsTouchingTheGround())
            {
                playerRigidbody.AddForce(Vector2.up * jumpForceFactor, ForceMode2D.Impulse);
                GetComponent<AudioSource>().Play();
            }
        }
    }

    bool IsTouchingTheGround()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, jumpRaycastDistance, groundMask);
    }

    public void Die()
    {
        float travelledDistance = GetTravelledDistance();
        float previousMaxDistance = PlayerPrefs.GetFloat("maxscore", 0f);

        if(travelledDistance > previousMaxDistance)
        {
            PlayerPrefs.SetFloat("maxscore", travelledDistance);
        }

        animator.SetBool(STATE_ALIVE, false);
        GameManager.sharedInstance.GameOver();
    }

    public void CollectHealth(int points)
    {
        this.healthPoints += points;
        if (this.healthPoints >= MAX_HEALTH)
            healthPoints = MAX_HEALTH;

        if(healthPoints <=0 )
        {
            Die();
        }
    }

    public void CollectMana(int points)
    {
        this.manaPoints += points;
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

    public float GetTravelledDistance()
    {
        return transform.position.x - startPosition.x;
    }
}
