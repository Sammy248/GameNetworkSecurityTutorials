using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;

public class GlobalLeaderboard : MonoBehaviour
{
    //public string leaderboardName;
    int maxResults = 5;
    public LeaderboardPopup leaderboardPopup;
    public void GetLeaderboard(string statisticName)
    {
        Debug.Log("Getting leaderboard");
        GetLeaderboardRequest request = new GetLeaderboardRequest()
        {
            MaxResultsCount = maxResults,
            StatisticName = statisticName,
        };
        PlayFabClientAPI.GetLeaderboard(request, PlayFabGetLeaderboardResult, PlayFabGetLeaderboardError);
    }
    void PlayFabGetLeaderboardResult(GetLeaderboardResult getLeaderboardResult)
    {
        Debug.Log("Playfab - Get Leaderboard completed");
        leaderboardPopup.UpdateUI(getLeaderboardResult.Leaderboard);
    }
    void PlayFabGetLeaderboardError(PlayFabError getLeaderboardError)
    {
        Debug.Log("PlayFab - Error occurred while getting Leaderboard: " + getLeaderboardError.ErrorMessage);

    }
    public void SubmitScore(string leaderboardName, int statistic)
    {
        UpdatePlayerStatisticsRequest request = new UpdatePlayerStatisticsRequest()
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate()
                {
                    StatisticName = leaderboardName,
                    Value = statistic
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, PlayFabUpdateStatsResult, PlayFabUpdateStatsError);
    }

    void PlayFabUpdateStatsResult(UpdatePlayerStatisticsResult updatePlayerStatisticsResult)
    {
        Debug.Log("Playfab - Score Submitted");
    }
    void PlayFabUpdateStatsError(PlayFabError updatePlayerStatisticsError)
    {
        Debug.Log("PlayFab - Error occurred while submitting score: " + updatePlayerStatisticsError.ErrorMessage);

    }
}
public static class PlayFabStats
{
    public const string MostKills = "Most Kills";
    public const string QuickestWin = "Quickest Win";
}
