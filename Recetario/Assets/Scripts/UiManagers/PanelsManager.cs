using System;
using UnityEngine;

public class PanelsManager : MonoBehaviour
{
    public Panel[] panels;

    private void Start()
    {
        for (int i = 0; i < panels.Length; i++)
        {
            if (panels[i].panel == null)
            {
                Debug.LogWarning($"PanelsManager: reference at {panels[i].name} is empty");
            }
            for (int j = 0; j < panels[i].subPanels.Length; j++)
            {
                if (panels[i].subPanels[j].subPanel == null)
                {
                    Debug.LogWarning($"PanelsManager: reference at {panels[i].name}/{panels[i].subPanels[j].name} is empty");
                }
            }
        }
    }

    public void TurnOnPanel(string panelName)
    {
        if (!string.IsNullOrEmpty(panelName))
        {
            if (panels.Length > 0)
            {
                for (int i = 0; i < panels.Length; i++)
                {
                    if (panels[i].name.Equals(panelName))
                    {
                        if (panels[i].panel != null)
                           panels[i].panel.SetActive(true);
                        else
                            Debug.LogWarning($"PanelsManager: Can't activate {panels[i].name} because reference is empty");
                        if (!string.IsNullOrEmpty(panels[i].defaultSubPanel))
                            TurnOnSubPanel(panelName + "," + panels[i].defaultSubPanel);
                    }
                    else
                        panels[i].panel.SetActive(false);
                }
            }
        }
        else
        {
            Debug.Log("panelName is empty");
        }
    }

    public void TurnOnSubPanel(string panelNameAndSubPanel)
    {
        string[] NameAndSubPanel = panelNameAndSubPanel.Split(',');
        string panelName = "";
        string subPanelName = "";

        if (!string.IsNullOrEmpty(NameAndSubPanel[0]))
        {
            panelName = NameAndSubPanel[0];
        }
        else
        {
            Debug.Log("panelName is empty");
            return;
        }

        if (!string.IsNullOrEmpty(NameAndSubPanel[1]))
        {
            subPanelName = NameAndSubPanel[1];
        }
        else
        {
            Debug.Log("subPanelName is empty");
            return;
        }

        if (panels.Length > 0)
        {
            for (int i = 0; i < panels.Length; i++)
            {
                if (panels[i].name.Equals(panelName))
                {
                    for (int j = 0; j < panels[i].subPanels.Length; j++)
                    {
                        if (panels[i].subPanels[j].name.Equals(subPanelName))
                        {
                            if (panels[i].subPanels[j].subPanel != null)
                                panels[i].subPanels[j].subPanel.SetActive(true);
                            else
                                Debug.LogWarning($"PanelsManager: Can't activate {panels[i].name}/{panels[i].subPanels[j].name} because reference is empty");
                        }
                        else
                        {
                            if (panels[i].subPanels[j].subPanel != null)
                                panels[i].subPanels[j].subPanel.SetActive(false);
                            else
                                Debug.LogWarning($"PanelsManager: {panels[i].name}/{panels[i].subPanels[j].name} reference is empty");
                        }
                    }
                }
            }
        }
    }
}

[Serializable]
public class Panel
{
    public string name;
    public string defaultSubPanel;
    public GameObject panel;
    public SubPanel[] subPanels;
}

[Serializable]
public class SubPanel
{
    public string name;
    public GameObject subPanel;
}