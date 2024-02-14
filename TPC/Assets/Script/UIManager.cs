using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] TMP_Text txtGems;

    internal void UpdateTxt(int amount)
    {
        txtGems.text = "Gems: " + amount.ToString();
    }
}