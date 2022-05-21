using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
[System.Serializable]

public enum Turn
{
    Player,
    CPU,
};

public enum Direction
{
    Left,
    Right
};

public enum GameState
{
    TitleScreen,
    Running,
    Paused,
    GameOver
};

public class Config
{
    public static int StartingHealth = 25;
    public static int StartingMana = 3;
    public static int ManaRegen = 3;
    public static int DrawCardManaCost = 3;
    public static int DeckSize = 30;
    public static int HandSize = 5;
    public static int StartingHandSize = 2;
    public static bool Debug = true;
}

public class Game : MonoBehaviour
{

    public Player player;
    public CPU cpu;
    public Board board;

    public GameObject playerObject;
    public GameObject cpuObject;
    public GameObject boardObject;

    public GameObject GameUI;
    public GameObject PausedUI;
    public GameObject TitleUI;
    public GameObject GameOverUI;
    public GameObject ControlsUI;
    public GameObject PreviousUI;
    public GameObject MasterHandImage;
    public TextMeshProUGUI GameOverText;

    private Turn turn;
    private GameState gameState = GameState.TitleScreen;
    private IEnumerator runRoutine;


    void Awake()
    {
        CardDatabase.init();
    }

    void Start()
    {
        this.init();
    }


    void init()
    {
        TitleUI.SetActive(true);
    }

    void newGame()
    {
        if (runRoutine != null)
        {
            StopCoroutine(runRoutine);
        }

        TitleUI.SetActive(false);
        PausedUI.SetActive(false);
        GameOverUI.SetActive(false);
        GameUI.SetActive(true);
        boardObject.SetActive(true);
        playerObject.SetActive(true);
        cpuObject.SetActive(true);

        player.init();
        cpu.init();
        board.init();

        turn = Turn.Player;
        gameState = GameState.Running;
        player.beginTurn(board);

        runRoutine = this.run();
        StartCoroutine(runRoutine);
    }

    IEnumerator run()
    {
        while (gameState == GameState.Running)
        {
            if (this.turn == Turn.Player)
            {
                Debug.Log("Waiting for player to end turn.");
                yield return new WaitForSeconds(1.5f);
            }
            else if (this.turn == Turn.CPU)
            {
                cpu.beginTurn(board);
                yield return new WaitForSeconds(1.5f);
                board.runGameLogic(cpu, player);
                yield return new WaitForSeconds(.5f);
                this.runGameLogic();
                if (gameState == GameState.Running)
                {
                    player.beginTurn(board);
                }
            }
        }
    }

    void runGameLogic()
    {
        if (player.health <= 0 || cpu.health <= 0)
        {
            gameState = GameState.GameOver;
            this.updateGameOverUI();
            this.showGameOverUI();
            StopCoroutine(runRoutine);
            Debug.Log("--- Game Over ---");
        }
        else
        {
            player.mana = Math.Min(10, player.mana + 3);
            cpu.mana = Math.Min(10, cpu.mana + 3);
            this.updateUI();
            this.changeTurn();
        }
    }

    void updateUI()
    {
        player.updateUI();
        cpu.updateUI();
    }

    void updateGameOverUI()
    {
        if (cpu.health <= 0)
        {
            GameOverText.text = "YOU WIN";
            MasterHandImage.SetActive(false);
        }
        else if (player.health <= 0)
        {
            GameOverText.text = "MASTER HAND WINS";
            MasterHandImage.SetActive(true);
        }
    }

    public void exitGameButton()
    {
        Application.Quit();
    }

    public void newGameButton()
    {
        this.newGame();
    }

    public void controlsButton()
    {
        this.showControlsUI();
    }

    public void backButton()
    {
        this.hideControlsUI();
    }

    public void pause()
    {
        this.showPausedUI();
        gameState = GameState.Paused;
        StopCoroutine(runRoutine);
    }

    public void resume()
    {
        this.hidePausedUI();
        gameState = GameState.Running;
        StartCoroutine(runRoutine);
    }

    void showPausedUI()
    {
        GameUI.SetActive(false);
        PausedUI.SetActive(true);
    }

    void hidePausedUI()
    {
        PausedUI.SetActive(false);
        GameUI.SetActive(true);
    }

    void showGameOverUI()
    {
        GameUI.SetActive(false);
        GameOverUI.SetActive(true);
    }

    void hideGameOverUI()
    {
        MasterHandImage.SetActive(false);
        GameOverUI.SetActive(false);
    }

    void showControlsUI()
    {
        if (PausedUI.activeSelf)
        {
            PreviousUI = PausedUI;
        }
        else if (TitleUI.activeSelf)
        {
            PreviousUI = TitleUI;
        }
        PreviousUI.SetActive(false);
        ControlsUI.SetActive(true);
    }

    void hideControlsUI()
    {
        ControlsUI.SetActive(false);
        PreviousUI.SetActive(true);
    }

    void changeTurn()
    {
        if (turn == Turn.Player)
        {
            turn = Turn.CPU;
        }
        else if (turn == Turn.CPU)
        {
            turn = Turn.Player;
        }
    }

    void Update()
    {
        if (gameState == GameState.TitleScreen || gameState == GameState.GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.N))
            {
                this.newGame();
            }
        }
        else if (gameState == GameState.Paused)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                this.resume();
            }
        }
        else if (gameState == GameState.Running)
        {
            this.updateUI();
            if (this.turn == Turn.Player)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    this.pause();
                }
                else if (Config.Debug && Input.GetKeyDown(KeyCode.P))
                {
                    player.mana += 10;
                    this.updateUI();
                }
                else if (Config.Debug && Input.GetKeyDown(KeyCode.O))
                {
                    player.health += 10;
                    this.updateUI();
                }
                else if (Config.Debug && Input.GetKeyDown(KeyCode.I))
                {
                    cpu.health -= 10;
                    this.updateUI();
                }
                else if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.Backslash))
                {
                    player.drawCard();
                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
                {
                    if (player.hand.displayingHand)
                    {
                        if (player.hand.displayingSelected)
                        {
                            player.moveSelected(Direction.Left, board);
                        }
                        else
                        {
                            player.hoverCard(Direction.Left);
                        }
                    }
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
                {
                    if (player.hand.displayingHand)
                    {
                        if (player.hand.displayingSelected)
                        {
                            player.moveSelected(Direction.Right, board);
                        }
                        else
                        {
                            player.hoverCard(Direction.Right);
                        }
                    }
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
                {
                    if (player.hand.displayingHand)
                    {
                        if (player.hand.displayingSelected)
                        {
                            player.playCard(board);
                        }
                        else
                        {
                            player.selectCard(board);
                        }
                    }
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
                {
                    if (player.hand.displayingHand)
                    {
                        player.deselectCard();
                    }
                }
                else if (Input.GetKeyDown(KeyCode.Q))
                {
                    player.toggleDisplayHand();
                }
                else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.E))
                {
                    player.endTurn();
                    this.changeTurn();
                }
            }
        }
    }
}
