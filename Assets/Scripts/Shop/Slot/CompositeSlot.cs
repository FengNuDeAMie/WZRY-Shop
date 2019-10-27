using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CompositeSlot : Slot, IPointerDownHandler
{
    #region 成员
    //物品格子
    private GameObject m_image;
    //选中的物品
    private items m_items;
    //物品所有权
    private itemOwn own;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        m_image = transform.Find("Image").gameObject;
    }
    //初始化话物品
    public new void setItem(items _item)
    {
        own = itemOwn.None;
        m_items = _item;
        m_image = transform.Find("Image").gameObject;
        //Debug.Log(_image);
        m_image.GetComponent<Image>().sprite = Resources.Load<Sprite>(_item.Sprite);
        m_image.transform.parent = this.transform;
        m_image.transform.localPosition = Vector3.zero;
        m_image.transform.localScale = Vector3.one;
    }
    //点击事件
    public new void OnPointerDown(PointerEventData eventData)
    {
        ItemToolTipManager.instance.showTool(m_image.GetComponent<Image>(), own, m_items, null);
        ItemToolTipManager.instance.showCompositePath(m_items, own);
    }

    //public void OnPointerUp(PointerEventData eventData)
    //{
    //    itemToolManager.instanse.showTool(_image.GetComponent<Image>(), own, _items, null);
    //    itemToolManager.instanse.showCompositePath(_items);
    //}
}
