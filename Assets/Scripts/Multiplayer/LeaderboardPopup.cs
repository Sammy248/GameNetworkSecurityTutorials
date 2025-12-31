using NUnit.Framework;
using PlayFab.ClientModels;
using UnityEngine;
//using System.Collections.Generic;

public class LeaderboardPopup : MonoBehaviour
{
    public GameObject scoreHolder;
    public GameObject noScoreText;
    public GameObject leaderboardItem;

    private void OnEnable()
    {
        GameManager.instance.globalLeaderboard.GetLeaderboard();
    }
    public void UpdateUI(List<PlayerLeaderboardEntry> playerLeaderboardEntries)
    {
        Debug.Log("entries:" + playerLeaderboardEntries.Count);
        if (playerLeaderboardEntries.Count > 0)         //always has 0
        {
            DestroyChildren(scoreHolder.transform);
            for (int i = 0; i <playerLeaderboardEntries.Count; i++)
            {
                GameObject newLeaderboardItem = Instantiate(leaderboardItem, Vector3.zero, Quaternion.identity, scoreHolder.transform);
                newLeaderboardItem.GetComponent<LeaderboardItem>().SetScores(i + 1, playerLeaderboardEntries[i].PlayFabId, playerLeaderboardEntries[i].StatValue);
            }
            scoreHolder.SetActive(true);
            noScoreText.SetActive(false);
        }
        else
        {
            scoreHolder.SetActive(false);
            noScoreText.SetActive(true);
        }
    }
    void DestroyChildren(Transform parent)
    {
        foreach(Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }
}
