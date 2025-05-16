using UnityEngine;
using UnityEngine.UI;

public class ShipSelection : MonoBehaviour
{
    [SerializeField] private Button previousButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private Text spaceshipName;

    private int currentShip;

    private void Awake()
    {
        SelectShip(0);
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
}
