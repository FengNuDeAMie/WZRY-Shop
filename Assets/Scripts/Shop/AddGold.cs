using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddGold : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(addGold);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void addGold() {
        Playerattribute.getInstance().GoldCoin += 1000;
    }
}
