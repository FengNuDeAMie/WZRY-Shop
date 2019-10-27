using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionButtonManager : MonoBehaviour
{
    #region 成员
    //单例字段
    public static SelectionButtonManager instance;
    #region 手动添加成员
    [SerializeField]
    private GameObject m_ScrollRectVertical;
    [SerializeField]
    private GameObject m_ScrollRectHorizontal;
    [SerializeField]
    private Button m_showButton;
    #endregion
    #region 代码初始化成员
    //按钮指示图标
    private Image m_image;
    // Start is called before the first frame update
    //按钮数组
    private GameObject[] m_buttons;
    //按钮名称列表
    List<string> m_btnName = new List<string>();
    //items存储列表
    private List<items> m_items = new List<items>();
    //物品格子图标
    private Image m_slot;
    //物品格子临时图标
    private Image m_tempslpt;
    //临时字符串
    private string m_temptext = null;
    #endregion
    #region 枚举成员
    private enum showEnum {
        Horizontal,
        Vertical
    }
    private showEnum Showenum;
    #endregion
    #endregion
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        
        //初始化展示状态
        Showenum= showEnum.Vertical;
        
        switch (Showenum)
        {
            case showEnum.Horizontal:
                m_ScrollRectVertical.SetActive(false);
                m_ScrollRectHorizontal.SetActive(true);
                ItemToolTipManager.instance.setItemInventroy(m_ScrollRectHorizontal.transform);
                break;
            case showEnum.Vertical:
                m_ScrollRectVertical.SetActive(true);
                m_ScrollRectHorizontal.SetActive(false);
                ItemToolTipManager.instance.setItemInventroy(m_ScrollRectVertical.transform);
                break;
           
        }
        //初始化按钮名字
        m_btnName.Add("推荐");
        m_btnName.Add("攻击");
        m_btnName.Add("法术");
        m_btnName.Add("防御");
        m_btnName.Add("移动");
        m_btnName.Add("打野");
        m_btnName.Add("辅助");
        //初始化按钮
        m_buttons = GameObject.FindGameObjectsWithTag("selectbtn");
        for (int i = 0; i < m_btnName.Count; i++)
        {
            m_buttons[i].GetComponentInChildren<Text>().text = m_btnName[i];
            m_buttons[i].AddComponent<selectBtn>();
        }
        //加载物品格子
        m_tempslpt = Resources.Load<Image>("prefab/ShopPrefab/Slot/slot");
        //加载指示图标图片
        m_image = Resources.Load<Image>("prefab/ShopPrefab/UI/btnTitle");
        //实例化
        m_image = Instantiate(m_image);
        //添加显示按钮事件
        m_showButton.onClick.AddListener(showClick);
    }
    /// <summary>
    /// 物品展示状态的方法
    /// </summary>
    private void showClick()
    {
        if (Showenum==showEnum.Vertical)
        {
            Showenum = showEnum.Horizontal;
            itemObjectpoolManager.instance.setObjecctpool();
            ItemToolTipManager.instance.setItemInventroy(m_ScrollRectHorizontal.transform);
            m_ScrollRectVertical.SetActive(false);
            m_ScrollRectHorizontal.SetActive(true);
            getItem(m_image.transform.parent.GetComponentInChildren<Text>().text,1);
        }
        else
        {
            Showenum = showEnum.Vertical;
            itemObjectpoolManager.instance.setObjecctpool();
            ItemToolTipManager.instance.setItemInventroy(m_ScrollRectVertical.transform);
            m_ScrollRectVertical.SetActive(true);
            m_ScrollRectHorizontal.SetActive(false);
            getItem(m_image.transform.parent.GetComponentInChildren<Text>().text,1);
        }
    }

    /// <summary>
    /// 设置按钮指示图标的方法
    /// </summary>
    /// <param name="button">选择的按钮</param>
    public void setImage(GameObject button)
    {
        foreach (var item in m_buttons)
        {
            if (item == button)
            {
                m_image.transform.parent = item.transform;
                m_image.transform.localScale = item.transform.localScale;
                m_image.transform.position = item.transform.position;
                m_image.transform.SetAsFirstSibling();
            }
        }
    }
    /// <summary>
    /// 根据按钮加载对应物体的方法
    /// </summary>
    /// <param name="_text">按钮的名字</param>
    public void getItem(string _text,int i)
    {
        if (m_temptext==_text&&i==0)
        {
            return;
        }
        m_temptext = _text;
        itemObjectpoolManager.instance.setObjecctpool();
        m_items = InventroyManager.instance.Getitems(m_temptext);
        if (m_items==null)
        {
            return;
        }
        //遍历存储列表加载物品
        foreach (var item in m_items)
        {
            switch (item._ItemQuality)
            {
                case global::items.ItemQuality.chuJi:
                    getSlot(item);
                    m_slot.transform.parent = GameObject.FindGameObjectWithTag("chujiPanle").transform;
                    m_slot.gameObject.transform.localScale = Vector3.one;
                    m_slot.transform.localPosition = Vector3.zero;
                    break;
                case global::items.ItemQuality.zhongJi:
                    getSlot(item);
                    m_slot.transform.parent = GameObject.FindGameObjectWithTag("zhongjiPanle").transform;
                    m_slot.gameObject.transform.localScale = Vector3.one;
                    m_slot.transform.localPosition = Vector3.zero;
                    break;
                case global::items.ItemQuality.gaoJi:
                    getSlot(item);
                    m_slot.transform.parent = GameObject.FindGameObjectWithTag("gaojiPanle").transform;
                    m_slot.gameObject.transform.localScale = Vector3.one;
                    m_slot.transform.localPosition = Vector3.zero;
                    break;
                default:
                    break;
            }
        }
    }
    /// <summary>
    /// 根据物品加载物品格子
    /// </summary>
    /// <param name="item">物品类型</param>
    private void getSlot(items item)
    {
        if (itemObjectpoolManager.instance.getslotQueue() != 0)
        {
            m_slot = itemObjectpoolManager.instance.getslot().GetComponent<Image>();
            m_slot.gameObject.SetActive(true);
            m_slot.transform.localPosition = Vector3.zero;
            m_slot.GetComponent<Slot>().setItem(item);
        }
        else
        {
            InstantiateImage(item);
        }
    }
    /// <summary>
    /// 根据item克隆格子
    /// </summary>
    /// <param name="item">物品</param>
    private void InstantiateImage(items item)
    {
        m_slot = Instantiate(m_tempslpt);
        m_slot.transform.localScale = Vector3.one;
        setit(m_slot, item);
    }
    /// <summary>
    /// 设置格子属性
    /// </summary>
    /// <param name="_tempGO">格子图片</param>
    /// <param name="items"></param>
    void setit(Image _tempGO,items items) {
        if (!_tempGO.gameObject.AddComponent<Slot>())
        {
            _tempGO.gameObject.AddComponent<Slot>();
        }
        _tempGO.GetComponent<Slot>().setItem(items);
    }

    
}
