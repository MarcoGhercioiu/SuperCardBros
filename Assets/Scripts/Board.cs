using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]

public class Board : MonoBehaviour
{

    public static readonly int BoardWidth = 4;
    public static readonly int BoardHeight = 2;
    public BoardSpace[,] spaces;
    public GameObject BoardObject;
    public GameObject BoardSpace1;
    public GameObject BoardSpace2;
    public GameObject BoardSpace3;
    public GameObject BoardSpace4;
    public GameObject BoardSpace5;
    public GameObject BoardSpace6;
    public GameObject BoardSpace7;
    public GameObject BoardSpace8;

    void Awake()
    {
        spaces = new BoardSpace[BoardHeight, BoardWidth];
        spaces[0, 0] = BoardSpace1.GetComponent<BoardSpace>() as BoardSpace;
        spaces[0, 1] = BoardSpace2.GetComponent<BoardSpace>() as BoardSpace;
        spaces[0, 2] = BoardSpace3.GetComponent<BoardSpace>() as BoardSpace;
        spaces[0, 3] = BoardSpace4.GetComponent<BoardSpace>() as BoardSpace;
        spaces[1, 0] = BoardSpace5.GetComponent<BoardSpace>() as BoardSpace;
        spaces[1, 1] = BoardSpace6.GetComponent<BoardSpace>() as BoardSpace;
        spaces[1, 2] = BoardSpace7.GetComponent<BoardSpace>() as BoardSpace;
        spaces[1, 3] = BoardSpace8.GetComponent<BoardSpace>() as BoardSpace;
    }

    public void init()
    {
        this.reset();
    }

    public void reset()
    {
        for (int col = 0; col < BoardWidth; col++)
        {
            spaces[0, col].init();
            spaces[1, col].init();
        }
    }

    public bool addCard(Card card, BoardSpace selectedBoardSpace)
    {
        if (!selectedBoardSpace.occupied)
        {
            card.enableHandPrefab();
            selectedBoardSpace.card = card;
            selectedBoardSpace.occupied = true;
            card.CardObject.transform.parent = selectedBoardSpace.boardSpaceObject.transform;
            card.CardObject.transform.localScale = new Vector3(UIConstants.CardOnBoardSize, UIConstants.CardOnBoardSize, UIConstants.CardOnBoardSize);
            card.CardObject.transform.localPosition = new Vector3(0, 0, 0);
            card.CardObject.transform.localRotation = Quaternion.identity;
            return true;
        }
        else
        {
            return false;
        }
    }

    public int cardsOnRow(int row)
    {
        int counter = 0;
        for (int i = 0; i < BoardWidth; i++)
        {
            if (spaces[row, i].occupied)
            {
                counter++;
            }
        }
        return counter;
    }

    public int getOpenSpaces(int row)
    {
        int counter = 0;
        for (int i = 0; i < BoardWidth; i++)
        {
            if (!spaces[row, i].occupied)
            {
                counter++;
            }
        }
        return counter;
    }

    public BoardSpace getFirstAvailableSpace(int row)
    {
        for (int col = 0; col < BoardWidth; col++)
        {
            if (!spaces[row, col].occupied)
            {
                return spaces[row, col];
            }
        }
        return null;
    }

    public int getIndexOfBoardSpace(int row, BoardSpace s)
    {
        for (int col = 0; col < BoardWidth; col++)
        {
            if (spaces[row, col] == s)
            {
                return col;
            }
        }
        return -1;
    }

    public BoardSpace getFirstSpaceLeft(int row, BoardSpace s)
    {
        int boardSpaceIndex = getIndexOfBoardSpace(row, s);
        if (boardSpaceIndex == -1)
        {
            Debug.Log("Cannot find the board space referenced by card.");
        }
        else
        {
            int newIndex = boardSpaceIndex - 1;
            if (newIndex < 0)
            {
                newIndex = BoardWidth - 1;
            }
            while (spaces[row, newIndex].occupied)
            {
                newIndex = newIndex - 1;
                if (newIndex < 0)
                {
                    newIndex = BoardWidth - 1;
                }
            }
            return spaces[row, newIndex];
        }
        return null;
    }

    public BoardSpace getFirstSpaceRight(int row, BoardSpace s)
    {
        int boardSpaceIndex = getIndexOfBoardSpace(row, s);
        if (boardSpaceIndex == -1)
        {
            Debug.Log("Cannot find the board space referenced by card.");
        }
        else
        {
            int newIndex = boardSpaceIndex + 1;
            if (newIndex >= BoardWidth)
            {
                newIndex = 0;
            }
            while (spaces[row, newIndex].occupied)
            {
                newIndex = newIndex + 1;
                if (newIndex >= BoardWidth)
                {
                    newIndex = 0;
                }
            }
            return spaces[row, newIndex];
        }
        return null;
    }

    public void runGameLogic(CPU cpu, Player player)
    {
        for (int column = 0; column < BoardWidth; column++)
        {
            BoardSpace CPUBoardSpace = spaces[1, column];
            BoardSpace PlayerBoardSpace = spaces[0, column];
            if (!CPUBoardSpace.occupied && !PlayerBoardSpace.occupied)
            {
                continue;
            }
            else if (!CPUBoardSpace.occupied)
            {
                cpu.health -= PlayerBoardSpace.card.attack;
            }
            else if (!PlayerBoardSpace.occupied)
            {
                player.health -= CPUBoardSpace.card.attack;
            }
            else
            {
                int[] leftoverDamage = this.battleCards(CPUBoardSpace, PlayerBoardSpace);
                player.health += leftoverDamage[0];
                cpu.health += leftoverDamage[1];
            }

        }

        player.updateUI();
        cpu.updateUI();

    }

    private int[] battleCards(BoardSpace cpuSpace, BoardSpace playerSpace)
    {
        Card cpuCard = cpuSpace.card;
        Card playerCard = playerSpace.card;
        int[] sum = { 0, 0 };
        int cpuHealth = cpuCard.health;
        int cpuAttack = cpuCard.attack;
        int playerHealth = playerCard.health;
        int playerAttack = playerCard.attack;
        sum[0] = playerHealth - cpuAttack;
        sum[1] = cpuHealth - playerAttack;
        if (sum[0] > 0)
        {
            playerCard.health = sum[0];
            sum[0] = 0;
            playerCard.cardDisplay.updateDisplay();
        }
        else
        {
            this.destroy(playerSpace);
        }
        if (sum[1] > 0)
        {
            cpuCard.health = sum[1];
            sum[1] = 0;
            cpuCard.cardDisplay.updateDisplay();
        }
        else
        {
            this.destroy(cpuSpace);
        }
        return sum;
    }

    private void destroy(BoardSpace space)
    {
        space.occupied = false;
        Destroy(space.card.CardObject);
        space.card = null;
    }

    void Update()
    { }
}
