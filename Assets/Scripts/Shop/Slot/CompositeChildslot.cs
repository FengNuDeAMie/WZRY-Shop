using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CompositeChildslot : Slot, IPointerDownHandler
{
    #region 成员
    //临时物品
    private GameObject temp;
    //物品格子
    private GameObject m_image;
    //临时合成子物品格
    private GameObject m_tempCompositeChildslot;
    //垂直线条
    private GameObject m_VerticalLine;
    //水平线条
    private GameObject m_HorizontalLine;
    //合成子物品格
    private GameObject m_compositeChildslot;
    //临时图片
    private Image m_tempCompositeSlot;
    //合成路径字符串
    private string[] m_compositepath = { };
    //合成物品列表
    private List<itemLeaf> m_itemList;
    //合成物品队列
    private List<GameObject> itemQueue = new List<GameObject>();
    //所有物品的字典
    private Dictionary<string, List<items>> dic;
    //选中的物品
    private items m_itemss;
    //合成树第一个物品
    private itemLeaf fatherItem;
    //合成树子物品
    private itemLeaf childItem;
    //是否点击事件
    private bool isPointDown = false;
    private itemOwn own;
    //物品层次
    private cenCI cen;
    public enum cenCI
    {
        One,
        Two
    }

    #endregion
    /// <summary>
    /// 初始化物品
    /// </summary>
    /// <param name="_item">选中的物品</param>
    /// <param name="itemLeaf">物品合成树</param>
    /// <param name="cI">物品的层次</param>
    public void setItem(items _item, itemLeaf itemLeaf, cenCI cI)
    {
        //itemQueue.Clear();

        cen = cI;
        own = itemOwn.None;
        m_itemss = _item;
        m_image = transform.Find("Image").gameObject;
        //Debug.Log(_image);

        m_image.GetComponent<Image>().sprite = Resources.Load<Sprite>(_item.Sprite);
        m_image.transform.parent = this.transform;
        m_image.transform.localPosition = Vector3.zero;
        m_image.transform.localScale = Vector3.one;
        //设置合成树第一个物品
        fatherItem = itemLeaf;
        //显示合成树
        showCompositePath(m_itemss);
    }
    /// <summary>
    /// 点击事件
    /// </summary>
    /// <param name="eventData"></param>
    public new void OnPointerDown(PointerEventData eventData)
    {

        Debug.Log("执行");
        for (int i = 0; i < fatherItem.getList().Count; i++)
        {
            Debug.Log(fatherItem.getList()[i].FatherItem.Name);
        }
        isPointDown = true;

        fixstring(m_itemss);
        Debug.Log(m_itemss.Explain);
        //显示提示面板
        ItemToolTipManager.instance.showTool(m_image.GetComponent<Image>(), own, this.fatherItem.FatherItem, this.fatherItem);



    }
    /// <summary>
    /// 显示合成路径
    /// </summary>
    /// <param name="_items">选中的物品</param>
    private void showCompositePath(items _items)
    {
        isPointDown = false;
        fixstring(_items);

    }
    /// <summary>
    /// 处理合成路径字符串
    /// </summary>
    /// <param name="_items">选中的物品</param>
    private void fixstring(items _items)
    {

        if (_items.CompositePath == "")
        {
            return;
        }
        m_compositepath = _items.CompositePath.Split('+');

        getChildSlot(m_compositepath);
        setChildSlotPos();

    }
    /// <summary>
    /// 根据合成路径加载物品
    /// </summary>
    /// <param name="v">合成路径</param>
    private void getChildSlot(string[] v)
    {
        dic = InventroyManager.instance.getItemDic();
        for (int i = 0; i < v.Length; i++)
        {
            foreach (var value in dic)
            {
                for (int j = 0; j < value.Value.Count; j++)
                {
                    if (v[i] == value.Value[j].Id.ToString())
                    {
                        childItem = new itemLeaf();
                        childItem.FatherItem = value.Value[j];
                        fatherItem.addList(childItem);
                    }
                }
            }
        }
        if (!isPointDown)
        {
            for (int i = 0; i < fatherItem.getList().Count; i++)
            {
                itemQueue.Add(getchildSlot());
            }
        }
    }
    /// <summary>
    /// 获取存放物品的格子
    /// </summary>
    /// <returns>返回一个空格子</returns>
    private GameObject getchildSlot()
    {

        if (itemObjectpoolManager.instance.getcompositechildslotQueue() != 0)
        {
            Debug.Log("这里");
            temp = itemObjectpoolManager.instance.getcompositechildslot();
            temp.SetActive(true);
            m_tempCompositeSlot = temp.GetComponent<Image>();
            Debug.Log("这里");
            m_tempCompositeSlot.gameObject.SetActive(true);
            m_tempCompositeSlot.transform.localPosition = Vector3.zero;
            return m_tempCompositeSlot.gameObject;
        }
        else
        {
            return InstantiateChild();
        }
    }
    /// <summary>
    /// 实例化物品格子
    /// </summary>
    /// <returns></returns>
    private GameObject InstantiateChild()
    {
        if (cen == cenCI.One)
        {
            m_tempCompositeChildslot = Instantiate(Resources.Load<GameObject>("prefab/ShopPrefab/Slot/CompositeChildslot"));
        }
        else
        {
            m_tempCompositeChildslot = Instantiate(this.gameObject);
        }

        setchild(m_tempCompositeChildslot);
        return m_tempCompositeChildslot;

    }
    /// <summary>
    /// 物品格子属性设为默认
    /// </summary>
    /// <param name="tempCompositeSlot">需要设置的物品格</param>
    private void setchild(GameObject tempCompositeSlot)
    {
        if (tempCompositeSlot.GetComponent<CompositeChildslot>() == null)
        {
            tempCompositeSlot.AddComponent<CompositeChildslot>();
        }
        tempCompositeSlot.SetActive(false);
    }

    /// <summary>
    /// 设置子物品位置
    /// </summary>
    public void setChildSlotPos()
    {
        float temp;
        float y;
        if (cen == cenCI.One)
        {
            temp = this.transform.localPosition.x - this.GetComponent<RectTransform>().rect.width * 3f;
            y = this.GetComponent<RectTransform>().rect.width * 6f / (itemQueue.Count - 1);
        }
        else
        {
            temp = this.transform.localPosition.x - this.GetComponent<RectTransform>().rect.width;
            y = this.GetComponent<RectTransform>().rect.width * 2f / (itemQueue.Count - 1);
        }


        if (itemQueue.Count > 1)
        {
            setOneVerticalLine();
            setHorizontalLine();
            for (int i = 0; i < itemQueue.Count; i++)
            {
                setSlotPos(itemQueue[i], new Vector2(temp + i * y,
                    this.transform.localPosition.y - this.GetComponent<RectTransform>().rect.height * 2f)
                    , this.fatherItem.getList()[i].FatherItem
                    , this.fatherItem.getList()[i]);
            }
        }
        else if (itemQueue.Count == 1)
        {
            for (int i = 0; i < itemQueue.Count; i++)
            {
                setOneVerticalLine();
                setSlotPos(itemQueue[i], new Vector2(this.transform.localPosition.x,
                    this.transform.localPosition.y - this.GetComponent<RectTransform>().rect.height * 2f)
                    , fatherItem.getList()[i].FatherItem
                    , fatherItem.getList()[i]);
            }
        }
        itemQueue.Clear();



    }


    /// <summary>
    /// 物品格子的初始化
    /// </summary>
    /// <param name="childGameObject">物品格</param>
    /// <param name="vector">位置</param>
    /// <param name="items">物品属性</param>
    /// <param name="itemLeaf">合成树</param>
    private void setSlotPos(GameObject childGameObject, Vector2 vector, items items, itemLeaf itemLeaf)
    {

        childGameObject.SetActive(true);
        //Debug.Log(this.transform.parent.name);
        childGameObject.transform.parent = this.transform.parent;
        childGameObject.transform.localScale = Vector3.one;
        childGameObject.transform.localPosition = vector;
        childGameObject.GetComponent<CompositeChildslot>().setItem(items, itemLeaf, cenCI.Two);
        setVerticalline(childGameObject);
    }
    /// <summary>
    /// 加载多条垂直线的方法
    /// </summary>
    /// <param name="gameObject">需要添加线条的物品</param>
    private void setVerticalline(GameObject gameObject)
    {
        //根据对象池加载线条
        if (itemObjectpoolManager.instance.getVerticallineQueue() != 0)
        {
            m_VerticalLine = itemObjectpoolManager.instance.getVerticalline();
        }
        else
        {
            m_VerticalLine = Instantiate(Resources.Load<GameObject>("prefab/ShopPrefab/CompositeLine/Verticalline"));
        }
        //初始化线条
        m_VerticalLine.gameObject.SetActive(true);
        m_VerticalLine.transform.parent = gameObject.transform.parent;
        m_VerticalLine.transform.localScale = Vector3.one;
        m_VerticalLine.transform.localPosition = new Vector2(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y + gameObject.GetComponent<RectTransform>().rect.height * 0.75f);
    }
    /// <summary>
    /// 添加水平线的方法
    /// </summary>
    private void setHorizontalLine()
    {
        //3层物品加载的水平线
        if (cen == cenCI.Two)
        {
            if (itemObjectpoolManager.instance.getVerticallineQueue() != 0)
            {
                m_VerticalLine = itemObjectpoolManager.instance.getVerticalline();
            }
            else
            {
                m_VerticalLine = Instantiate(Resources.Load<GameObject>("prefab/ShopPrefab/CompositeLine/Verticalline"));
            }
            //初始化线条
            m_VerticalLine.gameObject.SetActive(true);
            m_VerticalLine.transform.parent = this.transform.parent;
            m_VerticalLine.transform.localScale = Vector3.one;
            m_VerticalLine.transform.localPosition = new Vector2(this.transform.localPosition.x,
                this.transform.localPosition.y - this.GetComponent<RectTransform>().rect.height * 0.75f);
            if (itemObjectpoolManager.instance.getshortHorizontalLineQueue() != 0)
            {
                m_HorizontalLine = itemObjectpoolManager.instance.getshortHorizontalLine();
            }
            else
            {
                m_HorizontalLine = Instantiate(Resources.Load<GameObject>("prefab/ShopPrefab/CompositeLine/shortHorizontalLine"));
            }
        }

        //两层物品加载的水平线
        else
        {
            if (cen == cenCI.One)
            {
                if (itemObjectpoolManager.instance.getlongHorizontalLineQueue() != 0)
                {
                    m_HorizontalLine = itemObjectpoolManager.instance.getlongHorizontalLine();
                }
                else
                {
                    m_HorizontalLine = Instantiate(Resources.Load<GameObject>("prefab/ShopPrefab/CompositeLine/longHorizontalLine"));
                }
            }
            else
            {
                if (itemObjectpoolManager.instance.getshortHorizontalLineQueue() != 0)
                {
                    m_HorizontalLine = itemObjectpoolManager.instance.getshortHorizontalLine();
                }
                else
                {
                    m_HorizontalLine = Instantiate(Resources.Load<GameObject>("prefab/ShopPrefab/CompositeLine/shortHorizontalLine"));
                }

            }

        }
        //初始化线条
        m_HorizontalLine.SetActive(true);
        m_HorizontalLine.transform.parent = this.transform.parent;
        m_HorizontalLine.transform.localScale = Vector3.one;
        m_HorizontalLine.transform.localPosition = new Vector2(this.transform.localPosition.x, this.transform.localPosition.y - this.GetComponent<RectTransform>().rect.height);
    }
    /// <summary>
    /// 加载一条垂直线条的方法
    /// </summary>
    private void setOneVerticalLine()
    {
        //根据对象池加载线条
        if (itemObjectpoolManager.instance.getVerticallineQueue() != 0)
        {
            m_VerticalLine = itemObjectpoolManager.instance.getVerticalline();
        }
        else
        {
            m_VerticalLine = Instantiate(Resources.Load<GameObject>("prefab/ShopPrefab/CompositeLine/Verticalline"));
        }
        //初始化线条
        m_VerticalLine.gameObject.SetActive(true);
        m_VerticalLine.transform.parent = this.transform.parent;
        m_VerticalLine.transform.localScale = Vector3.one;
        m_VerticalLine.transform.localPosition = new Vector2(this.transform.localPosition.x, this.transform.localPosition.y - this.GetComponent<RectTransform>().rect.height * 0.75f);
    }
}
