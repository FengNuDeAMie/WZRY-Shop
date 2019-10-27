using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playerattribute 
{
    private static Playerattribute instance;
    public static Playerattribute getInstance() {
        if (instance==null)
        {
            instance = new Playerattribute();
        }
        return instance;
    }
    private int _Goldcoin;

    public int GoldCoin {
        get{
            return _Goldcoin;
        }
        set { _Goldcoin = value; }
        
            }
}
