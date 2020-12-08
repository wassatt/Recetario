using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShoppingListContentManager : MonoBehaviour
{
    public Transform contentObj;
    public GameObject pfb_grp_ingredient;
    
    void Start()
    {
        foreach (Transform child in contentObj.transform)
        {
            Destroy(child.gameObject);
        }        

       for (int i = 0; i < 5; i++)
       {
           GameObject obj = Instantiate(pfb_grp_ingredient, contentObj.transform);
           obj.transform.Find("btn_remove").GetComponent<Button>().onClick.AddListener(delegate {Destroy(obj);});
       }
    }
}
