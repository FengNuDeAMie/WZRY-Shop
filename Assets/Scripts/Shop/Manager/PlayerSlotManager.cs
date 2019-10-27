using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSlotManager : MonoBehaviour
{
    #region 成员变量
    //单例字段
    public static PlayerSlotManager instace;
    //临时物品格
    private GameObject m_tempSlot;
    //物品格子
    private GameObject m_slot;
    //玩家背包物品列表
    private List<GameObject> m_playerSlot = new List<GameObject>();
    #endregion
    private void Awake()
    {
        instace = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        //加载玩家背包格子
        m_tempSlot = Resources.Load("prefab/ShopPrefab/Slot/playerslot") as GameObject;
        //添加玩家背包格子
        for (int i = 0; i < 6; i++)
        {
            m_slot = Instantiate(m_tempSlot);
            m_playerSlot.Add(m_slot);
            m_slot.transform.parent = this.transform;
            m_slot.transform.localPosition = Vector3.one;
            m_slot.transform.localScale = this.transform.localScale;
            m_slot.AddComponent<playerSlot>();
        }
    }
    /// <summary>
    /// 获取玩家背包空格子的方法
    /// </summary>
    /// <returns>返回一个空格子</returns>
    public GameObject getNullSolt() {
        for (int i = 0; i < m_playerSlot.Count; i++)
        {
            if (m_playerSlot[i].GetComponent<playerSlot>().isNull==-1)
            {
                return m_playerSlot[i];
                
            }
        }
        return null;
    }
    /// <summary>
    /// 获取玩家背包物品列表
    /// </summary>
    /// <returns>返回玩家背包物品列表</returns>
    public List<GameObject> getList() {
        return m_playerSlot;
    }
}
