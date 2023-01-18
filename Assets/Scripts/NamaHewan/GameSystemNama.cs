using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class DataGameNama
{
    public static int dataLevel;
    public static int dataTime;
    public static int dataScore;
    public static int dataHealth;
}

public class GameSystemNama : MonoBehaviour
{
    //===========================================//
    //========== START PROPERTY GAME ============//
    //===========================================//
    public int lang;
    public static GameSystemNama instance;
    public static bool newGame = true;
    int maxLevel = 9;

    [Header("Component UI")]
    public TextMeshProUGUI textLevel;
    public TextMeshProUGUI textTime;
    public TextMeshProUGUI textScore;
    public RectTransform uiHealth;

    [Space]

    [Header("Component GUI")]
    public GameObject canvasPause;
    public GameObject canvasTransition;

    [Space]

    [Header("System Data")]
    public bool gameActive;
    public bool gameEnd;
    public int target;
    public static int currentData;

    [Space]

    // Random Property
    public bool randSystem;
    List<int> randQuestion = new List<int>();
    List<int> randPos = new List<int>();
    int rand;
    int rand2;
    float s;

    [System.Serializable]
    public class DataNama
    {
        public string name;
        public Sprite image;
    }

    [System.Serializable]
    public class DataNamaEng
    {
        public string name;
        public Sprite image;
    }

    [Header("Standart Settings")]
    public DataNama[] dataGameInd;
    public DataNamaEng[] dataGameEng;
    [Space]
    [Space]
    [Space]
    public DragNama[] dragObj;
    public DropNama[] dropObj;
    //===========================================//
    //=========== END PROPERTY GAME =============//
    //===========================================//

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Application.targetFrameRate = 60;
        gameActive = true;
        gameEnd = false;
        currentData = 0;
        target = dropObj.Length;
        ResetData();

        if (PlayerPrefs.GetInt("Language", lang) == 1)
        {
            RandQuestionEng();
        }
        else if (PlayerPrefs.GetInt("Language", lang) == 0)
        {
            RandQuestionInd();
        }
    }

    void Update()
    {
        if (gameActive && !gameEnd)
        {
            // Countdown time
            if (DataGameNama.dataTime > 0)
            {
                s += Time.deltaTime;
                if (s >= 1)
                {
                    DataGameNama.dataTime--;
                    s = 0;
                }
            }

            // When the opportunity is up
            if (DataGameNama.dataHealth <= 0)
            {
                gameActive = false;
                gameEnd = true;

                // Game Lose
                canvasTransition.GetComponent<UiControl>().BtnScene("ResultNama");
                AudioManager.instance.WinLose(11);
            }

            // When time is up
            if (DataGameNama.dataTime <= 0)
            {
                gameActive = false;
                gameEnd = true;

                // Game Lose
                canvasTransition.GetComponent<UiControl>().BtnScene("ResultNama");
                AudioManager.instance.WinLose(6);
            }

            // When the target has been reached
            if (currentData >= target)
            {
                gameActive = false;
                gameEnd = true;

                // Game Win
                if (DataGameNama.dataLevel < (maxLevel - 1))
                {
                    DataGameNama.dataLevel++;

                    // Move to next level
                    SceneManager.LoadScene("Game" + DataGameNama.dataLevel);
                    AudioManager.instance.SetSfx(9);
                }

                else
                {
                    // End Game
                    // canvasTransition.GetComponent<UiControl>().BtnScene("ResultNama");
                    // AudioManager.instance.WinLose(6);
                    
                    // Infinity Loop in Stage 12
                    DataGameNama.dataLevel++;
                    SceneManager.LoadScene("Game9");
                    AudioManager.instance.SetSfx(9);
                }

            }
        }

        SetInfoUI();
    }

    /// <summary>
    /// Function for random question IND Lang
    /// </summary>
    public void RandQuestionInd()
    {
        Debug.Log("Indonesia");
        randQuestion.Clear();
        randPos.Clear();

        randQuestion = new List<int>(new int[dragObj.Length]);
        for (int i = 0; i < randQuestion.Count; i++)
        {
            rand = Random.Range(1, dataGameInd.Length);
            while (randQuestion.Contains(rand))
            {
                rand = Random.Range(1, dataGameInd.Length);
            }

            randQuestion[i] = rand;
            dragObj[i].key = rand - 1;

            // Applies the text data to the class variable name
            dragObj[i].text.text = dataGameInd[rand - 1].name;
        }

        // Random Position
        randPos = new List<int>(new int[dropObj.Length]);
        for (int i = 0; i < randPos.Count; i++)
        {
            rand2 = Random.Range(1, randQuestion.Count + 1);
            while (randPos.Contains(rand2))
            {
                rand2 = Random.Range(1, randQuestion.Count + 1);
            }

            randPos[i] = rand2;
            dropObj[i].drop.key = randQuestion[rand2 - 1] - 1;

            // Applies the image data to the class variable image
            dropObj[i].image.sprite = dataGameInd[dropObj[i].drop.key].image;
        }
    }

    /// <summary>
    /// Function for random question ENG Lang
    /// </summary>
    public void RandQuestionEng()
    {
        Debug.Log("English");
        randQuestion.Clear();
        randPos.Clear();

        randQuestion = new List<int>(new int[dragObj.Length]);
        for (int i = 0; i < randQuestion.Count; i++)
        {
            rand = Random.Range(1, dataGameEng.Length);
            while (randQuestion.Contains(rand))
            {
                rand = Random.Range(1, dataGameEng.Length);
            }

            randQuestion[i] = rand;
            dragObj[i].key = rand - 1;

            // Applies the text data to the class variable name
            dragObj[i].text.text = dataGameEng[rand - 1].name;
        }

        // Random Position
        randPos = new List<int>(new int[dropObj.Length]);
        for (int i = 0; i < randPos.Count; i++)
        {
            rand2 = Random.Range(1, randQuestion.Count + 1);
            while (randPos.Contains(rand2))
            {
                rand2 = Random.Range(1, randQuestion.Count + 1);
            }

            randPos[i] = rand2;
            dropObj[i].drop.key = randQuestion[rand2 - 1] - 1;

            // Applies the image data to the class variable image
            dropObj[i].image.sprite = dataGameEng[dropObj[i].drop.key].image;
        }
    }

    /// <summary>
    /// Function set info data to game ui
    /// </summary>
    public void SetInfoUI()
    {
        //Level Info
        textLevel.text = (DataGameNama.dataLevel + 1).ToString();

        //Time Info
        int minute = Mathf.FloorToInt(DataGameNama.dataTime / 60);
        int second = Mathf.FloorToInt(DataGameNama.dataTime % 60);
        textTime.text = minute.ToString("00") + ":" + second.ToString("00");

        //Score Info
        textScore.text = DataGameNama.dataScore.ToString();

        //Health Info
        uiHealth.sizeDelta = new Vector2(47f * DataGameNama.dataHealth , 40f);
    }

    /// <summary>
    /// Function to reset data to default
    /// </summary>
    public static void ResetData()
    {
        if (SceneManager.GetActiveScene().name == "Game0")
        {
            newGame = false;
            DataGameNama.dataLevel = 0;
            DataGameNama.dataTime = 60 * 2;
            DataGameNama.dataScore = 0;
            DataGameNama.dataHealth = 5;
        }
    }

    /// <summary>
    /// Function if the pause button is clicked
    /// </summary>
    public void BtnPause(bool pause)
    {
        if (pause)
        {
            gameActive = false;
            canvasPause.SetActive(true);
            canvasPause.GetComponentInParent<Animator>().Play("StartPause");
            AudioManager.instance.SetSfx(7);
        }
        else
        {
            gameActive = true;
            canvasPause.GetComponentInParent<Animator>().Play("EndPause");
            AudioManager.instance.SetSfx(8);
        }
    }
}
