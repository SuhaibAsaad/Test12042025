using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CardsLayoutManager : MonoBehaviour
{
    [SerializeField] GameObject cardPrefab;
    [SerializeField] List<CardData> cardsPool;
    public int rows, cols;
    int numberOfCards;
    GridLayoutGroup gridLayout;

    private void Start()
    {
        gridLayout = GetComponent<GridLayoutGroup>();
    }

    public void GenerateCards(int rows, int columns)
    {
        if (transform.childCount != 0)
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
        numberOfCards = rows * columns;
        if (numberOfCards >= cardsPool.Count)
        {
            print("Not enough card data avaliable to generate this layout");
        }

        for (int i = 0; i < numberOfCards; i++)
        {
            Instantiate(cardPrefab, transform);
        }

        gridLayout.constraintCount = rows;

        CalculateCellSize(columns,rows);


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
        float totalWidthSpace = (cellDimensions * cols) + (spacing * (cols - 1));
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
            GenerateCards(rows,cols);
        }
    }
}
