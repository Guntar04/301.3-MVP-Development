using UnityEngine;

[CreateAssetMenu(fileName = "NewTower", menuName = "Game Data/Tower")]
public class TowerData : ScriptableObject
{
    public string towerName;
    [TextArea] public string strength;
    [TextArea] public string weakness;
}
