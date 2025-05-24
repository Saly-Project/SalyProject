using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ShipSelection : MonoBehaviour
{
    public GameObject[] ShipsPrefabs;
    private int currentShip;

    private void Awake()
    {
        SelectShip(0);
        PlayerPrefs.SetString("spaceship", transform.GetChild(currentShip).name);
    }

    private void SelectShip(int _index)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(i == _index);
        }
    }

    public void ChangeShip(int _change) 
    {
        currentShip = (transform.childCount + currentShip + _change) % transform.childCount;
        SelectShip(currentShip);
    }

    public void SelectButton()
    {
        PlayerPrefs.SetString("spaceship", transform.GetChild(currentShip).name);
        NetworkManager.Singleton.NetworkConfig.PlayerPrefab = GetPrefabByName(transform.GetChild(currentShip).name);
    }

    public GameObject GetPrefabByName(string name)
    {
        foreach (GameObject prefab in ShipsPrefabs) 
        { 
            if (prefab.name == name) return prefab;
        }

        return null;
    }
}
