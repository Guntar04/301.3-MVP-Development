using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class GameSaveUI : MonoBehaviour
{
    public GameObject popupObject;
    public float displayDuration = 2f;

    private void Start()
    {
       if(popupObject != null)
       
           popupObject.SetActive(false);      
    }

    public void ShowSavePopup()
    {
        if (popupObject != null)
        {
            popupObject.SetActive(true);
            CancelInvoke(nameof(HidePopup));
            Invoke(nameof(HidePopup), displayDuration);
        }

    }
       private void HidePopup()
       { 
           popupObject.SetActive(false);

    }

    }



