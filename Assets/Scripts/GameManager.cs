using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public MatchSettings matchSettings;

    private Transform[] spawnPoints;

    public GameObject[] players = new GameObject[4];

    private int knockedOutPlayers;

    private Transform spectatorPoint;

    private PlayerInputManager playerInputManager;
    [SerializeField] private GameObject pauseMenu;

    private int playerCounter = 1;

    private bool inGame;
    private bool paused;

    [SerializeField] private Canvas canvas;
    [SerializeField] private TMP_Text text;
    [SerializeField] private Button cancelPlayerSelectButton;
    private LevelInfoHolder levelInfoHolder;

    private void Awake()
    {
        paused = false;
        inGame = false;
        if (instance != null)
        {
            Destroy(this);  
        } else
        {
            instance = this;
        }
        DontDestroyOnLoad(this);
        playerInputManager = this.GetComponentInChildren<PlayerInputManager>();
        matchSettings.numberOfPlayers = PlayerPrefs.GetInt("NumberOfPlayers", 2);
        matchSettings.startingLives = PlayerPrefs.GetInt("NumberOfLives", 3);
    }

    public int PlayerHasJoined(GameObject player)
    {
        DontDestroyOnLoad(player);
        players[playerCounter] = player;
        playerCounter++;
        if (playerCounter < matchSettings.numberOfPlayers)
        {
            text.text = "Click button on player " + (playerCounter + 1) + "'s device";
        } else
        {
            CancelPlayerControlAssignment();
        }
        cancelPlayerSelectButton.GetComponentInChildren<Text>().text = "Continue with " + (playerCounter) + " players";
        return playerCounter;
    }

    public void GetPlayerControls(int numberOfPlayers)
    {
        cancelPlayerSelectButton.gameObject.SetActive(true);
        cancelPlayerSelectButton.GetComponentInChildren<Text>().text = "cancel";
        matchSettings.numberOfPlayers = numberOfPlayers;
        for (int i = 0; i < playerCounter; i++) 
        {
            Destroy(players[i]);
        }
        playerCounter = 0;
        players = new GameObject[4];
        text.text = "Click button on player 1's device";
        canvas.gameObject.SetActive(true);
        playerInputManager.EnableJoining();
    }

    public void CancelPlayerControlAssignment()
    {
        matchSettings.numberOfPlayers = playerCounter;
        playerInputManager.DisableJoining();
        cancelPlayerSelectButton.gameObject.SetActive(false);
        canvas.gameObject.SetActive(false);
    }

    public void StartMatch()
    {
        inGame = true;
        knockedOutPlayers = 0;
        text.text = "";
        canvas.gameObject.SetActive(true);
        StartCoroutine(Countdown());
        levelInfoHolder = LevelInfoHolder.instance;
        spawnPoints = levelInfoHolder.spawnPoints;
        spectatorPoint = levelInfoHolder.spectatorPoint;
        for (int i = 0; i < matchSettings.numberOfPlayers; i++)
        {
            players[i].GetComponentInChildren<PlayerHealth>().Setup();
        }
    }

    IEnumerator Countdown()
    {
        text.text = "3";
        yield return new WaitForSeconds(1f);
        text.text = "2";
        yield return new WaitForSeconds(1f);
        text.text = "1";
        yield return new WaitForSeconds(1f);
        text.text = "game start";
        yield return new WaitForSeconds(.5f);
        canvas.gameObject.SetActive(false);
    }

    public Transform GetSpawnPoint()
    {
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);
        return spawnPoints[spawnPointIndex];
    }

    public Transform GetInitialSpawnPoint(int playerNum)
    {
        return spawnPoints[playerNum - 1];
    }

    public Transform GetSpectatorPoint()
    {
        return spectatorPoint;
    }

    public void PlayerDied()
    {
        knockedOutPlayers++;
        if (knockedOutPlayers == matchSettings.numberOfPlayers - 1)
        {
            StartCoroutine(GameOver());
        }
    }

    IEnumerator GameOver()
    {
        int winningPlayer = 0;
        for (int i = 0; i <= knockedOutPlayers; i++)
        {
            if (players[i].GetComponentInChildren<PlayerHealth>().livesLeft != 0)
            {
                winningPlayer = i + 1;
            }
        }
        yield return new WaitForSeconds(1f);
        players[winningPlayer - 1].GetComponentInChildren<PlayerInput>().DeactivateInput();
        yield return new WaitForSeconds(1f);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        //enable playerinput for controlling cursor with a controller?
        text.text = "Player " + winningPlayer + " won";
        //maybe make an end game menu here
        canvas.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        for (int i = 0; i < knockedOutPlayers; i++)
        {
            players[i].GetComponentInChildren<PlayerHealth>();
        }
        inGame = false;
        SceneManager.LoadScene(0);
        canvas.gameObject.SetActive(false);
    }

    /**
    public void PauseGame()
    {
        if (!inGame)
        {
            for (int i = 0; i < playerCounter; i++)
            {
                players[i].GetComponentInChildren<PlayerInput>().DeactivateInput();
            }
            Cursor.lockState = CursorLockMode.None;
            pauseMenu.SetActive(true);
        }
    }
    */

    public void PauseGame()
    {
        if (inGame)
        {
            if (!paused)
            {
                for (int i = 0; i < playerCounter; i++)
                {
                    players[i].GetComponentInChildren<PlayerInput>().DeactivateInput();
                }
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Time.timeScale = 0;
                pauseMenu.SetActive(true);
                paused = true;
            }
            else
            {
                ResumeGame();
            }
        }
    }

    public void ResumeGame()
    {
        Debug.Log("frogs");
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        for (int i = 0; i < playerCounter; i++)
        {
            players[i].GetComponentInChildren<PlayerInput>().ActivateInput();
        }
        paused = false;
    }

    public void QuitGame()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 1;

        pauseMenu.SetActive(false);
        SceneManager.LoadScene(0);
        canvas.gameObject.SetActive(false);
    }
}
