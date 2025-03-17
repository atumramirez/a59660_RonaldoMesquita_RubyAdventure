using UnityEngine;
using TMPro;

public class RobotManager : MonoBehaviour
{
    public int totalRobots = 10;
    public int fixedRobots = 0;
    private int playerRobots;
    public TMP_Text robotsText;

    void Start()
    {
        playerRobots = fixedRobots;

        if (fixedRobots > totalRobots)
        {
            fixedRobots = totalRobots;
        }
    }

    public void AddRobots(int amount)
    {
        if (playerRobots + amount <= totalRobots)
        {
            playerRobots += amount;
        }
        UpdateText();
    }
    public void RemoveRobots(int amount)
    {
        if (playerRobots - amount >= fixedRobots)
        {
            playerRobots -= amount;
        }
        UpdateText();
    }

    public int GetPlayerRobots()
    {
        return playerRobots;
    }

    public int GetTotalRobots()
    {
        return totalRobots;
    }

    public void UpdateText()
    {
        robotsText.text = playerRobots + "/" + totalRobots;
    }
}