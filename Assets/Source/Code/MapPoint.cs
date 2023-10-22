using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPoint : MonoBehaviour
{
    public int level = 1;
    public int maxLevel = 4;
    public int id = 0;
    public bool isBlock = false;

    public void SetData(int id,List<Sprite> listChild)
    {
        this.id = id;

        for (int i = 1; i < 5; i++)
        {
            transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = listChild[i - 1];
            transform.GetChild(i).gameObject.SetActive(false);
        }
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(true);
    }
    public void PickUp()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }
    
    public void CancelPickUp()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void LevelUp(bool check)
    {
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(level).gameObject.SetActive(false);
        level++;
        transform.GetChild(level).gameObject.SetActive(true);
        if(check)GameManager.Instance.CheckLevelUp();
    }
}
