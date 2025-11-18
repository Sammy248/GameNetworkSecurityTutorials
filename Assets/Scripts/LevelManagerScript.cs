using UnityEngine;
using UnityEngine.UI;

public class LevelManagerScript : MonoBehaviour
{
    public int enemies = 0;
    public Text enemiesText;
    private void Awake()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy").Length;

        enemiesText.text = enemies.ToString();

        Enemy.onEnemyKilled += onEnemyKilledAction;
    }
    void onEnemyKilledAction()
    {        
        enemies--;
        enemiesText.text = enemies.ToString();
    }
}
