using UnityEngine;

public class StarSystem : MonoBehaviour
{
    public static StarSystem Main;

    private void Awake()
    {
        Main = this;
    }

    public int CalculateStars(int hp)
    {
        if (hp >= 8) return 3;
        if (hp >= 5) return 2;
        if (hp >= 1) return 1;
        return 0;
    }

}
