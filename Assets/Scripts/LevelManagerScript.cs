using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManagerScript : MonoBehaviour
{
    public int enemies = 0;
    public Text enemiesText;

    public Text restartTimer;
    public Canvas restartCanvas;
    float timer = 5f;
    bool timerBool = false;
    private void Awake()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy").Length;

        enemiesText.text = enemies.ToString();

        Enemy.onEnemyKilled += onEnemyKilledAction;
    }
    void Update()
    {
        if (timerBool)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                SceneManager.LoadScene("SinglePlayer");
            }
            restartTimer.text = timer.ToString();
        }
    }
    void onEnemyKilledAction()
    {        
        enemies--;
        enemiesText.text = enemies.ToString();
    }
    public void onPlayerKilledAction()
    {
        restartCanvas.gameObject.SetActive(true);
        timer = 10;
        timerBool = true;
    }
}

