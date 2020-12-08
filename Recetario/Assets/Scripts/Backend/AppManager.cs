using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AppManager : MonoBehaviour
{
    [SerializeField]
    private DataBaseManager dbManager;
    [SerializeField]
    private bool isAlle;

    public UnityEvent onLoadAllesMode;
    public UnityEvent onLoadGuestMode;
    
    void Start()
    {
        //TODO: endpoint isAlle;
        InitApp();
    }

    public void InitApp()
    {

        if (isAlle)
        {

            onLoadAllesMode.Invoke();
        }
        else
        {

            onLoadGuestMode.Invoke();
        }
    }

}
