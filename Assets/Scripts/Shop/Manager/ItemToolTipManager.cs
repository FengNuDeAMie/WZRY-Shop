using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemToolTipManager : MonoBehaviour
{
    #region 成员
    //单例模式的静态字段
    public static ItemToolTipManager instance;
    #region Unity成员
    #region 手动添加成员
    //打开合成路径的图标
    [SerializeField]
    private Sprite m_openSprite;
    //关闭合成路径的图标
    [SerializeField]
    private Sprite m_closeSprite;

    //物品提示的面板
    [SerializeField]
    private Transform m_Compositepanel;
    //物品合成路径格子的父对象
    [SerializeField]
    private GameObject m_fatherGameObject;
    //显示金钱的文本
    [SerializeField]
    private Text m_goldText;
    //物品说名文本
    [SerializeField]
    private Text m_Explain;
    //物品合成面板
    [SerializeField]
    private Transform m_CompositeslotPanle;
    [SerializeField]
    //可合成物品的滑动范围
    private GameObject m_ScrollRect;
    #endregion
    #region 代码初始化成员
    private Transform m_ItemInventroy;
    //物品格子
    private Image m_compositeSlot;
    //物品格子的临时对象
    private Image m_tempCompositeSlot;
    //物品的图标
    private Image m_itemImage;
    //操作按钮
    private Button m_caozuo;
    //合成路径的按钮
    private Button m_btnTool;
    //购买物品的按钮
    private Button m_buy;
    //物品名字文本
    private Text m_name;
    //从对象池中取出的物品格子
    private GameObject m_temp;
    //卖出物品对象
    private GameObject m_sellItem;
    //合成路径的物品格子
    private GameObject m_compositeChildslot;
    //临时对象
    private GameObject m_tempCompositeChildslot;
    private RawImage m_VerticalLine;
    private GameObject m_HorizontalLine;
    #endregion
    #endregion
    #region 数据结构成员
    //物品说名字符数组
    private string[] m_tempstring = { };
    //物品说名字符数组
    private string[] m_strarr = { };
    //保存合成路径的字符数组
    private string[] m_compositepath = { };
    //保存物品的字典
    private Dictionary<string, List<items>> m_dic;
    //保存物品格子的队列
    private Queue<GameObject> m_itemQueue = new Queue<GameObject>();
    //保存父物体结点的列表
    private List<itemLeaf> m_fatherItemLeaf;
    //保存子物品结点的列表
    private List<itemLeaf> m_childItemLeaf;
    //保存玩家物品格子的队列
    private Queue<GameObject> m_playSlotQueue = new Queue<GameObject>();
    //选中的物品
    private items m_item;
    //保存父节点
    private itemLeaf m_fatherItem;
    //子节点
    private itemLeaf m_childItem;
    int itemPrice = 0;
    bool isFound = false;
    #endregion
    #region 枚举成员
    //控制面板开关的枚举
    private enum ToolisOpen
    {
        close,
        open
    }
    //开关枚举的变量
    private ToolisOpen toolis;
    #endregion
    #endregion

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        toolis = ToolisOpen.close;
        m_compositeChildslot = Resources.Load<GameObject>("prefab/ShopPrefab/Slot/CompositeChildslot");
        m_compositeSlot = Resources.Load<Image>("prefab/ShopPrefab/Slot/Compositeslot");
        m_itemImage = Resources.Load<Image>("prefab/ShopPrefab/UI/_image");
        m_name = transform.Find("Name").gameObject.GetComponent<Text>();
        m_caozuo = transform.Find("tempButton").gameObject.GetComponent<Button>();
        m_btnTool = transform.Find("CompositePathBtn").gameObject.GetComponent<Button>();
        m_buy = transform.Find("buyButton").gameObject.GetComponent<Button>();
        m_Compositepanel.localScale = Vector3.zero;

        m_btnTool.onClick.AddListener(btnTool);
        m_caozuo.onClick.AddListener(Caozuo);

        m_buy.onClick.AddListener(buyItem);

        this.gameObject.SetActive(false);
    }



    // Update is called once per frame
    #region 面板的显示和隐藏
    /// <summary>
    /// 显示物品提示面板
    /// </summary>
    /// <param name="_image">选中物品的图标</param>
    /// <param name="_itemOwn">选中物品格子的所有权</param>
    /// <param name="_item">选中的物品</param>
    /// <param name="_itemLeaf">选中物品的子物品</param>
    public void showTool(Image _image, Slot.itemOwn _itemOwn, items _item, itemLeaf _itemLeaf)
    {
        m_item = _item;
        //显示面板
        this.gameObject.SetActive(true);
        //根据物品栏所有权加载按钮
        switch (_itemOwn)
        {
            case Slot.itemOwn.None:
                m_caozuo.GetComponentInChildren<Text>().text = "预购";
                m_buy.gameObject.SetActive(true);
                m_buy.GetComponentInChildren<Text>().text = "购买";

                break;
            case Slot.itemOwn.ownerplayer:
                m_caozuo.GetComponentInChildren<Text>().text = "出售";
                m_sellItem = _image.transform.parent.gameObject;
                m_buy.gameObject.SetActive(false);
                break;
            default:
                break;
        }
        //存入对象池
        itemObjectpoolManager.instance.setCompositeslotpool();
        //物品名字的显示
        m_name.text = _item.Name;
        //物品说名的显示
        m_Explain.text = Processingstrings(_item.Explain);
        //显示可合成物品
        showCompositeSlot(_item);
        //根据是否有物品树显示金钱
        if (_itemLeaf != null)
        {
            m_fatherItem = _itemLeaf;
            m_goldText.text = showGoldText();
        }
        m_itemImage.sprite = _image.sprite;


    }
    /// <summary>
    /// 隐藏物品提示面板
    /// </summary>
    public void hideTool()
    {
        //DrawLine.instance.stopDraw();
        //合成路径关闭
        toolis = ToolisOpen.close;
        //显示所有物品的面板
        m_ItemInventroy.localScale = Vector3.one;
        //设置所有物品面板的滑动开始范围
        m_ItemInventroy.GetComponent<ScrollRect>().content.localPosition = new Vector2(m_ItemInventroy.GetComponent<ScrollRect>().content.localPosition.x, 0);
        //关闭可合成物品面板
        m_Compositepanel.localScale = Vector3.zero;
        //设置合成路径按钮为开启图标
        m_btnTool.gameObject.GetComponent<Image>().sprite = m_openSprite;
        itemObjectpoolManager.instance.setCompositechildslotpool();
        this.gameObject.SetActive(false);
    }
    /// <summary>
    /// 设置所有物品面板的方法
    /// </summary>
    /// <param name="transform">所有物品的Transform</param>
    public void setItemInventroy(Transform transform)
    {
        m_ItemInventroy = transform;
    }
    #endregion


    #region 显示物品金钱的算法
    /// <summary>
    /// 返回选中物品的金钱
    /// </summary>
    /// <returns></returns>
    private string showGoldText()
    {
        m_playSlotQueue.Clear();
        itemPrice = m_item.BuyPrice;
        Debug.Log(m_item.BuyPrice+"jslkfj"+m_item.Name);
        if (m_fatherItem.getList().Count != 0)
        {
            //根据物品树获取到子物品列表
            m_fatherItemLeaf = m_fatherItem.getList();
            for (int soltList = 0; soltList < PlayerSlotManager.instace.getList().Count; soltList++)
            {

                isFound = false;
                if (PlayerSlotManager.instace.getList()[soltList].GetComponent<playerSlot>().isNull == -1)
                {
                    continue;
                }


                for (int i = 0; i < m_fatherItemLeaf.Count; i++)
                {
                    if (isFound)
                    {
                        continue;
                    }
                    //Debug.Log(fatherItemLeaf.Count);
                    if (m_fatherItemLeaf.Count == 0)
                    {
                        continue;
                    }
                    if (m_fatherItemLeaf[i].FatherItem.Id == PlayerSlotManager.instace.getList()[soltList].GetComponent<playerSlot>().item.Id)
                    {
                        Debug.Log(m_item.BuyPrice);
                        isFound = true;
                        m_playSlotQueue.Enqueue(PlayerSlotManager.instace.getList()[soltList]);
                        itemPrice -= PlayerSlotManager.instace.getList()[soltList].GetComponent<playerSlot>().item.BuyPrice;
                        m_fatherItemLeaf.Remove(m_fatherItemLeaf[i]);
                        continue;
                    }
                    if (m_fatherItemLeaf[i].getList().Count != 0)
                    {
                        if (m_childItemLeaf == null)
                        {
                            m_childItemLeaf = m_fatherItemLeaf[i].getList();
                        }
                        m_childItemLeaf = m_fatherItemLeaf[i].getList();

                    }
                    else
                    {
                        continue;
                    }
                    Debug.Log(m_item.BuyPrice);
                    //Debug.Log(fatherItem.getList()[i].FatherItem.Name);
                    for (int j = 0; j < m_childItemLeaf.Count; j++)
                    {
                        if (isFound)
                        {
                            continue;
                        }
                        if (m_childItemLeaf.Count == 0)
                        {
                            continue;
                        }
                        if (m_childItemLeaf[j].FatherItem.Id == PlayerSlotManager.instace.getList()[soltList].GetComponent<playerSlot>().item.Id)
                        {

                            isFound = true;

                            m_playSlotQueue.Enqueue(PlayerSlotManager.instace.getList()[soltList]);
                            itemPrice -= PlayerSlotManager.instace.getList()[soltList].GetComponent<playerSlot>().item.BuyPrice;
                            //childItemLeaf[j].getList().Remove(childItemLeaf[j]);
                            m_childItemLeaf.Remove(m_childItemLeaf[j]);

                            continue;
                        }
                        //    Debug.Log(fatherItem.getList()[i].getList()[j].FatherItem.Name);
                    }
                }

            }
            //清空子物品
            m_childItemLeaf = null;
        }


        return itemPrice.ToString();
    }
    #endregion


    #region 显示可合成物品
    /// <summary>
    /// 分割字符串
    /// </summary>
    /// <param name="_item">选中的物品</param>
    private void showCompositeSlot(items _item)
    {

        if (_item.Compounds == "")
        {
            return;
        }
        getCompositeSlot(_item.Compounds.Split('+'));


    }
    /// <summary>
    /// 获取可合成的物品
    /// </summary>
    /// <param name="v">可合成物品字符串</param>
    private void getCompositeSlot(string[] v)
    {
        m_ScrollRect.GetComponent<ScrollRect>().content.localPosition = Vector2.zero;
        m_dic = InventroyManager.instance.getItemDic();
        if (m_dic.ContainsKey("推荐"))
        {
            m_dic.Remove("推荐");
        }
        for (int i = 0; i < v.Length; i++)
        {
            foreach (var value in m_dic)
            {
                for (int j = 0; j < value.Value.Count; j++)
                {
                    if (v[i] == value.Value[j].Id.ToString())
                    {
                        getSlot(value.Value[j]);
                        m_tempCompositeSlot.transform.parent = m_CompositeslotPanle.transform;
                        m_tempCompositeSlot.transform.localScale = Vector3.one;
                    }
                }
            }
        }
    }
    /// <summary>
    /// 设置物品格子
    /// </summary>
    /// <param name="items">可合成的物品</param>
    private void getSlot(items items)
    {
        //Debug.Log(itemObjectpoolManager.instance.getcompositeslotQueue());
        if (itemObjectpoolManager.instance.getcompositeslotQueue() != 0)
        {
            //Debug.Log("这里");
            m_temp = itemObjectpoolManager.instance.getCompositeslot();
            m_temp.SetActive(true);
            m_tempCompositeSlot = m_temp.GetComponent<Image>();
            //Debug.Log("这里");
            m_tempCompositeSlot.gameObject.SetActive(true);
            m_tempCompositeSlot.transform.localPosition = Vector3.zero;

            m_tempCompositeSlot.GetComponent<CompositeSlot>().setItem(items);
        }
        else
        {
            InstantiateImage(items);
        }
    }
    /// <summary>
    /// 克隆物品格子
    /// </summary>
    /// <param name="item">合成的物品</param>
    private void InstantiateImage(items item)
    {
        m_tempCompositeSlot = Instantiate(m_compositeSlot);
        m_tempCompositeSlot.transform.localScale = Vector3.one;
        setit(m_tempCompositeSlot, item);
    }
    /// <summary>
    /// 克隆物品格子之后设置物品格子的属性
    /// </summary>
    /// <param name="tempCompositeSlot">物品格子</param>
    /// <param name="item">物品</param>
    private void setit(Image tempCompositeSlot, items item)
    {
        if (!tempCompositeSlot.gameObject.AddComponent<CompositeSlot>())
        {
            tempCompositeSlot.gameObject.AddComponent<CompositeSlot>();
        }
        tempCompositeSlot.gameObject.GetComponent<CompositeSlot>().setItem(item);
    }
    #endregion
    #region 按钮事件

    /// <summary>
    /// 购买物品的按钮事件
    /// </summary>
    private void buyItem()
    {

        if (Playerattribute.getInstance().GoldCoin < int.Parse(m_goldText.text))
        {
            return;
        }
        if (m_playSlotQueue.Count != 0)
        {
            foreach (var item in m_playSlotQueue)
            {
                item.GetComponent<playerSlot>().setplayerSlot(-1, null);
                Destroy(item.transform.GetChild(0).gameObject);

            }
            m_playSlotQueue.Clear();
        }
        if (PlayerSlotManager.instace.getNullSolt() == null)
        {
            return;
        }
       
        GameObject temp = Instantiate(m_itemImage).transform.gameObject;
        temp.transform.parent = PlayerSlotManager.instace.getNullSolt().transform;
        temp.transform.localPosition = Vector3.zero;
        temp.transform.localScale = Vector3.one;
        //CreatPlayerSlot.instace.getNullSolt().GetComponent<playerSlot>().isNull = 0;
        //CreatPlayerSlot.instace.getNullSolt().GetComponent<playerSlot>().item=item;
        PlayerSlotManager.instace.getNullSolt().GetComponent<playerSlot>().setplayerSlot(0, m_item);
        
        Playerattribute.getInstance().GoldCoin -= int.Parse(m_goldText.text);
        hideTool();
    }
    /// <summary>
    /// 操作按钮的事件
    /// </summary>
    private void Caozuo()
    {
        //点击预购按钮执行的事件
        if (m_caozuo.GetComponentInChildren<Text>().text == "预购")
        {
            InventroyManager.instance.setPrepurchase(m_fatherItem);
            itemObjectpoolManager.instance.setCompositechildslotpool();
            itemObjectpoolManager.instance.setLinepool();
            hideTool();
        }
        //点击出售按钮执行的事件
        else
        {
            Playerattribute.getInstance().GoldCoin += m_item.SellPrice;
            m_sellItem.GetComponent<playerSlot>().isNull = -1;
            Destroy(m_sellItem.transform.GetChild(0).gameObject);
            hideTool();
        }
    }
    /// <summary>
    /// 点击合成路径按钮执行的事件
    /// </summary>
    private void btnTool()
    {

        switch (toolis)
        {
            case ToolisOpen.close:
                //DrawLine.instance.startDraw();
                toolis = ToolisOpen.open;
                m_ItemInventroy.localScale = Vector3.zero;
                m_Compositepanel.localScale = Vector3.one;
                m_btnTool.gameObject.GetComponent<Image>().sprite = m_closeSprite;
                break;
            case ToolisOpen.open:
                //DrawLine.instance.stopDraw();
                toolis = ToolisOpen.close;
                m_ItemInventroy.localScale = Vector3.one;
                m_ItemInventroy.GetComponent<ScrollRect>().content.localPosition = new Vector2(m_ItemInventroy.GetComponent<ScrollRect>().content.localPosition.x, 0);
                m_Compositepanel.localScale = Vector3.zero;
                m_btnTool.gameObject.GetComponent<Image>().sprite = m_openSprite;
                break;
            default:
                break;
        }
    }
    #endregion
    #region 处理物品说明
    /// <summary>
    /// 分割物品说名的字符串
    /// </summary>
    /// <param name="_string">物品说名字符串</param>
    /// <returns>物品说名</returns>
    private string Processingstrings(string _string)
    {

        m_strarr = null;
        m_tempstring = null;
        if (_string.Contains("|"))
        {
            Debug.Log("包含");
            m_tempstring = _string.Split('|');
            m_tempstring[0] = m_tempstring[0] + "\n";
            m_tempstring[1] = "\n" + m_tempstring[1];
            m_strarr = m_tempstring[0].Split('。');
            _string = null;
            _string = MethCyclicalgorithmod(_string);
            m_strarr = m_tempstring[1].Split('《');
            _string = MethCyclicalgorithmod(_string);
        }
        else
        {
            //_tempstring = _string; 
            m_strarr = _string.Split('。');
            _string = null;
            _string = MethCyclicalgorithmod(_string);
        }

        
        Debug.Log(_string);
        return _string;
    }
    /// <summary>
    /// 合并字符串
    /// </summary>
    /// <param name="_string">物品说名</param>
    /// <returns></returns>
    private string MethCyclicalgorithmod(string _string)
    {
        for (int i = 0; i < m_strarr.Length - 1; i++)
        {
            m_strarr[i] = m_strarr[i] + "\n";
        }
        for (int i = 0; i < m_strarr.Length; i++)
        {
            _string += m_strarr[i];
        }
        Debug.Log(_string + "645646846546");
        return _string;
    }
    #endregion
    #region 显示合成树
    /// <summary>
    /// 显示合成路径
    /// </summary>
    /// <param name="_items">选中的物品</param>
    public void showCompositePath(items _items, Slot.itemOwn _itemOwn)
    {
        //把之前的物品存入对象池
        itemObjectpoolManager.instance.setCompositechildslotpool();
        //把合成连接线存入对象池
        itemObjectpoolManager.instance.setLinepool();
        m_fatherItem = new itemLeaf();
        Debug.Log("实例化了");
        m_fatherItem.FatherItem = _items;
        //fatherGameObject.transform.Find("Image").GetComponentInChildren<Image>().sprite = _itemImage.sprite;
        //fixstring(_items);
        if (m_fatherGameObject.GetComponent<CompositeChildslot>()==null)
        {
            m_fatherGameObject.AddComponent<CompositeChildslot>();
        }
        m_fatherGameObject.GetComponent<CompositeChildslot>().setItem(_items,m_fatherItem,CompositeChildslot.cenCI.One);
        if (_itemOwn == Slot.itemOwn.None)
        {
            m_goldText.text = showGoldText();
        }
        else
        {
            m_goldText.text = m_item.SellPrice.ToString();
        }


    }
    #endregion

    
}
