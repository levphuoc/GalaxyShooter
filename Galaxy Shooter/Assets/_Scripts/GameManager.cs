using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private PlayerController player;
    public static GameManager instance;
    public Text liveText;
 
    public float starWait;
    public GameObject[] enemies;
    public Boundary boundary;
    public Vector2 spawnWait;
    public int enemyCountMax = 10;
    public float spawnWaitMin;
    public float waveWait;
    public float waveWaitMin;
    public bool gameOver = false;
    private int enemyCount = 1;
    public GameObject loseWindow;
    public GameObject winWindow;
    public Slider starProgressBar; // Thanh tiến trình
    public int maxStars = 10; // Số sao cần để chiến thắng
    private int currentStars = 0; // Số sao đã thu thập
    public string nextLevelScreen;
    private bool isGameOver = false;
    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        instance = this;
      
    }

    private void Start()
    {
        StartCoroutine(SpawnWave());
    }

    private void Update()
    {
       
        if (!isGameOver) // Ch? ki?m tra GameOver n?u ch?a k?t thúc trò ch?i
        {
           
            GameOver(); // G?i GameOver() trong Update
        }


        // Check if bossCaller is null and no boss is instantiated
      
    }

    IEnumerator SpawnWave()
    {
        yield return new WaitForSeconds(starWait);

        int maxStarsOnScreen = 5; // Số sao tối đa trên màn hình
        float starSpawnCooldown = 3f; // Thời gian chờ tối thiểu giữa 2 lần spawn sao
        float lastStarSpawnTime = 0f;

        while (!gameOver)
        {
            for (int i = 0; i < enemyCount; i++)
            {
                GameObject enemy = enemies[Random.Range(0, enemies.Length)];
                bool isStar = enemy.CompareTag("Star"); // Kiểm tra nếu là sao

                // Kiểm soát số sao trên màn hình
                if (isStar)
                {
                    int starCount = GameObject.FindGameObjectsWithTag("Star").Length;

                    if (starCount >= maxStarsOnScreen || Time.time - lastStarSpawnTime < starSpawnCooldown)
                    {
                        continue; // Bỏ qua lần spawn này nếu vượt giới hạn hoặc cooldown chưa hết
                    }
                    lastStarSpawnTime = Time.time;
                }

                Vector3 spawnPosition = new Vector3(Random.Range(boundary.xMin, boundary.xMax), boundary.yMin, 0);
                Instantiate(enemy, spawnPosition, Quaternion.identity);
                yield return new WaitForSeconds(Random.Range(spawnWait.x, spawnWait.y));
            }

            enemyCount++;
            if (enemyCount >= enemyCountMax)
            {
                enemyCount = enemyCountMax;
                spawnWait.x = Mathf.Max(spawnWait.x - 0.1f, spawnWaitMin);
                spawnWait.y = Mathf.Max(spawnWait.y - 0.1f, spawnWaitMin);
                waveWait = Mathf.Max(waveWait - 0.1f, waveWaitMin);
            }

            yield return new WaitForSeconds(1f);
        }
    }

    public void GameOver()
    {
        if (player.lives <= 0 && !isGameOver) // Ch? g?i n?u ch?a có game over
        {
            isGameOver = true; // ?ánh d?u là game over
            loseWindow.SetActive(true);
            Time.timeScale = 0; // D?ng th?i gian
            AudioManager.instance.LoseMusic();
        }
        else
        {
            Time.timeScale = 1; // ??m b?o th?i gian v?n ch?y n?u ch?a game over
        }
    }
    public void GameWon()
    {
        winWindow.SetActive(true); // Show the win UI
        Time.timeScale = 0; // Stop the game

        AudioManager.instance.VictoryMusic();

    }
    public void ReplyLevel()
    {
        //Play again the scene theat we are currently inside of it
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        AudioManager.instance.LevelMusic();
    }
    public void NextLevel()
    {
        SceneManager.LoadScene(nextLevelScreen);
        AudioManager.instance.LevelMusic();
    }
    public void BackToMainMenu()
    {
        //Load the scene with index zero
        SceneManager.LoadScene(0);

        AudioManager.instance.LevelMusic();
    }

    public void SetLivesText(int lives)
    {
        liveText.text = "x " + lives.ToString();
    }

   
    
    public void AddStar()
    {
        currentStars++;
        starProgressBar.value = (float)currentStars / maxStars; // Cập nhật thanh tiến trình

        if (currentStars >= maxStars)
        {
            GameWon(); // Kích hoạt chiến thắng
        }
    }
}
