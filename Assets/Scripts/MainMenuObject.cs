using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/**
 * This class is connected to a GameObject in the mainmenu scene and contains methods to be
 * called by attached buttons on the menu to load different scenes.
 * Set the menuScreens and use the LoadScreen and LoadScene methods for buttons on the menus by
 * setting them within the Unity Editor.
 */
public class MainMenuObject : MonoBehaviour
{
    public GameObject[] menuScreens;

    [SerializeField] private GameObject gameManager;

    // Start is called before the first frame update
    void Start()
    {
        if (FindObjectOfType<GameManager>() == null)
        {
            Instantiate(gameManager);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadScreen(int screenIndex)
    {
        menuScreens[screenIndex].SetActive(true);
        for (int i = 0; i < menuScreens.Length; i++)
        {
            if (menuScreens[i].activeInHierarchy == true && i != screenIndex)
            {
                menuScreens[i].SetActive(false);
            }
        }
    }

    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void SetupPlayerControls()
    {
        GameManager.instance.GetPlayerControls(GameManager.instance.matchSettings.numberOfPlayers);
    }
}
