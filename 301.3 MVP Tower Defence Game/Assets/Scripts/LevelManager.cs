using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public static LevelManager Main;

    public Transform startPoint;
    public Transform[] path;

    private void Awake() {
        Main = this;
    }
}
