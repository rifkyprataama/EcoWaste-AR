using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroLevel : MonoBehaviour
{
    void onEnable()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

}
