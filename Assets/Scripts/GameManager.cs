using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public CardManager cardManager;
    public GameObject startButton;
    public GameObject losePanel;
    public GameObject LowerText;
    public GameObject HigherText;
    public GameObject betPanel;
    public GameObject betButton;
    public TextMeshProUGUI cardText;
    public TextMeshProUGUI chipsText;
    public TextMeshProUGUI roundsText;
    public TextMeshProUGUI resultText;

    public TextMeshProUGUI finalChipsText;
    public TextMeshProUGUI betText;
    public Slider betSlider;
    public Image cardImage;
    public Image nextCardImage;
    public Sprite placeHolder;
    public Sprite[] cardSprites;

    public int presetChips = 100;
    private int playerChips = 0;
    private int betAmount = 10;
    private int roundsPlayed = 0;
    private int currentCard;
    private bool awaitingBet = false;
    private bool awaitingGuess = false;

    void Start()
    {
        currentCard = 0;
        nextCardImage.gameObject.SetActive(false);
        resultText.gameObject.SetActive(false);
        UpdateUI();
    }

    public void StartNewRound()
    {
        playerChips = presetChips;
        currentCard = 0;
        roundsPlayed = 0;
        startButton.SetActive(false);
        losePanel.SetActive(false);
        LowerText.SetActive(true);
        HigherText.SetActive(true);
        cardImage.gameObject.SetActive(true);

        ShowBetPanel();
        UpdateUI();
    }

    public void ShowBetPanel()
    {
        currentCard = 0;
        awaitingBet = true;
        awaitingGuess = false;
        betPanel.SetActive(true);
        betSlider.maxValue = playerChips;
        betSlider.minValue = 1;
        betSlider.value = Mathf.Min(betAmount, playerChips);
        UpdateBetText();
        UpdateUI();
    }

    public void UpdateBetText()
    {
        betAmount = (int)betSlider.value;
        betText.text = $"Bet Amount: {betAmount}";
    }

    public void PlaceBet()
    {
        if (!awaitingBet) return;

        awaitingBet = false;
        awaitingGuess = true;
        betPanel.SetActive(false);

        currentCard = cardManager.getNewCard(); // Draw card after placing bet
        UpdateUI();
    }

    public void GuessHigher()
    {
        if (!awaitingGuess) return;

        ProcessGuess(cardManager.getNewCard(), isHigherGuess: true);
    }

    public void GuessLower()
    {
        if (!awaitingGuess) return;

        ProcessGuess(cardManager.getNewCard(), isHigherGuess: false);
    }

    private void ProcessGuess(int nextCard, bool isHigherGuess)
    {
    roundsPlayed++;
    awaitingGuess = false;

    StartCoroutine(ShowNextCardAndResult(nextCard, isHigherGuess));
    }

    private IEnumerator ShowNextCardAndResult(int nextCard, bool isHigherGuess)
    {
    // Show next card visually
    nextCardImage.sprite = cardSprites[nextCard - 2];
    nextCardImage.gameObject.SetActive(true);

    yield return new WaitForSeconds(1.5f); // Wait to build suspense

    bool win = isHigherGuess ? (nextCard > currentCard) : (nextCard < currentCard);

    if (win)
    {
        resultText.text = "You Win!";
        playerChips += betAmount;
    }
    else
    {
        resultText.text = "You Lose!";
        playerChips -= betAmount;
    }

    resultText.gameObject.SetActive(true);

    yield return new WaitForSeconds(1.5f); // Let result sit

    // Update state
    currentCard = nextCard;
    UpdateUI();
    resultText.gameObject.SetActive(false);
    nextCardImage.gameObject.SetActive(false);

    CheckPlayerLose();

    if (playerChips > 0)
    {
        ShowBetPanel();
    }
}

    private void CheckPlayerLose()
    {
        if (playerChips <= 0)
        {
            currentCard = 0;
            UpdateUI();
            losePanel.SetActive(true);
            roundsText.text = $"Rounds Played: {roundsPlayed}";
            finalChipsText.text = "You ran out of chips!";
        }
    }

    public void RestartGame()
    {
        losePanel.SetActive(false);
        StartNewRound();
    }

    private void UpdateUI()
    {
        cardText.text = (currentCard >= 2 && currentCard <= 14) ? $"Card: {currentCard}" : "Card: ?";
        chipsText.text = $"Chips: {playerChips}";

        if (currentCard >= 2 && currentCard <= 14)
        {
            cardImage.sprite = cardSprites[currentCard - 2];
        }
        else
        {
            cardImage.sprite = placeHolder;
        }
    }
}
