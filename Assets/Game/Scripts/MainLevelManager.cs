using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FateGames;
public class MainLevelManager : LevelManager
{
    public static MainLevelManager Instance { get => (MainLevelManager)_Instance; }
    private Road road = null;
    private List<GameObject> confettiPositions;
    private Player player = null;
    [SerializeField] private Text coinText;
    private int coin = 0;
    public Text CoinText { get => coinText; }
    public int Coin { get => coin; }
    public Road Road { get => road; }
    public Player Player { get => player; }
    public List<GameObject> ConfettiPositions { get => confettiPositions; }

    private new void Awake()
    {
        base.Awake();
        road = FindObjectOfType<Road>();
        player = FindObjectOfType<Player>();
        confettiPositions = new List<GameObject>(GameObject.FindGameObjectsWithTag("Confetti Position"));
    }

    private void Start()
    {
        coinText.text = PlayerProgression.COIN.ToString();
    }
    public override void StartLevel()
    {
        player.StartWalking();
    }
    public override void FinishLevel(bool success)
    {
        if (GameManager.Instance.State != GameManager.GameState.FINISHED)
        {
            GameManager.Instance.State = GameManager.GameState.FINISHED;
            // CODE HERE ********





            // ******************
            LeanTween.delayedCall(2, () => { GameManager.Instance.FinishLevel(success); });

            if (success) PlayerProgression.COIN += coin;
        }
    }

    public void AddCoin(int number)
    {
        coin += number;
    }


}
