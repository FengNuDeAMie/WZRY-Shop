using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timing : MonoBehaviour
{
   
    int minute;
    int second;
    private  Text text_timeSpend;
    float timeSpend = 0.0f;
    private Playerattribute _playerattribute;
    // Start is called before the first frame update
    void Start()
    {
        text_timeSpend = GetComponent<Text>();
        _playerattribute = Playerattribute.getInstance();
        StartCoroutine(addGold());
    }

    // Update is called once per frame
    void Update()
    {
       
            timeSpend += Time.deltaTime;
            minute = ((int)timeSpend ) / 60;
            second = (int)timeSpend - (int)timeSpend / 3600 * 3600 - minute * 60;
           
            text_timeSpend.text = string.Format("{0:D2}:{1:D2}", minute, second);
       
       
    }
    IEnumerator addGold() {
        _playerattribute.GoldCoin++;
       yield return new WaitForSeconds(1);
        yield return addGold();
    }
}
