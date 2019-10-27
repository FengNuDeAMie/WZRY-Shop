using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class itemObjectpoolManager : MonoBehaviour
{
    private GameObject[] _slot;
    private GameObject[] _item;
    private Queue<GameObject> itemQueue = new Queue<GameObject>();
    private Queue<GameObject> slotQueue = new Queue<GameObject>();
    private GameObject[] _Compositeslot;
    private GameObject[] _Compositechildslot;
    private Queue<GameObject> compositeslotQueue = new Queue<GameObject>();
    private Queue<GameObject> compositechildslotQueue = new Queue<GameObject>();
    private GameObject[] _Verticalline;
    private Queue<GameObject> VerticallineQueue = new Queue<GameObject>();
    private GameObject[] _longHorizontalLine;
    private Queue<GameObject> longHorizontalLineQueue = new Queue<GameObject>();
    private GameObject[] _shortHorizontalLine;
    private Queue<GameObject> shortHorizontalLineQueue = new Queue<GameObject>();
    public static itemObjectpoolManager instance;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    public void setObjecctpool() {
        _item = GameObject.FindGameObjectsWithTag("item");
        _slot = GameObject.FindGameObjectsWithTag("slot");
        for (int i = 0; i < _item.Length; i++)
        {
            itemQueue.Enqueue(_item[i]);
            _item[i].transform.parent = this.transform;
            _item[i].SetActive(false);
        }
        for (int i = 0; i <_slot.Length; i++)
        {
            
            slotQueue.Enqueue(_slot[i]);
            _slot[i].transform.parent = this.transform;
            _slot[i].transform.localScale = Vector3.one;
            _slot[i].SetActive(false);
        }
    }
    public int getitemQueue() {
        return itemQueue.Count;
    }
    public int getslotQueue()
    {
        return slotQueue.Count;
    }
    public GameObject getItem() {
        return itemQueue.Dequeue();
    }
    public GameObject getslot()
    {
        return slotQueue.Dequeue();
    }
    public void setCompositeslotpool() {
        _Compositeslot = GameObject.FindGameObjectsWithTag("Compositeslot");
        for (int i = 0; i < _Compositeslot.Length; i++)
        {
            compositeslotQueue.Enqueue(_Compositeslot[i]);
            _Compositeslot[i].transform.parent = this.transform;
            _Compositeslot[i].SetActive(false);
        }
    }
    public int getcompositeslotQueue() {
        return compositeslotQueue.Count;
    }
    public GameObject getCompositeslot() {
        return compositeslotQueue.Dequeue();
    }
    public void setCompositechildslotpool() {
        _Compositechildslot = GameObject.FindGameObjectsWithTag("CompositeChildslot");
        for (int i = 0; i < _Compositechildslot.Length; i++)
        {
            compositechildslotQueue.Enqueue(_Compositechildslot[i]);
            _Compositechildslot[i].transform.parent = this.transform;
            _Compositechildslot[i].SetActive(false);
        }
        
    }
    public int getcompositechildslotQueue() {
        return compositechildslotQueue.Count;
    }
    public GameObject getcompositechildslot() {
        return compositechildslotQueue.Dequeue();
    }
    public void setLinepool()
    {
        _longHorizontalLine = GameObject.FindGameObjectsWithTag("longHorizontalLine");
        _shortHorizontalLine = GameObject.FindGameObjectsWithTag("shortHorizontalLine");
        _Verticalline = GameObject.FindGameObjectsWithTag("Verticalline");
        for (int i = 0; i < _shortHorizontalLine.Length; i++)
        {
            shortHorizontalLineQueue.Enqueue(_shortHorizontalLine[i]);
            _shortHorizontalLine[i].transform.parent = this.transform;
            _shortHorizontalLine[i].SetActive(false);
        }
        for (int i = 0; i <_longHorizontalLine.Length; i++)
        {
            longHorizontalLineQueue.Enqueue(_longHorizontalLine[i]);
            _longHorizontalLine[i].transform.parent = this.transform;
            _longHorizontalLine[i].SetActive(false);
        }
        for (int i = 0; i < _Verticalline.Length; i++)
        {
            VerticallineQueue.Enqueue(_Verticalline[i]);
            _Verticalline[i].transform.parent = this.transform;
            _Verticalline[i].SetActive(false);
        }
    }
    public int getVerticallineQueue()
    {
        return VerticallineQueue.Count;
    }
    public GameObject getVerticalline()
    {
        return VerticallineQueue.Dequeue();
    }
    public int getlongHorizontalLineQueue() {
        return longHorizontalLineQueue.Count;
    }
    public GameObject getlongHorizontalLine() {
        return longHorizontalLineQueue.Dequeue();
    }
    public int getshortHorizontalLineQueue() {
        return shortHorizontalLineQueue.Count;
    }
    public GameObject getshortHorizontalLine() {
        return shortHorizontalLineQueue.Dequeue();
    }
}
