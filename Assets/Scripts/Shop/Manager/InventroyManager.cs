using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class InventroyManager : MonoBehaviour
{


    public static InventroyManager instance;
    private Dictionary<string, List<items>> itemDic = new Dictionary<string, List<items>>();
    private List<items> attackitem = new List<items>();
    private List<items> runspeeditem = new List<items>();
    private List<items> jungleitem = new List<items>();
    private List<items> supitem = new List<items>();
    private List<items> miditem = new List<items>();
    private List<items> topitem = new List<items>();
    private List<items> prepurchase = new List<items>();
    private List<itemLeaf> fatherItemLeaf;
    //保存子物品结点的列表
    private List<itemLeaf> childItemLeaf;
    private List<items> tempList;
    private Button _OpenButton;
    private Button _CloseButton;
    private GameObject _ShopUI;
    private GameObject _itemTool;
    private items tempitem;
    private JSONObject j;
    //private GameObject _itemTool;
    int _runSpeed;
    int _attackSpeed;
    int _magicDefense;
    int _cd;
    int _physicalPenetration;
    string _synopis;
    string name;
    string sprite;
    string explain;
    int temp;
    string compounds;
    string compositepath;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }


    }

    private void ParseItemJson()
    {

        //文本在unity里面是TextAsset类型
        TextAsset itemText = Resources.Load<TextAsset>("item");//加载json文本

        string itemJson = itemText.text;//得到json文本里面的内容
        j = new JSONObject(itemJson);
        foreach (var item in j.list)
        {
            temp++;
            int id = (int)(item["id"].n);
            name = item["name"].str;
            int buyPrice = (int)(item["buyPrice"].n);
            int sellPrice = (int)(item["sellPrice"].n);
            sprite = item["sprite"].str;
            explain = item["Explain"].str;
            items.ItemQuality quality = (items.ItemQuality)System.Enum.Parse(typeof(items.ItemQuality), item["quality"].str);

            items.ItemType type = (items.ItemType)System.Enum.Parse(typeof(items.ItemType), item["type"].str);

            items.ItemjiNeng itemjiNeng = (items.ItemjiNeng)System.Enum.Parse(typeof(items.ItemjiNeng), item["itemjiNeng"].str);
            _synopis = item["synopsis"].str;
            compounds = item["Compounds"].str;
            compositepath = item["CompositePath"].str;
            switch (type)
            {
                case items.ItemType.gongJi:

                    int _aggressivity = (int)(item["aggressivity"].n);
                    _attackSpeed = (int)(item["attackspeed"].n);
                    int _bloodsucking = (int)(item["bloodsucking"].n);
                    int _criticalchance = (int)(item["criticalchance"].n);
                    int _MaxHp = (int)(item["maxhp"].n);
                    _runSpeed = (int)(item["RunSpeed"].n);
                    _cd = (int)(item["cd"].n);
                    _magicDefense = (int)(item["magicDefense"].n);
                    _physicalPenetration = (int)(item["physicalpenetration"].n);

                    tempitem = new attackItems(id, name, buyPrice, sellPrice, sprite, type, quality, itemjiNeng, _synopis, explain, compounds, compositepath, _aggressivity, _attackSpeed, _bloodsucking,
                        _criticalchance, _MaxHp, _runSpeed, _cd, _magicDefense, _physicalPenetration);
                    attackitem.Add(tempitem);
                    break;
                case items.ItemType.faShu:
                    tempitem = new midItems(id, name, buyPrice, sellPrice, sprite, type, quality, itemjiNeng, _synopis, explain, compounds, compositepath);
                    miditem.Add(tempitem);
                    break;
                case items.ItemType.fangYu:
                    tempitem = new topItems(id, name, buyPrice, sellPrice, sprite, type, quality, itemjiNeng, _synopis, explain, compounds, compositepath);
                    topitem.Add(tempitem);
                    break;
                case items.ItemType.yiDong:
                    _runSpeed = (int)(item["RunSpeed"].n);
                    _attackSpeed = (int)(item["attackspeed"].n);
                    _magicDefense = (int)(item["magicDefense"].n);
                    _physicalPenetration = (int)(item["physicalpenetration"].n);
                    _cd = (int)(item["cd"].n);
                    int _upMp = (int)(item["upmp"].n);
                    tempitem = new runspeedItems(id, name, buyPrice, sellPrice, sprite, type, quality, itemjiNeng, _synopis, explain, compounds, compositepath, _runSpeed, _attackSpeed, _magicDefense, _physicalPenetration, _cd, _upMp);
                    runspeeditem.Add(tempitem);
                    break;
                case items.ItemType.daYe:
                    tempitem = new jungleItems(id, name, buyPrice, sellPrice, sprite, type, quality, itemjiNeng, _synopis, explain, compounds, compositepath);
                    jungleitem.Add(tempitem);
                    break;
                case items.ItemType.fuZhu:
                    tempitem = new supItems(id, name, buyPrice, sellPrice, sprite, type, quality, itemjiNeng, _synopis, explain, compounds, compositepath);
                    supitem.Add(tempitem);
                    break;
                default:
                    break;
            }

        }
        itemDic.Add("攻击", attackitem);
        itemDic.Add("移动", runspeeditem);
        itemDic.Add("打野", jungleitem);
        itemDic.Add("辅助", supitem);
        itemDic.Add("防御", topitem);
        itemDic.Add("法术", miditem);




    }

    // Start is called before the first frame update
    void Start()
    {
        _CloseButton = GameObject.FindGameObjectWithTag("CloseButton").GetComponent<Button>();
        _ShopUI = GameObject.FindGameObjectWithTag("ShopUI");
        _ShopUI.SetActive(false);
        _OpenButton = GameObject.FindGameObjectWithTag("OpenShop").GetComponent<Button>();
        _OpenButton.onClick.AddListener(Open);
        _CloseButton.onClick.AddListener(CloseUI);
        ParseItemJson();
    }

    private void CloseUI()
    {
        ItemToolTipManager.instance.hideTool();
        _ShopUI.SetActive(false);
    }

    // Update is called once per frame

    public List<items> Getitems(string _text)
    {
        foreach (var item in itemDic)
        {

            if (item.Key == _text)
            {
                return item.Value;
            }

        }
        return null;


    }
    void Open()
    {

        _ShopUI.SetActive(true);
    }
    public Dictionary<string, List<items>> getItemDic()
    {
        return itemDic;
    }
    bool isFound = false;
    public void setPrepurchase(itemLeaf _itemLeaf)
    {
        tempList = new List<items>();
        isFound = false;
        tempList.Add(_itemLeaf.FatherItem);
      
        if (_itemLeaf.getList().Count != 0)
        {
            fatherItemLeaf = _itemLeaf.getList();
            for (int j = 0; j < fatherItemLeaf.Count; j++)
            {
                tempList.Add(fatherItemLeaf[j].FatherItem);
                if (fatherItemLeaf[j].getList().Count != 0)
                {
                    childItemLeaf = fatherItemLeaf[j].getList();
                    for (int n = 0; n < childItemLeaf.Count; n++)
                    {
                        tempList.Add(childItemLeaf[n].FatherItem);
                    }
                }
            }
        }
        childItemLeaf = null;
        for (int i = 0; i < tempList.Count; i++)
        {
            isFound = false;
            if (prepurchase.Count==0)
            {
                prepurchase.Add(tempList[i]);
                continue;
            }
            for (int j = 0; j < prepurchase.Count; j++)
            {
                if (isFound)
                {
                    continue;
                }
                if (prepurchase[j].Id==tempList[i].Id)
                {
                    isFound = true;
                    continue;
                }
                
            }
            if (!isFound)
            {
                prepurchase.Add(tempList[i]);
            }
        }
        if (itemDic.ContainsKey("推荐"))
        {
            itemDic["推荐"] = prepurchase;
        }
        else
        {
            itemDic.Add("推荐", prepurchase);
        }
       
    }

   
}