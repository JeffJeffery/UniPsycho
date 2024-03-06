using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeTrigger : MonoBehaviour
{
    public string targetScene;
    public string targetSceneTrue;
    public int noseCount = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ChangeScene();
        }
    }

    private void ChangeScene()
    {
        if(noseCount >= 50)
        {
            SceneManager.LoadScene(targetSceneTrue);
        } else
        {
            SceneManager.LoadScene(targetScene);
        }
    }
}