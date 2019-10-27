using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackItems : items
{
    private int _aggressivity;
    private int _attackSpeed;
    private int _bloodsucking;
    private int _criticalchance;
    private int _MaxHp;
    private int _runSpeed;
    private int _cd;
    private int _magicDefense;
    private int _physicalPenetration;
    public attackItems(int id,string name, int buyPrice, int sellPrice, string sprite, ItemType itemType, ItemQuality itemQuality, ItemjiNeng itemjiNeng,string synopis, string explain,string compounds, string compositepath, int aggressivity,int attackspeed,int bloodsucking,
        int criticalchance,int maxhp,int runspeed,int cd,int magicdefense,int physicalpenetration) : base(id,name, buyPrice, sellPrice, sprite, itemType, itemQuality, itemjiNeng,synopis,explain,compounds,compositepath)
    {
        this.Aggressivity = aggressivity;
        this.AttackSpeed = attackspeed;
        this.Bloodsucking = bloodsucking;
        this.Criticalchance = criticalchance;
        this.MaxHp = maxhp;
        this.RunSpeed = runspeed;
        this.Cd = cd;
        this.MagicDefense = magicdefense;
        this.PhysicalPenetration = physicalpenetration;
    }

    public int Aggressivity { get => _aggressivity; set => _aggressivity = value; }
    public int AttackSpeed { get => _attackSpeed; set => _attackSpeed = value; }
    public int Bloodsucking { get => _bloodsucking; set => _bloodsucking = value; }
    public int MaxHp { get => _MaxHp; set => _MaxHp = value; }
    public int RunSpeed { get => _runSpeed; set => _runSpeed = value; }
    public int Cd { get => _cd; set => _cd = value; }
    public int MagicDefense { get => _magicDefense; set => _magicDefense = value; }
    public int PhysicalPenetration { get => _physicalPenetration; set => _physicalPenetration = value; }
    public int Criticalchance { get => _criticalchance; set => _criticalchance = value; }
}
