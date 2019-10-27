using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class Slot : MonoBehaviour, IPointerDownHandler
{
    #region 成员
    //物品格子
    private GameObject m_image;
    //选中的物品
    private items m_items;
    //物品所有权
    private itemOwn own;
    public enum itemOwn
    {
        None,
        ownerplayer
    }
    #endregion

    // Start is called before the first frame update
    /// <summary>
    /// 设置物品
    /// </summary>
    /// <param name="_item">选中的物品</param>
    public virtual void setItem(items _item)
    {
        //设置物品所有权
        own = itemOwn.None;
        //从对象池中取出物品格子
        if (itemObjectpoolManager.instance.getitemQueue() != 0)
        {

            m_image = itemObjectpoolManager.instance.getItem();
            m_image.SetActive(true);

        }
        else
        {
            m_image = Instantiate(Resources.Load("prefab/ShopPrefab/UI/itemImage") as GameObject);

        }
        m_items = _item;
        //加载物品图片
        m_image.GetComponent<Image>().sprite = Resources.Load<Sprite>(_item.Sprite);
        //设置物品的父物体并设置位置缩放
        m_image.transform.parent = this.transform;
        m_image.transform.localPosition = Vector3.zero;
        m_image.transform.localScale = Vector3.one;

        //Debug.Log(this.transform.GetChild(0).transform.Find("Name").GetComponent<Text>().text);
        //Debug.Log(_item);
        //显示物品名字和简要说明
        this.transform.GetChild(0).transform.Find("Name").GetComponent<Text>().text = m_items.Name;
        this.transform.GetChild(0).transform.Find("synopsis").GetComponent<Text>().text = m_items.Synopsis;
        if (_item.Synopsis == "")
        {
            this.transform.GetChild(0).transform.Find("synopsis").gameObject.SetActive(false);
        }
        else
        {
            this.transform.GetChild(0).transform.Find("synopsis").gameObject.SetActive(true);
        }
        //显示物品的价格
        this.transform.GetChild(0).transform.Find("preice").GetComponent<Text>().text = _item.BuyPrice.ToString();



    }

   
    /// <summary>
    /// 给物品格子添加点击事件
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        
        Debug.Log(ItemToolTipManager.instance);
        Debug.Log(m_image.GetComponent<Image>());
        ItemToolTipManager.instance.showTool(m_image.GetComponent<Image>(), own, m_items,null);
        ItemToolTipManager.instance.showCompositePath(m_items,own);

    }

   
}
