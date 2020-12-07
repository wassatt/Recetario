using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AppManager : MonoBehaviour
{
    [SerializeField]
    private ScriptableBool isAlle;

    public UnityEvent onLoadAllesMode;
    public UnityEvent onLoadGuestMode;

    // Start is called before the first frame update
    void Start()
    {
        InitApp();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InitApp()
    {

        if (isAlle.Get())
        {

            onLoadAllesMode.Invoke();
        }
        else
        {

            onLoadGuestMode.Invoke();
        }
    }

}
