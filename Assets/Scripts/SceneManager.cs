using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneManager : MonoBehaviour
{


    // Update is called once per frame
    void Update()
    {
        {
            // Only specifying the sceneName or sceneBuildIndex will load the Scene with the Single mode
            SceneManager.LoadScene("OtherSceneName", LoadSceneMode.Additive);
        }
    }
}
