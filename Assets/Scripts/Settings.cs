using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public void SetDifficulty(int difficulty)
    {

        PlayerPrefs.SetInt("Difficulty", difficulty);
    }

    public void SetSensitivity(System.Single sensitivity)
    {
        PlayerPrefs.SetFloat("Sensitivity", sensitivity);
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
        GameObject.Find("Sensitivity").GetComponent<Slider>().value = PlayerPrefs.GetFloat("Sensitivity", .5f);
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
