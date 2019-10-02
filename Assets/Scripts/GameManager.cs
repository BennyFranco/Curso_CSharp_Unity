using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    menu,
    inGame,
    gameOver
}

public class GameManager : MonoBehaviour
{
    public GameState currentGameState = GameState.menu;

    public int collectedObject = 0;

    public static GameManager sharedInstance;

    private PlayerController controller;

    void Awake()
    {
        if(sharedInstance == null)
        {
            sharedInstance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("Player").GetComponent<PlayerController>();
        SetGameState(GameState.menu);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Submit") && currentGameState != GameState.inGame)
        {
            StartGame();
        }
    }

    public void StartGame()
    {
        SetGameState(GameState.inGame);
    }

    public void GameOver()
    {
        SetGameState(GameState.gameOver);
    }

    public void BackToMenu()
    {
        SetGameState(GameState.menu);
    }

    private void SetGameState(GameState newGameState)
    {
        if(newGameState == GameState.menu)
        {
            MenuManager.sharedInstance.ShowMainMenu();
            MenuManager.sharedInstance.HideGameMenu();
            MenuManager.sharedInstance.HideGameOverMenu();
        }
        else if(newGameState == GameState.inGame)
        {
            LevelManager.sharedInstance.RemoveAllLevelBlocks();
            LevelManager.sharedInstance.GenerateInitialBlocks();
            MenuManager.sharedInstance.HideMainMenu();
            MenuManager.sharedInstance.ShowGameMenu();
            MenuManager.sharedInstance.HideGameOverMenu();
            controller.StartGame();
        }
        else if (newGameState == GameState.gameOver)
        {
            // TODO: Game over logic here.
            MenuManager.sharedInstance.HideMainMenu();
            MenuManager.sharedInstance.HideGameMenu();
            MenuManager.sharedInstance.ShowGameOverMenu();
        }

        currentGameState = newGameState;
    }

    public void CollectObject(Collectable collectable)
    {
        collectedObject += collectable.value;
    }
}
