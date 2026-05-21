using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneReloader : MonoBehaviour
{
    [SerializeField] private GameObject _winPanel;
    [SerializeField] private GameObject _losePanel;
    [SerializeField] private BossController _bossController;
    [SerializeField] private PlayerControllerFacade _playerControllerFacade;

    private void OnEnable()
    {
        _bossController.OnDefeated += ActiveWinPanel;
        _playerControllerFacade.OnDefeated += ActiveLosePanel;
    }

    private void OnDisable()
    {
        _bossController.OnDefeated -= ActiveWinPanel;
        _playerControllerFacade.OnDefeated -= ActiveLosePanel;
    }

    private void ActiveWinPanel()
    {
        _winPanel.SetActive(true);
    }

    private void ActiveLosePanel()
    {
        _losePanel.SetActive(true);
    }

    public void ReloadAsync()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Single);
    }
}