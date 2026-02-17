using UnityEngine;

public class GardenItem : MonoBehaviour
{
    private WeedHuntMiniGame miniGame;
    private bool isWeed;

    public void Init(WeedHuntMiniGame game, bool weed)
    {
        miniGame = game;
        isWeed = weed;
    }

    void OnMouseDown()
    {
        Debug.Log(gameObject.name + " clicked. Is weed? " + isWeed);

        if (miniGame == null) return;

        if (isWeed)
        {
            miniGame.WeedClicked();
            Destroy(gameObject);
        }
        else
        {
            miniGame.FlowerClicked();
        }
    }
}
