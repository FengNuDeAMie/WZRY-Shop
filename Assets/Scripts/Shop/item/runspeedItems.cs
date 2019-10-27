using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class runspeedItems : items
{
    private int _runSpeed;
    private int _attackSpeed;
    private int _magicDefense;
    private int _physicalPenetration;
    private int _cd;
    private int _upMp;
    public runspeedItems(int id, string name, int buyPrice, int sellPrice, string sprite, ItemType itemType, ItemQuality itemQuality, ItemjiNeng itemjiNeng, string synopis,string explain, string compounds, string compositepath, int runspeed,
        int attackspeed, int magicdefense,int physicalpentration,int cd,int upmp) : base(id, name, buyPrice, sellPrice, sprite, itemType, itemQuality, itemjiNeng,synopis,explain,compounds,compositepath)
    {
        this.RunSpeed = runspeed;
        this.AttackSpeed = attackspeed;
        this.MagicDefense = magicdefense;
        this.PhysicalPenetration = physicalpentration;
        this.Cd = cd;
        this.UpMp = upmp;
    }

    public int RunSpeed { get => _runSpeed; set => _runSpeed = value; }
    public int AttackSpeed { get => _attackSpeed; set => _attackSpeed = value; }
    public int PhysicalPenetration { get => _physicalPenetration; set => _physicalPenetration = value; }
    public int MagicDefense { get => _magicDefense; set => _magicDefense = value; }
    public int Cd { get => _cd; set => _cd = value; }
    public int UpMp { get => _upMp; set => _upMp = value; }
}
