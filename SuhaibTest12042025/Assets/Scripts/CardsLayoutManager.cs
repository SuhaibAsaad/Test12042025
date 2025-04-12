using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CardsLayoutManager : MonoBehaviour
{
    [SerializeField] GameObject cardPrefab;
    [SerializeField] List<CardData> cardsPool;
    public List<CardManager> cards = new List<CardManager>();
    public int currentRows, currentColumns;
    int numberOfCards;
    GridLayoutGroup gridLayout;

    private void Start()
    {
        gridLayout = GetComponent<GridLayoutGroup>();
    }

    public string GenerateCards(int rows, int columns)
    {
        numberOfCards = rows * columns;
        if (numberOfCards >= cardsPool.Count * 2)
        {
            return  "Not enough card data avaliable to generate this layout";
        }

        if (numberOfCards % 2 != 0)
        {
            return "Invalid formation, number of cards must be divisible by 2";
        }

        if (numberOfCards == 0)
        {
            return "Invalid formation, can't have zero";
        }

        currentColumns = columns;
        currentRows = rows;

        if (transform.childCount != 0)
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
            cards.Clear();
        }

        for (int i = 0; i < numberOfCards; i++)
        {
          cards.Add(Instantiate(cardPrefab, transform).GetComponent<CardManager>());
        }

        gridLayout.constraintCount = rows;

        CalculateCellSize(columns,rows);
        LoadCardsWithData();

        return "Generation Successful";
    }

    public void GenerateCardsFromSaveData(int rows, int columns, List<CardSaveData> cardsSaveData)
    {
        numberOfCards = rows * columns;
        currentColumns = columns;
        currentRows = rows;

        if (transform.childCount != 0)
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
            cards.Clear();
        }

        for (int i = 0; i < numberOfCards; i++)
        {
            cards.Add(Instantiate(cardPrefab, transform).GetComponent<CardManager>());
        }

        cards = cards.OrderBy(x => Random.value).ToList(); //shuffle the cards

        for (int i = 0; i < numberOfCards; i++)
        {
            cards[i].AssignCardFromSaveData(cardsSaveData[i]);
        }

        gridLayout.constraintCount = rows;

        CalculateCellSize(columns, rows);
    }

    void LoadCardsWithData()
    {
        List<CardData> currentCardsPool = new List<CardData>(cardsPool).OrderBy(x => Random.value).ToList(); //shuffle the cards pool 
        for(int i = 0,  j = 0; i < cards.Count; i++, j++)
        {
            cards[i].AssignCard(currentCardsPool[j]); //assign a card data to a card
            i++;
            cards[i].AssignCard(currentCardsPool[j]); //assign the same card data to the next card
        }

        cards = cards.OrderBy(x => Random.value).ToList(); //shuffle the cards
    }

    public void CalculateCellSize(int columns, int rows)
    {
        float layoutWidth = GetComponent<RectTransform>().rect.width;
        float layoutHeight = GetComponent<RectTransform>().rect.height;

        //if the container window is more height than width, calculate cell size by width, otherwise height
        if (layoutWidth < layoutHeight)
        {
            CalculateCellSizeBasedOnWidth(layoutWidth,layoutHeight,rows, columns);
        }
        else
        {
            CalculateCellSizeBasedOnHeight(layoutWidth,layoutHeight, rows,columns);
        }

    }

    void CalculateCellSizeBasedOnWidth(float width, float height, int rows, int columns)
    {
        //estimating cell dimensions 
        float layoutWidth = width;
        float cellDimensions = layoutWidth / columns;
        int spacing = (int)cellDimensions / 4;
        cellDimensions = cellDimensions - (spacing * 2);

        //incase of a scenario where there is a large difference between rows and columns, such as 2x5, make sure that with the current cell size , it won't go outside the window, otherwise switch to calculating by height
        float totalHeightSpace = (cellDimensions * rows) + (spacing * (rows - 1));
        if (totalHeightSpace > height)
        {
            CalculateCellSizeBasedOnHeight(width,  height, rows, columns);
        }

        else
        {
            gridLayout.cellSize = new Vector2(cellDimensions, cellDimensions);
            gridLayout.padding = new RectOffset(spacing, spacing, spacing, spacing);
            gridLayout.spacing = new Vector2(spacing, spacing);
        }

    }

    void CalculateCellSizeBasedOnHeight(float width, float height, int rows, int columns)
    {
        //estimating cell dimensions
        float layoutHeight = height;
        float cellDimensions = layoutHeight / rows;
        int spacing = (int)cellDimensions / 4;
        cellDimensions = cellDimensions - (spacing * 2);


        //incase of a scenario where there is a large difference between rows and columns, such as 2x5, make sure that with the current cell size , it won't go outside the window, otherwise switch to calculating by width
        float totalWidthSpace = (cellDimensions * currentColumns) + (spacing * (currentColumns - 1));
        if (totalWidthSpace > width)
        {
            CalculateCellSizeBasedOnWidth(width, height, rows, columns);
        }

        else
        {
            gridLayout.cellSize = new Vector2(cellDimensions, cellDimensions);
            gridLayout.padding = new RectOffset(spacing, spacing, spacing, spacing);
            gridLayout.spacing = new Vector2(spacing, spacing);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            GenerateCards(currentRows,currentColumns);
        }
    }
}
