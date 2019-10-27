using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class selectBtn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        SelectionButtonManager.instance.setImage(this.gameObject);
        SelectionButtonManager.instance.getItem(GetComponentInChildren<Text>().text,0);
        ItemToolTipManager.instance.hideTool();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
