using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class UiControl : MonoBehaviour
{
    string saveSceneName;
    [HideInInspector][SerializeField] GameObject pauseUI;

    public void Start()
    {
        Application.targetFrameRate = Screen.currentResolution.refreshRate;
    }

    //================================================//
    // LOAD SCENE GAME
    //================================================//

    public void MencocokanNama()
    {
        SceneManager.LoadScene("Game0");
        AudioManager.instance.SetBgm(1);
    }

    public void MencocokanBentuk()
    {
        SceneManager.LoadScene("GameBentuk0");
        AudioManager.instance.SetBgm(12);
    }

    public void BtnScene(string name)
    {
        this.gameObject.SetActive(true);
        saveSceneName = name;
        GetComponent<Animator>().Play("TransitionOut");
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene(saveSceneName);
    }

    //================================================//
    // BUTTON FUNCTION
    //================================================//

    public void BtnSound(int id)
    {
        AudioManager.instance.SetSfx(id);
    }
    
    public void BtnVoice(int id)
    {
        AudioManager.instance.SetVoice(id);
    }
    public void BtnVoiceEng(int id)
    {
        AudioManager.instance.SetVoiceEng(id);
    }

    public void BtnAnimalSound(int id)
    {
        AudioManager.instance.SetAnimalSound(id);
    }

    public void BtnMenu(string name)
    {
        this.gameObject.SetActive(true);
        saveSceneName = name;
        GetComponent<Animator>().Play("TransitionOut");
        AudioManager.instance.SetBgm(0);
        AudioManager.instance.audioBgm.loop = true;
    }

    public void Exit()
    {
        Application.Quit();
        Debug.Log("Permainan Telah Berhenti");
    }

    //================================================//
    // REDIRECT TO MAIL ( SUPPORT REPORT BUGS )
    //================================================//

    public void SendEmail()
    {
        string email = "rifkinurmansyah@icloud.com";
        string subject = MyEscapeURL("Dunia Hewan - Support & Feedback");
        string body = MyEscapeURL("");
        Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
    }

    string MyEscapeURL (string url)
    {
        return UnityWebRequest.EscapeURL(url).Replace("+","%20");
    }
}
