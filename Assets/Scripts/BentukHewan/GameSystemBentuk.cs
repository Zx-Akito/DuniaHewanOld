using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class DataGameBentuk
{
    public static int dataLevel;
    public static int dataTime;
    public static int dataScore;
    public static int dataHealth;
}

public class GameSystemBentuk : MonoBehaviour
{
    //===========================================//
    //========== START PROPERTY GAME ============//
    //===========================================//
    public static GameSystemBentuk instance;
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

    // Game Data
    [System.Serializable]
    public class DataBentuk
    {
        public Sprite imageDrag;
        public Sprite imageDrop;
        public AudioClip animalVoice;
    }

    [Header("Standart Settings")]
    public DataBentuk[] dataGame;
    [Space]
    [Space]
    [Space]
    public DragBentuk[] dragObjs;
    public DropBentuk[] dropObjs;
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
        target = dropObjs.Length;
        ResetData();
        RandQuestion();
    }

    void Update()
    {
        if (gameActive && !gameEnd)
        {
            // Countdown time
            if (DataGameBentuk.dataTime > 0)
            {
                s += Time.deltaTime;
                if (s >= 1)
                {
                    DataGameBentuk.dataTime--;
                    s = 0;
                }
            }

            // When the opportunity is up
            if (DataGameBentuk.dataHealth <= 0)
            {
                gameActive = false;
                gameEnd = true;

                // Game Lose
                SceneManager.LoadScene("ResultBentuk");
                AudioManager.instance.WinLose(11);
            }

            // When time is up
            if (DataGameBentuk.dataTime <= 0)
            {
                gameActive = false;
                gameEnd = true;

                // Game Lose
                SceneManager.LoadScene("ResultBentuk");
                AudioManager.instance.WinLose(6);
            }

            // When the target has been reached
            if (currentData >= target)
            {
                gameActive = false;
                gameEnd = true;

                // Game Win
                if (DataGameBentuk.dataLevel < (maxLevel - 1))
                {
                    DataGameBentuk.dataLevel++;

                    // Move to next level
                    SceneManager.LoadScene("GameBentuk" + DataGameBentuk.dataLevel);
                    AudioManager.instance.SetSfx(9);
                }

                else
                {
                    // End Game
                    // canvasTransition.GetComponent<UiControl>().BtnScene("ResultBentuk");
                    // AudioManager.instance.WinLose(6);

                    DataGameBentuk.dataLevel++;
                    SceneManager.LoadScene("GameBentuk9");
                    AudioManager.instance.SetSfx(9);
                }

            }
        }

        SetInfoUI();
    }

    /// <summary>
    /// Function for random question
    /// </summary>
    public void RandQuestion()
    {
        randQuestion.Clear();
        randPos.Clear();

        // Random Question
        randQuestion = new List<int>(new int[dragObjs.Length]);
        for (int i = 0; i < randQuestion.Count; i++)
        {
            rand = Random.Range(1, dataGame.Length);
            while (randQuestion.Contains(rand))
            {
                rand = Random.Range(1, dataGame.Length);
            }

            randQuestion[i] = rand;
            dragObjs[i].key = rand - 1;

            // Applies the image data to the class variable imageDrag
            dragObjs[i].images.sprite = dataGame[rand - 1].imageDrag;
            // Applies the clip data to the class variable animalVoice
            dragObjs[i].animVoice = dataGame[rand - 1].animalVoice;
        }

        randPos = new List<int>(new int[dropObjs.Length]);
        for (int i = 0; i < randPos.Count; i++)
        {
            rand2 = Random.Range(1, randQuestion.Count + 1);
            while (randPos.Contains(rand2))
            {
                rand2 = Random.Range(1, randQuestion.Count + 1);
            }

            randPos[i] = rand2;
            dropObjs[i].drop.key = randQuestion[rand2 - 1] - 1;

            // Applies the image data to the class variable imageDrop
            dropObjs[i].image.sprite = dataGame[dropObjs[i].drop.key].imageDrop;
        }
    }

    /// <summary>
    /// Function set info data to game ui
    /// </summary>
    public void SetInfoUI()
    {
        //Level Info
        textLevel.text = (DataGameBentuk.dataLevel + 1).ToString();

        //Time Info
        int minute = Mathf.FloorToInt(DataGameBentuk.dataTime / 60);
        int second = Mathf.FloorToInt(DataGameBentuk.dataTime % 60);
        textTime.text = minute.ToString("00") + ":" + second.ToString("00");

        //Score Info
        textScore.text = DataGameBentuk.dataScore.ToString();

        //Health Info
        uiHealth.sizeDelta = new Vector2(47f * DataGameBentuk.dataHealth , 40f);
    }

    /// <summary>
    /// Function to reset data to default
    /// </summary>
    public static void ResetData()
    {
        if (SceneManager.GetActiveScene().name == "GameBentuk0")
        {
            newGame = false;
            DataGameBentuk.dataLevel = 0;
            DataGameBentuk.dataTime = 60 * 2;
            DataGameBentuk.dataScore = 0;
            DataGameBentuk.dataHealth = 5;
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
