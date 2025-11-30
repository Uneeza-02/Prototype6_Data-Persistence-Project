using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;

    public Text HighScoreText;

    private bool m_Started = false;
    private int HighScore = 0;
    private int m_Points;

    private string TopPlayer = "";
    private string Name = "";
    
    private bool m_GameOver = false;

    
    // Start is called before the first frame update
    void Start()
    {
        //DeleteSaveFile();
        LoadPlayerData();
        SetNameAndScore();

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    void SetNameAndScore()
    {
        if (NameManager.Instance != null)
        {
            Name = NameManager.Instance.GetPlayerName();

            if (TopPlayer == "")
            {
                TopPlayer = Name;
            }
        }

        HighScoreText.text = "Best Score : " + TopPlayer + " : " + HighScore;
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        if (m_Points > HighScore) SavePlayerData();

        m_GameOver = true;
        GameOverText.SetActive(true);
    }

    [System.Serializable]
    class SaveData
    {
        public int HighScore;
        public string Name;
    }

    public void SavePlayerData()
    {
        SaveData data = new SaveData();
        data.HighScore = m_Points;
        data.Name = Name; 

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadPlayerData()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            HighScore = data.HighScore;
            TopPlayer = data.Name;
        }
    }

    public void Exit()
    {
        SceneManager.LoadScene(0);
    }

    public void DeleteSaveFile()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("Save file deleted.");
        }
        else
        {
            Debug.Log("No save file found.");
        }
    }
}
