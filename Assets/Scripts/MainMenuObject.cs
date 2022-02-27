using UnityEngine;
using System.Collections;
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
    [SerializeField] private UnityEngine.UI.Text playerCountText;
    private int playerCounter;
    [SerializeField] private Button defaultPlayGameScreenButton;
    [SerializeField] private Text selectMorePlayerText;

    // Start is called before the first frame update
    void Start()
    {
        if (FindObjectOfType<GameManager>() == null)
        {
            Instantiate(gameManager);
        }
        playerCounter = GameManager.instance.returnPlayerCounter();
        playerCountText.text = "Players " + playerCounter;
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
        SelectButton(GameObject.FindGameObjectWithTag("ControlButton"));
    }

    public void SelectButton(GameObject uiElement)
    {
        if (uiElement.GetComponent<UnityEngine.UI.Button>())
        {
            uiElement.GetComponent<UnityEngine.UI.Button>().Select();
        } else if (uiElement.GetComponent<TMPro.TMP_Dropdown>())
        {
            uiElement.GetComponent<TMPro.TMP_Dropdown>().Select();
        }
    }

    public void UpdatePlayerCount(int playerCount)
    {
        playerCounter = playerCount;
        playerCountText.text = "Players: " + playerCount;
    }

    public void OnPlay()
    {
        if (playerCounter > 0) {
            LoadScreen(1);
            defaultPlayGameScreenButton.Select();
        } else
        {
            StartCoroutine(SelectMorePlayers());
        }
    }

    IEnumerator SelectMorePlayers()
    {
        selectMorePlayerText.text = "Please Assign Controls";
        yield return new WaitForSeconds(5f);
        selectMorePlayerText.text = "";
    }
}
