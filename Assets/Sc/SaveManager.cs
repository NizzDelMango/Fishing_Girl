using UnityEngine;

public static class SaveManager
{
    public static void SavePlayerData(int level, int exp, int gold, int rodIndex, int[] fishCounts)
    {
        PlayerPrefs.SetInt("Level", level);
        PlayerPrefs.SetInt("Exp", exp);
        PlayerPrefs.SetInt("Gold", gold);
        PlayerPrefs.SetInt("RodIndex", rodIndex);

        for (int i = 0; i < fishCounts.Length; i++)
            PlayerPrefs.SetInt("Fish_" + i, fishCounts[i]);

        PlayerPrefs.Save();
        Debug.Log("저장 완료");
    }

    public static int LoadInt(string key, int defaultValue = 0)
    {
        return PlayerPrefs.GetInt(key, defaultValue);
    }

    public static int[] LoadFishCounts(int count)
    {
        int[] fishCounts = new int[count];
        for (int i = 0; i < count; i++)
            fishCounts[i] = PlayerPrefs.GetInt("Fish_" + i, 0);
        return fishCounts;
    }
}
