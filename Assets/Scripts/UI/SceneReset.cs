using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneReset : MonoBehaviour
{
    [SerializeField] private Button sceneResetButton;
    private int sceneIndex = 0;

    private void Start()
    {
        sceneResetButton.onClick.AddListener(resetScene);
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    private void resetScene()
    {
        SceneManager.LoadScene(sceneIndex);
    }
}