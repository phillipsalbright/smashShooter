using UnityEngine;

/**
 * Singleton object made to hold spawnpoints and other relevant information specific to the current level.
 */
public class LevelInfoHolder : MonoBehaviour
{
    public static LevelInfoHolder instance;

    public MatchSettings matchSettings;

    public Transform[] spawnPoints;

    public Transform spectatorPoint;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else
        {
            Destroy(this);
        }
        GameManager.instance.StartMatch();
    }
}
