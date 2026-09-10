using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleUIController : MonoBehaviour
{
    [SerializeField] private GameObject selectJobPanel;
    public void SelectWarrior()
    {
        GameSession.SelectedJob = JobType.Warrior;
        LoadStageScene();
    }

    public void SelectArcher()
    {
        GameSession.SelectedJob = JobType.Archer;
        LoadStageScene();
    }

    public void LoadStageScene()
    {
        SceneManager.LoadScene(SceneNames.Stage);
    }


    public void ShowJobSelectPanel()
    {
        selectJobPanel.SetActive(true);
    }

}
