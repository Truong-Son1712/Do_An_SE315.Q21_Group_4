using UnityEngine;
using UnityEngine.SceneManagement;

public class AuthenticationUIManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject loginPanel;
    public GameObject signupPanel;

    [Header("Scene Settings")]
    public string lobbySceneName = "Lobby";

    private void Start()
    {
        ShowLoginPanel();
    }

    public void ShowLoginPanel()
    {
        if (loginPanel != null)
            loginPanel.SetActive(true);
        if (signupPanel != null)
            signupPanel.SetActive(false);
    }

    public void ShowSignupPanel()
    {
        if (loginPanel != null)
            loginPanel.SetActive(false);
        if (signupPanel != null)
            signupPanel.SetActive(true);
    }

    public void OnLoginButtonClicked()
    {
        if (string.IsNullOrEmpty(lobbySceneName))
        {
            Debug.LogWarning("Lobby scene name is empty. Please set lobbySceneName in the inspector.");
            return;
        }

        SceneManager.LoadScene(lobbySceneName);
    }
}
