using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleUIController : MonoBehaviour
{

    public void LoadStageScene()
    {
        SceneManager.LoadScene(SceneNames.Stage);
    }
   
}
