using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;


public class PickUpScore : BasePickUp
{
    [SerializeField] private int[] possibleScores = new int[] {5, 10, 15, 20, 25};
    private int score;

    public static event Action<PickUpScore> OnPickUpScoreCollected;

    public int Score { get => score;}

    private void Start()
    {
        int randIndex = Random.Range(0, possibleScores.Length);
        score = possibleScores[randIndex];
    }

    protected override void ApplyEffect()
    {
        OnPickUpScoreCollected?.Invoke(this);
    }
}
