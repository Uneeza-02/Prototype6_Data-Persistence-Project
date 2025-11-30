#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuUIHandler : MonoBehaviour
{
    public TMP_InputField NameInputField;

    private void Start()
    {
        NameInputField.text = NameManager.Instance.PlayerName;
    }

    public void OnValueChanged()
    {
        NameManager.Instance.PlayerName = NameInputField.text;
    }

    public void StartNew()
    {
        NameManager.Instance.SaveName();
        NameManager.Instance.LoadName();
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
        NameManager.Instance.SaveName();

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
