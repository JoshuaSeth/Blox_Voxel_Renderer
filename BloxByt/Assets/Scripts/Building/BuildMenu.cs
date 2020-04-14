using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildMenu : MonoBehaviour
{
    public List<List<Item>> buildableParts = new List<List<Item>>();
    Item[] buildItems;
    List<Cube.Type> types = new List<Cube.Type>();
    public Transform buttonsHolder;
    public GameObject x60ButtonPrfb;

    // Start is called before the first frame update
    void Start()
    {
        buildItems = Resources.LoadAll<Item>("Items/Buildings/");

        foreach (Cube.Type type in System.Enum.GetValues(typeof(Cube.Type)))
        {
            types.Add(type);
        }

        LoadMainButtonMenu();
    }

    public void LoadMainButtonMenu()
    {
        DestroyButtons();
        foreach (Item buildItem in buildItems)
        {
            GameObject button = GameObject.Instantiate(x60ButtonPrfb, buttonsHolder) as GameObject;
            button.GetComponentInChildren<Text>().text = buildItem.name;
            button.GetComponent<Button>().onClick.AddListener(delegate { OpenFolder(buildItem); });

        }
    }

    public void OpenFolder(Item folderItem)
    {
        DestroyButtons();
        foreach (Cube.Type type in folderItem.posibleForms.variations)
        {
            GameObject button = GameObject.Instantiate(x60ButtonPrfb, buttonsHolder) as GameObject;
            button.GetComponentInChildren<Text>().text = type.ToString();
            button.GetComponent<Button>().onClick.AddListener(delegate { BuildActions.active.MakeBuildPart(folderItem, type); });
        }
    }

    public void DestroyButtons()
    {
        List<Transform> deletes = new List<Transform>();
        for (int i = 1; i<buttonsHolder.childCount;i++)
        {
            deletes.Add(buttonsHolder.GetChild(i));
        }

        foreach (Transform del in deletes)
            GameObject.Destroy(del.gameObject);
    }
}
