using UnityEngine;
using UnityEngine.UI;
using LootLocker.Requests;
using TMPro;

public class GameResultNama : MonoBehaviour
{

    public GameObject noInternet;
    public GameObject loading;
    public GameObject emptyPlayer;
    public TMP_InputField yourName;
    public TextMeshProUGUI playerScore;
    public TextMeshProUGUI playerName;
    string leaderboardKey = "namahewan";
    public TextMeshProUGUI currentScore;
    public TextMeshProUGUI highScore;
    public string[] illegalText;

    void Start()
    {
        // Start Session
        StartSession();

        if (DataGameNama.dataScore >= PlayerPrefs.GetInt("scoreNamaHewan"))
        {
            PlayerPrefs.SetInt("scoreNamaHewan", DataGameNama.dataScore);            
        }

        currentScore.text = DataGameNama.dataScore.ToString();
        highScore.text = PlayerPrefs.GetInt("scoreNamaHewan").ToString();
    }

    /// <summary>
    /// Function start session to LootLocker Leaderboard
    /// </summary>
    public void StartSession()
    {
        LootLockerSDKManager.StartGuestSession((response) => 
        {
            if (response.success)
            {
                Debug.Log("Player Was Loggined");
                PlayerPrefs.SetString("PlayerID", response.player_id.ToString());
                Leaderboard();
            }
            else
            {
                Debug.Log("Failed Login!" + response.Error);
            }
        });
    }

    /// <summary>
    /// Function Load Leaderboard in LootLocker
    /// </summary>
    public void Leaderboard()
    {
        LootLockerSDKManager.GetScoreList(leaderboardKey, 10, 0, (response) =>
        {
            if(response.success)
            {
                noInternet.SetActive(false);
                loading.SetActive(false);
                string tempName = "";
                string tempScore = "";

                LootLockerLeaderboardMember[] members = response.items;

                if (members.Length == 0)
                {
                    emptyPlayer.SetActive(true);
                }

                else
                {
                    emptyPlayer.SetActive(false);
                    for (int i = 0; i < members.Length; i++)
                    {
                        tempName += members[i].rank + ". ";
                        if (members[i].player.name != "")
                        {
                            tempName += members[i].player.name;
                        }
                        else
                        {
                            tempName += members[i].score + "\n";
                        }
                        tempScore += members[i].score + "\n";
                        tempName += "\n";
                    }
                    playerName.text = tempName;
                    playerScore.text = tempScore;
                }
            }
            else
            {
                Debug.Log("Failed!" + response.Error);
                noInternet.SetActive(true);
                loading.SetActive(false);
            }
        });
    }

    /// <summary>
    /// Function Upload Score to Leaderboard in LootLocker
    /// </summary>
    public void SubmitScore()
    {
        string playerID = PlayerPrefs.GetString("PlayerID");
        LootLockerSDKManager.SubmitScore(playerID, PlayerPrefs.GetInt("scoreNamaHewan"), leaderboardKey, (response) =>
        {
            if (response.success)
            {
                Debug.Log("Successfully Upload Score");
                SetPlayerName();
            }
            else
            {
                Debug.Log("Failed Upload Score!" + response.Error);
            }
        });
    }

    /// <summary>
    /// Function Set Name Player Leaderboard in LootLocker
    /// </summary>
    public void SetPlayerName()
    {
        bool detectText = true;
        if (yourName.text == "")
        {
            Debug.Log("Name cannot be empty!");
            detectText = false;
        }

        else
        {
            for (int i = 0; i < illegalText.Length; i++)
            {
                if (yourName.text.ToLower() == illegalText[i])
                {
                    detectText = false;
                    break;
                }
            }
        }

        if (detectText)
        {
            LootLockerSDKManager.SetPlayerName(yourName.text, (response) =>
            {
                if (response.success)
                {
                    Debug.Log("Successfully Set Player Name");
                    Leaderboard();
                }
                else
                {
                    Debug.Log("Failed to Set Player Name!" + response.Error);
                }
            });
        }
    }
}
