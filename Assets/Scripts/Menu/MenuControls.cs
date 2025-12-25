using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
public class MenuControls : MonoBehaviour
{

    [SerializeField] private GameObject MainMenu;
    [SerializeField] private GameObject Settings;
    public void PlayPressed()
    {
        Debug.Log("Game Started");
        SceneManager.LoadScene("Game");
    }

    public void ExitPressed()
    {
        Debug.Log("Exit pressed!");
        Application.Quit();
    }

    public void ShowSettings()
    {
        Debug.Log("ShowSettings вызван");

        if (MainMenu == null) Debug.LogError("MainMenu не назначен!");
        if (Settings == null) Debug.LogError("Settings не назначен!");

        try
        {
            Settings.SetActive(true);
            Debug.Log("Settings включен");

            MainMenu.SetActive(false);
            Debug.Log("MainMenu отключен");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Ошибка: {e.Message}");
        }
    }

    public void ShowMenu()
    {
        Debug.Log("ShowSettings вызван");

        if (MainMenu == null) Debug.LogError("MainMenu не назначен!");
        if (Settings == null) Debug.LogError("Settings не назначен!");

        try
        {
            Settings.SetActive(false);
            Debug.Log("Settings отключен");

            MainMenu.SetActive(true);
            Debug.Log("MainMenu включен");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Ошибка: {e.Message}");
        }
    }

  




}
