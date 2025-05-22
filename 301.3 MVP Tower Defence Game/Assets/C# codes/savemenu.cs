using UnityEngine;

public class savemenu : MonoBehaviour
{
    public GameObject savemenuPanel;

    void Start()
    {
        savemenuPanel.SetActive(false);
    }

    public void OpenSaveMenu()
    {   
        savemenuPanel.SetActive(true);
    }

    public void SaveGame()
    { 
     Debug.Log("Game Saved!");
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        
        { savemenuPanel.SetActive(!savemenuPanel.activeSelf); }
    }
}
