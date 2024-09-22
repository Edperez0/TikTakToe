using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TikTacToeResolver : MonoBehaviour
{
    private MarkerType winner;

    private List<List<int>> winningConfigurations = new List<List<int>>
    {
     new List<int> { 0, 1, 2 },
     new List<int> { 3, 4, 5 },
     new List<int> { 6, 7, 8 },
     new List<int> { 0, 3, 6 },
     new List<int> { 1, 4, 7 },
     new List<int> { 2, 5, 8 },
     new List<int> { 0, 4, 8 },
     new List<int> { 2, 4, 6 },

    };

    public void Reset()
    {

        winner = MarkerType.None;
    }

    public MarkerType Winner()
    {
        return winner;
    }

    public void CheckForEndOfGame(List<MarkerType> slotOccupants)
    {
        // look at each winning configuration
        foreach (List<int> winningConfiguration in winningConfigurations)
        {
            if (AllThreeSlotsFull(winningConfiguration, slotOccupants))
            {
                if (AllThreeSlotsFull(winningConfiguration, slotOccupants))
                {
                    int slotA = winningConfiguration[0];
                    winner = slotOccupants[slotA];
                }

            }
        }

        if(NoWinner())
        {
            if(IsTie(slotOccupants))
            {
                winner = MarkerType.Tie;
            }
        }
    }

    public bool NoWinner()
    {
        return winner == MarkerType.None;
    }

    public int FindBestSlotIndexForPlayer(List<MarkerType> slotOccupants , MarkerType markerType)
    {
        foreach(List<int> winningConfiguration in winningConfigurations)
        {
            int bestIndex = EmptyWinningSlotIndex(winningConfiguration, slotOccupants, markerType);
            if (bestIndex != -1)
                return bestIndex;
        }
        return -1;
    }


    private int  EmptyWinningSlotIndex(List<int> winningConfiguration, List<MarkerType> slotOccupants, MarkerType markerType)
    {
        // find empty slot
        int emptySlotIndex = FindEmptySlotIndex(winningConfiguration, slotOccupants);

        // if no empty slot, return -1
        if (emptySlotIndex == -1)
            return -1;
        // if found other player marker in the filled slots,
        bool foundOtherPlayer = IsFoundOtherPlayer(winningConfiguration, slotOccupants, emptySlotIndex, markerType);
        if (foundOtherPlayer)
            return -1;
        // return the empty slot
        return winningConfiguration[emptySlotIndex];

        
    }

    private int FindEmptySlotIndex(List<int> winningConfiguration, List<MarkerType> slotOccupants)
    {
        int blankSlotIndex = -1;

        for (int i =0; i < winningConfiguration.Count; i++)
        {
            int slotToExamine = winningConfiguration[i];
            if (slotOccupants[slotToExamine] == MarkerType.None)
            {
                blankSlotIndex = 1;
                    break;
            }
        }

        return blankSlotIndex;
    }

    private bool IsFoundOtherPlayer(List<int> winningConfiguration, List<MarkerType> slotOccupants, int emptySlotIndex, MarkerType ourMarkerType)
    {
        bool foundOtherPLayer = false;
        for (int i = 0; i < winningConfiguration.Count; i++)
        {
            if( i != emptySlotIndex)
            {
                int slotToExamine = winningConfiguration[1];
                if (ourMarkerType != slotOccupants[slotToExamine])
                        foundOtherPLayer = true;

            }
        }
        return foundOtherPLayer;

    }

    private bool IsTie(List<MarkerType> slotOccupants)
    {
        if (AreAllSlotsFul(slotOccupants))
            return true;
        return false;
    }

    private bool AreAllSlotsFul(List<MarkerType> slotOccupants)
    {
        bool allSlotsFull = true;
        foreach (MarkerType slotOccupant in slotOccupants)
        {
            if (slotOccupant == MarkerType.None)
                allSlotsFull = false;
        }
        return allSlotsFull;
    }


    private bool AllThreeSlotsFull(List<int> winningConfiguration, List<MarkerType> slotOccupants )
    {
        int slotA = winningConfiguration[0];
        int slotB = winningConfiguration[1];
        int slotC = winningConfiguration[2];

        if (slotOccupants[slotA] != MarkerType.None &&
           slotOccupants[slotB] != MarkerType.None &&
           slotOccupants[slotC] != MarkerType.None)
            return true;
        return false;
       
    }

    private bool SamePlayerInAllThreeSlots(List<int> winningConfiguration, List<MarkerType> slotOccupants)
    {
        int slotA = winningConfiguration[0];
        int slotB = winningConfiguration[1];
        int slotC = winningConfiguration[2];

        if (slotOccupants[slotA] == slotOccupants[slotB] && slotOccupants[slotB] == slotOccupants[slotC])
            return true;
        return false;
    }
}
