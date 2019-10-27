using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class items
{
    private int _id;
    private string _name;
    private int _buyPrice;
    private int _sellPrice;
    private string _Sprite;
    private string _Synopsis;
    private string _Explain;
    private string _Compounds;
    private string _CompositePath;
    public string Name { get => _name; set => _name = value; }
    public int BuyPrice { get => _buyPrice; set => _buyPrice = value; }
    public int SellPrice { get => _sellPrice; set => _sellPrice = value; }
    public string Sprite { get => _Sprite; set => _Sprite = value; }
    public ItemType _ItemType { get; set; }
    public ItemQuality _ItemQuality { get; set; }
    public ItemjiNeng _ItemjiNeng { get; set; }
    public int Id { get => _id; set => _id = value; }
    public string Synopsis { get => _Synopsis; set => _Synopsis = value; }
    public string Explain { get => _Explain; set => _Explain = value; }
    public string Compounds { get => _Compounds; set => _Compounds = value; }
    public string CompositePath { get => _CompositePath; set => _CompositePath = value; }

    public enum ItemType
    {
        gongJi,
        faShu,
        fangYu,
        yiDong,
        daYe,
        fuZhu
    }
    public enum ItemQuality
    {
        chuJi,
        zhongJi,
        gaoJi,

    }
    public enum ItemjiNeng
    {
        weiyi,
        None
    }
    public items(int id, string name, int buyPrice, int sellPrice, string sprite, ItemType itemType, ItemQuality itemQuality, ItemjiNeng itemjiNeng, string synopis,string explain,string compounds,string compositepath)
    {
        this.Id = id;
        this.Name = name;
        this.BuyPrice = buyPrice;
        this.SellPrice = sellPrice;
        this.Sprite = sprite;
        this._ItemType = itemType;
        this._ItemQuality = itemQuality;
        this._ItemjiNeng = itemjiNeng;
        this.Synopsis = synopis;
        this.Explain = explain;
        this.Compounds = compounds;
        this.CompositePath = compositepath;
    }


}
