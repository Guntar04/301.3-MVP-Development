using UnityEngine;

public class StarSystem : MonoBehaviour
{
    public static StarSystem Main;

    public int gainedStars = 0;

    private void Awake()
    {
        Main = this;
    }

    public int CalculateStars(int hp)
    {
        if (hp >= 8)
        {
            gainedStars = 3;
            return 3;
        }
        if (hp >= 5)
        {
            gainedStars = 2;
            return 2;
        }
        if (hp >= 1)
        {
            gainedStars = 1;
            return 1;
        }
        return 0;
    }

}
