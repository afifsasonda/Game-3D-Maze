using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoaderManager : MonoBehaviour
{
    // load
    public static void Load(string sceneName)
    {
        SceneLoader.Load(sceneName);
    }
    //progress load
    public static void ProgressLoad(string sceneName)
    {
        SceneLoader.ProgressLoad(sceneName);
    }
    //reload level
    public static void ReloadLevel()
    {
        SceneLoader.ReloadLevel();
    }
    //load next level1
    public static void LoadNextLevel()
    {
        SceneLoader.LoadNextLevel();
    }
}
