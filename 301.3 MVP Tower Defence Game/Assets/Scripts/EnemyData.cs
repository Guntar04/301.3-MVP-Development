using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemy", menuName = "Game Data/Enemy")]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    [TextArea] public string strength;
    [TextArea] public string weakness;
}
