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

    private Transform startPoint;

    private PlayerInputManager playerInputManager;

    private int playerCounter = 1;

    [SerializeField] private Canvas canvas;
    [SerializeField] private TMP_Text text;
    [SerializeField] private Button cancelPlayerSelectButton;
    private LevelInfoHolder levelInfoHolder;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);  
        } else
        {
            instance = this;
        }
        DontDestroyOnLoad(this);
        playerInputManager = this.GetComponentInChildren<PlayerInputManager>();
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
        playerInputManager.DisableJoining();
        cancelPlayerSelectButton.gameObject.SetActive(false);
        canvas.gameObject.SetActive(false);
    }

    public void StartMatch()
    {
        knockedOutPlayers = 0;
        text.text = "";
        canvas.gameObject.SetActive(true);
        StartCoroutine(Countdown());
        levelInfoHolder = LevelInfoHolder.instance;
        spawnPoints = levelInfoHolder.spawnPoints;
        startPoint = levelInfoHolder.startPoint;
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

    public Transform GetInitialSpawnPoint()
    {
        return startPoint;
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
        for (int i = 0; i < knockedOutPlayers; i++)
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
        SceneManager.LoadScene(0);
        canvas.gameObject.SetActive(false);
    }
}
