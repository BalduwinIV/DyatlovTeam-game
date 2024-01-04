using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneOnTrigger : MonoBehaviour
{
    public string sceneName = "SecondLvl"; 
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            LoadScene();
        }
    }
    void LoadScene()
    {
        SceneManager.LoadScene(sceneName);
    }


}
