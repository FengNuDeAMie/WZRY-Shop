using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetGold : MonoBehaviour
{
    private Playerattribute _playerattribute;
    private Text _GoldText;
    // Start is called before the first frame update
    void Start()
    {
        _playerattribute = Playerattribute.getInstance();
        _GoldText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        _GoldText.text = _playerattribute.GoldCoin.ToString();
    }
}
