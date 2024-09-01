using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using TMPro;

public class Profile : MonoBehaviour
{
    public TMP_Text Player;

    // Start is called before the first frame update
    void Start()
    {
        if (Player != null){
            Player.text = GameManager.Instance.Username;
        }
    }

    
}
