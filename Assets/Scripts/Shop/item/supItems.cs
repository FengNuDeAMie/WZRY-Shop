using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class supItems : items
{
    public supItems(int id, string name, int buyPrice, int sellPrice, string sprite, ItemType itemType, ItemQuality itemQuality, ItemjiNeng itemjiNeng, string synopis, string explain, string compounds, string compositepath) : base(id, name, buyPrice, sellPrice, sprite, itemType, itemQuality, itemjiNeng, synopis, explain,compounds,compositepath)
    {
    }
}
