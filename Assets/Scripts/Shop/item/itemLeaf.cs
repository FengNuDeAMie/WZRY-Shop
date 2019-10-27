using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemLeaf

{
    private items fatherItem;
    private List<itemLeaf> itemList;
    public itemLeaf()
    {
        itemList = new List<itemLeaf>();
    }

    public items FatherItem { get => fatherItem; set => fatherItem = value; }
   

    public void addList(itemLeaf items) {
        itemList.Add(items);
    }
    public List<itemLeaf> getList() {
        return this.itemList;
    }
    public int listLength() {
        return itemList.Count - 1;
    }
    public void setList(List<itemLeaf> _itemList) {
        this.itemList = _itemList;
    }
}
