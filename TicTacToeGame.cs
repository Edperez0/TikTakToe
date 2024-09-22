using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicTacToeGame : MonoBehaviour
{
    public Slots Slots;
    public TikTacToeResolver TicTacToeResolver;
    public TurnDisplay TurnDisplay;
    public WinnerDisplay WinnerDisplay;
    public GameMode GameMode;
    public Sounds Sounds;


    private MarkerType currentMarkerType;
    private MarkerType firstPLayerMarkerType;
    private int numerOfTurnsPlayed;
    private bool isWaitingForComputerToPlay;

    // Start is called before the first frame update
    void Start()
    {
        Reset();
        
    }

    public void OnslotClicked(Slot slot)
    {
        if(!isWaitingForComputerToPlay)
        PlaceMarkerInSlot(slot);
    }

    public void OnResetButtonClicked()
    {
        Sounds.PlayResetButtonSound();
        Reset();
    }

    public void Reset()
    {
        ResetPlayers();
        ResetDisplays();
        Sounds.PlayResetButtonSound();
        numerOfTurnsPlayed = 0;
        ResetSlots();
        ResetSounds();
    }

    public void ChangeOpponent()
    {
        Reset();
    }

    private void ResetSlots()
    {
        Slots.Reset();
    }

    private void ResetSounds()
    {
        Sounds.Reset();

    }

    private void ResetPlayers()
    {
        TicTacToeResolver.Reset();
        RandomizePlayer();
        firstPLayerMarkerType = currentMarkerType;
        isWaitingForComputerToPlay = false;
    }


    private void ResetDisplays()
    {
        TurnDisplay.Reset(currentMarkerType);
        WinnerDisplay.Reset();
    }

    private void PlaceMarkerInSlot(Slot slot)
    {
        if (GameNotOver())
        {
            UpdateSlotImage(slot);
            Sounds.PlayRandomMarkerSound();
            CheckForWinner();

            EndTurn();
        }
    }

    

    private bool GameNotOver()
    {
        return TicTacToeResolver.NoWinner();
    }

    private void CheckForWinner()
    {
        numerOfTurnsPlayed++;
        if (numerOfTurnsPlayed < 5)
            return;

        TicTacToeResolver.CheckForEndOfGame(Slots.SlotOccupants());
    }

    private void EndTurn()
    {
        if(GameNotOver())
             ChangePlayer();
        else
            ShowWinner();
    }

    private void ShowWinner()
    {
        PlayEndOfGameSound();
        WinnerDisplay.Show(TicTacToeResolver.Winner());

    }

    private void PlayEndOfGameSound()
    {
        if (TicTacToeResolver.Winner() == MarkerType.Tie)
            Sounds.PlayTieGameSound();
        else
            Sounds.PlayGameModeSound();
    }

    private void ChangePlayer()
    {
        if (currentMarkerType == MarkerType.Paw)
            currentMarkerType = MarkerType.Panther;
        else
            currentMarkerType = MarkerType.Paw;

        TurnDisplay.Show(currentMarkerType);

        SeeIfComputerShouldPlay();

    }

    private void SeeIfComputerShouldPlay()
    {
        if (IsHumanTurn())
            return;
        if (IsPlayerComputerOpponent())
            PlayComputerTurn();

    }

    private bool IsPlayerComputerOpponent()
    {
        return GameMode.GetOpponentType() != OpponentType.Human;

    }

    private void PlayComputerTurn()
    {
        StartCoroutine(PauseForComputerPlayer());
    }

    IEnumerator PauseForComputerPlayer()
    {
        isWaitingForComputerToPlay = true;
        float secondsToWait = Random.Range(0.5f, 1f);
        yield return new WaitForSeconds(secondsToWait);
        isWaitingForComputerToPlay = false;
        PlayComputerTurnAfterPause();

    }

    private void PlayComputerTurnAfterPause()
    {
        if (GameMode.GetOpponentType() == OpponentType.EasyComputer)
        {
            PlayEasyComputerMove();
        }
        else if (GameMode.GetOpponentType() == OpponentType.HardComputer)
        {
            PlayHardComputerMove();
        }
    }

    private void PlayHardComputerMove()
    {
        // if can win,
        bool hasWon = TryToWin();
        if (hasWon)
            return;

        // if can block
        bool hasBlocked = TryToBlock();
        if (hasBlocked)
            return;

        //random slot
        PlayMarkerInRandomeSlot();
    }

    private bool TryToWin()
    {
        return TryToPlayBestMoveForPlayer(currentMarkerType);
    }

    private bool TryToBlock()
    {
        return TryToPlayBestMoveForPlayer(firstPLayerMarkerType);
    }

    private bool TryToPlayBestMoveForPlayer(MarkerType markerType)
    {
        int BestSlotIndex = TicTacToeResolver.FindBestSlotIndexForPlayer(Slots.SlotOccupants(), firstPLayerMarkerType);
        if (BestSlotIndex != -1)
        {
            PlaceMarkerInSlot(Slots.GetSlot(BestSlotIndex));
            return true;
        }

        return false;
    }

    private void PlayEasyComputerMove()
    {
        PlayMarkerInRandomeSlot();
    }

    private void PlayMarkerInRandomeSlot()
    {
        Slot slot = Slots.RandomFreeSlot();
        PlaceMarkerInSlot(slot);
    }


    private bool IsHumanTurn()
    {
        return currentMarkerType == firstPLayerMarkerType;
    }
    private void UpdateSlotImage(Slot slot)
    {
        Slots.UpdateSlot(slot, currentMarkerType);
    }

    private void RandomizePlayer()
    {
        int randomNumber = Random.Range(1, 3);
        if (randomNumber == 1)
            currentMarkerType = MarkerType.Panther;
        else
            currentMarkerType = MarkerType.Paw;
    }

}
