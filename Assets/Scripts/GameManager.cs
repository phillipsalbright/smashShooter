using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public MatchSettings matchSettings;

    public Transform[] spawnPoints;

    public Transform spectatorPoint;

    public Transform startPoint;

    private int playerCounter = 1;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one GameManager in scene.");
        } else
        {
            instance = this;
        }
    }

    public int GetNextPlayerNumber()
    {
        playerCounter++;
        return playerCounter - 1;
    }
}
