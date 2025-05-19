using UnityEngine;

public class CardManager : MonoBehaviour
{
    public int currentCard = 2;
    private int prevCard;
    public int getNewCard()
    {
        prevCard = currentCard;
        currentCard = Random.Range(2,15);
        if(currentCard == prevCard)
        {
            currentCard = Random.Range(2,15);
        }

        return currentCard;
    }
}
