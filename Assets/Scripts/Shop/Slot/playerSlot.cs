using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class playerSlot : Slot,IPointerDownHandler
{
    #region 成员
    //格子是否为空的标记
    public int isNull = -1;
    //物品所有权
    private itemOwn own ;
    //选中的物品
    public items item;
    #endregion
    // Start is called before the first frame update
    /// <summary>
    /// 点击事件
    /// </summary>
    /// <param name="eventData"></param>
    public new  void OnPointerDown(PointerEventData eventData)
    {
        if (isNull==-1)
        {
            return;
        }
        //更改物品所有权
        own = itemOwn.ownerplayer;
        if (this.transform.childCount==0)
        {
            ItemToolTipManager.instance.hideTool();
            return;
        }
        Debug.Log(this.transform.GetChild(0).gameObject.GetComponent<Image>().name);
        ItemToolTipManager.instance.showTool(this.transform.GetChild(0).gameObject.GetComponent<Image>(), own,item,null);
        ItemToolTipManager.instance.showCompositePath(item,own);
    }
    /// <summary>
    /// 设置背包格子的位置和缩放
    /// </summary>
    /// <param name="_image"></param>
    public void setImage(GameObject _image) {
        _image.transform.localPosition = Vector3.zero;
        _image.transform.parent = this.transform;
    }
    /// <summary>
    /// 设置背包格子的属性
    /// </summary>
    /// <param name="_isnull">是否为空的标记</param>
    /// <param name="_items">选中的物品</param>
    public void setplayerSlot(int _isnull,items _items) {
        isNull = _isnull;
        item = _items;
    } 
    
}
