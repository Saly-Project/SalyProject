using UnityEngine;

public class GeneralScripts : MonoBehaviour
{
    [SerializeField] public GameObject QuitMenu;
    [SerializeField] public GameObject MainMenu;
    
    void Start()
    {
        QuitMenu.SetActive(false);
    }
    public void QuitFunc()
    {
        QuitMenu.SetActive(true);
        MainMenu.SetActive(false);
    }

    public void DefoQuit()
    {
        Debug.Log("Quitting man..");
        Application.Quit();
    }
    public void UnQuit()
    {
        QuitMenu.SetActive(false);
        MainMenu.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
