using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LodingScene : MonoBehaviour
{
    Slider _slider;
    Text _text;
    AsyncOperation async = null;
    float _curProgressValue = 0f;

    // Start is called before the first frame update
    void Start()
    {
        _slider = GameObject.FindGameObjectWithTag("NormalSlider").GetComponent<Slider>();
        _text = GameObject.FindGameObjectWithTag("NormalText").GetComponent<Text>();
        _text.text = "%";
        StartCoroutine("LoadScene");
    }

    IEnumerator LoadScene()
    {
        async = SceneManager.LoadSceneAsync("battleScene");
        async.allowSceneActivation = false;
        while (async.progress<0.9f)
        {
            Debug.Log(async.progress);
            yield return new WaitForEndOfFrame();
        }
        Debug.Log(async.progress);
        _curProgressValue = async.progress;
        Debug.Log(async.ToString());
        async.allowSceneActivation = true;


    }

    // Update is called once per frame
    void Update()
    {
        _slider.value = _curProgressValue;
        _text.text = (async.progress*100).ToString()+"%";
    }
}
