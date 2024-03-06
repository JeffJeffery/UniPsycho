using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeAfterDelay : MonoBehaviour
{
    public string targetScene;
    public float delayTime = 5.0f;

    void Start()
    {
        StartCoroutine(ChangeSceneAfterDelay());
    }

    IEnumerator ChangeSceneAfterDelay()
    {
        yield return new WaitForSeconds(delayTime);
        SceneManager.LoadScene(targetScene);
    }
}