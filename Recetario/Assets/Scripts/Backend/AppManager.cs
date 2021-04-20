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
        InitApp();
    }

    public void InitApp()
    {
        StartCoroutine(dbManager.endpointsTools.GetWithParam(API.urlIsAdmin, AuthManager.currentUserMail, "", callback => {
            isAlle = bool.Parse(callback);

            if (isAlle)
            {

                onLoadAllesMode.Invoke();
            }
            else
            {

                onLoadGuestMode.Invoke();
            }
        }));
    }

}
