using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneLoader : MonoBehaviour
{
    
#if UNITY_EDITOR

    [SerializeField] private UnityEditor.SceneAsset sceneAsset;
#endif
    [SerializeField] private string sceneName;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (sceneAsset != null)
            sceneName = sceneAsset.name;
    }
#endif

    public void LoadScene()
    {
        Debug.Log($"[SceneLoader] LoadScene 호출 — sceneName = {sceneName}");
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("[SceneLoader] sceneName이 비어 있습니다!");
            return;
        }
        SceneManager.LoadScene(sceneName);
    }


    public void QuitGame()
    {
#if UNITY_EDITOR    
        UnityEditor.EditorApplication.isPlaying = false;
#else              
        Application.Quit();
#endif
    }
}
