using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    private int SensitivityNumber;

    public void SetPlayerNumber(int numberOfPlayers)
    {
        GameManager.instance.matchSettings.numberOfPlayers = numberOfPlayers + 1;
        PlayerPrefs.SetInt("NumberOfPlayers", numberOfPlayers + 1);
    }

    public void SetLives(int lives)
    {
        GameManager.instance.matchSettings.startingLives = lives + 1;
        PlayerPrefs.SetInt("NumberOfLives", lives + 1);
    }

    public void SetSensitivity(System.Single sensitivity)
    {
        PlayerPrefs.SetFloat("Sensitivity" + SensitivityNumber, sensitivity);
    }

    public void SetSensitivityPlayer(int playerNumber)
    {
        SensitivityNumber = playerNumber + 1;
        GameObject.Find("Sensitivity").GetComponent<Slider>().value = PlayerPrefs.GetFloat("Sensitivity" + SensitivityNumber, .5f);
        PlayerPrefs.SetInt("PlayerSensitivityBarNumber", SensitivityNumber);
    }

    public void SavePrefs()
    {
        PlayerPrefs.Save();
    }

    /**
     * Set the options UI representations to their current value.
     */
    void Awake()
    {
        SensitivityNumber = PlayerPrefs.GetInt("PlayerSensitivityBarNumber", 1);
        GameObject.Find("PlayerSensitivityNumber").GetComponent<TMPro.TMP_Dropdown>().value = SensitivityNumber - 1;
        GameObject.Find("Sensitivity").GetComponent<Slider>().value = PlayerPrefs.GetFloat("Sensitivity" + SensitivityNumber, .5f);
        GameObject.Find("NumberOfPlayers").GetComponent<TMPro.TMP_Dropdown>().value = PlayerPrefs.GetInt("NumberOfPlayers", 2) - 1;
        GameObject.Find("Number of Lives").GetComponent<TMPro.TMP_Dropdown>().value = PlayerPrefs.GetInt("NumberOfLives", 3) - 1;
    }
    /** Example code from a video for reference:
    // Start is called before the first frame update
    void Start()
    {
        LoadPrefs();
    }

    void OnApplicationQuit()
    {
        SavePrefs();
    }

    
    public void SavePrefs()
    {
        // PlayerPrefs.Set<Int/Float/String>("name", value);
        // PlayerPrefs.Save();
    }

    public void LoadPrefs()
    {
        // variable name = PlayerPrefs.Get<Int/Float/String>(name, valueToReturnifname is not found);
        // ** Do whatever we need with objects and stuff ** //
    }
    */
}
