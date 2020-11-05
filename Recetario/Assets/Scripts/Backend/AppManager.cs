using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AppManager : MonoBehaviour
{
    [SerializeField]
    private ScriptableBool isRestaurant;

    public UnityEvent onSetRestaurantHome;
    public UnityEvent onSetCustomerHome;

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

        if (isRestaurant.Get())
        {

            onSetRestaurantHome.Invoke();
        }
        else
        {

            onSetCustomerHome.Invoke();
        }
    }

}
