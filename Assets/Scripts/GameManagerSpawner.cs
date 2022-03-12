using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject gameManager;
    // Start is called before the first frame update
    void Awake()
    {
        if (FindObjectOfType<GameManager>() == null)
        {
            Instantiate(gameManager);
        }
    }
}
