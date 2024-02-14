using UnityEngine;

public class CollectableData : MonoBehaviour
{
    private UIManager uiManager;
    private int gems;

    private void Start()
    {
        uiManager = GameManager.instance.uiManager;
    }

    internal void UpdateGems(int amount)
    {
        gems += amount;
        uiManager.UpdateTxt(gems);
    }
}