using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] internal Player player;
    [SerializeField] internal Effects effects;
    [SerializeField] internal UIManager uiManager;
    [SerializeField] internal CollectableData collectableData;
}