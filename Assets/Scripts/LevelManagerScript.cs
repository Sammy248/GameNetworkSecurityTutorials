using UnityEngine;
using UnityEngine.UI;

public class LevelManagerScript : MonoBehaviour
{
    public int enemies = 5;
    public Text enemiesText;
    private void Awake()
    {
        enemiesText.text = enemies.ToString();

        Enemy.onEnemyKilled += onEnemyKilledAction;
    }
    void onEnemyKilledAction()
    {
        enemies--;
        enemiesText.text = enemies.ToString();
    }
}
