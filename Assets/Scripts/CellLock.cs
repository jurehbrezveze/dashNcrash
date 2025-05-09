using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CellLock : MonoBehaviour
{
    private int maxLevel;
    private int levelNum;

    void Start()
    {
        maxLevel = PlayerPrefs.GetInt("LastLevel", 1);
        int.TryParse(gameObject.name, out levelNum);
        Transform child = transform.Find("Lock");
        if (child != null && levelNum > maxLevel)
        {
            child.gameObject.SetActive(true);
        }
    }
}
