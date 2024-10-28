using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace BattleRTS.UnitFormations;

public static class UnitFormationUtils
{
  public static List<Vector2> CalculateSquareFormation(Vector2 startPosition, float distance, int positionCount)
  {
    List<Vector2> positionsAvailable = new List<Vector2>();
    int sideLength = (int)Mathf.Ceil(Mathf.Sqrt(positionCount)); // Determine the size of the square
    for (int i = 0; i < sideLength; i++)
    {
      for (int j = 0; j < sideLength; j++)
      {
        if (positionsAvailable.Count < positionCount)
        {
          Vector2 position = startPosition + new Vector2(i * distance, j * distance);
          positionsAvailable.Add(position);
        }
      }
    }
    return positionsAvailable;
  }

  public static List<Vector2> CalculateCircleFormation(Vector2 startPosition, float radius, int positionCount)
  {
    List<Vector2> positionsAvailable = new List<Vector2>();
    for (int i = 0; i < positionCount; i++)
    {
      float angle = i * (360f / positionCount);
      Vector2 position = startPosition + new Vector2(Mathf.Cos(Mathf.DegToRad(angle)), Mathf.Sin(Mathf.DegToRad(angle))) * radius;
      positionsAvailable.Add(position);
    }
    return positionsAvailable;
  }

  public static List<Vector2> CalculateWedgeFormation(Vector2 startPosition, float distance, int positionCount)
  {
    List<Vector2> positionsAvailable = new List<Vector2>();
    int rows = 0;
    int totalUnits = 0;

    // Calculate how many rows we need to fit the positionCount
    while (totalUnits < positionCount)
    {
      rows++;
      totalUnits += rows; // Each row adds one more unit than the previous row
    }

    for (int i = 0; i < rows; i++)
    {
      for (int j = 0; j <= i; j++)
      {
        if (positionsAvailable.Count < positionCount)
        {
          // Calculate the offset for each position in the row
          Vector2 position = startPosition + new Vector2(j * distance - (i * distance) / 2, i * distance);
          positionsAvailable.Add(position);
        }
      }
    }

    return positionsAvailable;
  }

  public static List<Vector2> CalculateLineFormation(Vector2 startPosition, float distance, int positionCount)
  {
    List<Vector2> positionsAvailable = new List<Vector2>();
    for (int i = 0; i < positionCount; i++)
    {
      Vector2 position = startPosition + new Vector2(i * distance, 0); // Horizontal line
      positionsAvailable.Add(position);
    }
    return positionsAvailable;
  }

  public static List<Vector2> CalculateStaggeredFormation(Vector2 startPosition, float distance, int positionCount)
  {
    List<Vector2> positionsAvailable = new List<Vector2>();
    int rows = (int)Mathf.Ceil(Mathf.Sqrt(positionCount)); // Calculate number of rows needed

    for (int i = 0; i < rows; i++)
    {
      for (int j = 0; j < rows; j++)
      {
        // Offset every second row
        if (positionsAvailable.Count < positionCount)
        {
          float offsetX = (i % 2 == 0) ? 0 : distance / 2; // Offset for staggered effect
          Vector2 position = startPosition + new Vector2(j * distance + offsetX, i * distance);
          positionsAvailable.Add(position);
        }
      }
    }

    return positionsAvailable;
  }



  public static List<Vector2> CalculateDiamondFormation(Vector2 startPosition, float distance, int positionCount)
  {
    List<Vector2> positionsAvailable = new List<Vector2>();
    int rows = (int)Mathf.Ceil(Mathf.Sqrt(positionCount));

    for (int i = 0; i < rows; i++)
    {
      for (int j = 0; j < rows; j++)
      {
        // Calculate diamond-like positions
        if (positionsAvailable.Count < positionCount)
        {
          float xOffset = (rows - 1 - i) * distance / 2 + j * distance;
          float yOffset = Mathf.Abs(rows - 1 - i) * distance;
          Vector2 position = startPosition + new Vector2(xOffset, yOffset);
          positionsAvailable.Add(position);
        }
      }
    }

    return positionsAvailable;
  }

  public static List<Vector2> CalculateHexagonalFormation(Vector2 startPosition, float distance, int positionCount)
  {
    List<Vector2> positionsAvailable = new List<Vector2>();
    int rows = (int)Mathf.Ceil(Mathf.Sqrt(positionCount)); // Calculate number of rows needed

    for (int i = 0; i < rows; i++)
    {
      for (int j = 0; j < rows; j++)
      {
        if (positionsAvailable.Count < positionCount)
        {
          // Calculate the hexagonal offset
          float offsetX = (i % 2 == 0) ? 0 : distance * 0.5f; // Offset every other row
          Vector2 position = startPosition + new Vector2(j * distance + offsetX, i * distance * Mathf.Sqrt(3) / 2);
          positionsAvailable.Add(position);
        }
      }
    }

    return positionsAvailable;
  }

  public static List<Vector2> CalculateColumnFormation(Vector2 startPosition, float distance, int positionCount)
  {
    List<Vector2> positionsAvailable = new List<Vector2>();
    for (int i = 0; i < positionCount; i++)
    {
      Vector2 position = startPosition + new Vector2(0, i * distance);
      positionsAvailable.Add(position);
    }
    return positionsAvailable;
  }
  public static List<Vector2> CalculateRowFormation(Vector2 startPosition, float distance, int positionCount)
  {
    List<Vector2> positionsAvailable = new List<Vector2>();
    for (int i = 0; i < positionCount; i++)
    {
      Vector2 position = startPosition + new Vector2(i * distance, 0);
      positionsAvailable.Add(position);
    }
    return positionsAvailable;
  }

  public static List<Vector2> CalculateCircularWedgeFormation(Vector2 startPosition, float distance, int positionCount)
  {
    List<Vector2> positionsAvailable = new List<Vector2>();

    // Divide the circle into equal parts based on the number of units
    for (int i = 0; i < positionCount; i++)
    {
      float angle = i * (360f / positionCount) + 90; // Start from the top
      Vector2 position = startPosition + new Vector2(Mathf.Cos(Mathf.DegToRad(angle)), Mathf.Sin(Mathf.DegToRad(angle))) * distance;

      positionsAvailable.Add(position);
    }

    return positionsAvailable;
  }

  public static List<Vector2> CalculateTShapeFormation(Vector2 startPosition, float distance, int positionCount)
  {
    List<Vector2> positionsAvailable = new List<Vector2>();

    // Horizontal part of T (3 units max)
    int horizontalLength = Mathf.Min(3, positionCount);
    for (int i = 0; i < horizontalLength; i++)
    {
      positionsAvailable.Add(startPosition + new Vector2(i * distance, 0));
    }

    // Vertical part of T (the remaining units)
    int verticalLength = positionCount - horizontalLength; // Remaining units
    for (int i = 0; i < verticalLength; i++)
    {
      positionsAvailable.Add(startPosition + new Vector2(1 * distance, (i + 1) * distance)); // Down from the center
    }

    return positionsAvailable;
  }

  public static List<Vector2> CalculateLShapeFormation(Vector2 startPosition, float distance, int positionCount)
  {
    List<Vector2> positionsAvailable = new List<Vector2>();

    // Vertical part of L
    int verticalLength = Mathf.Min(positionCount, 3); // Maximum 3 units vertically
    for (int i = 0; i < verticalLength; i++)
    {
      positionsAvailable.Add(startPosition + new Vector2(0, i * distance));
    }

    // Horizontal part of L
    if (positionCount > verticalLength)
    {
      int horizontalLength = positionCount - verticalLength; // Remaining units
      for (int i = 0; i < horizontalLength; i++)
      {
        positionsAvailable.Add(startPosition + new Vector2(i * distance, (verticalLength - 1) * distance)); // Horizontal at the base
      }
    }

    return positionsAvailable;
  }
  public static List<Vector2> CalculateCrossShapeFormation(Vector2 startPosition, float distance, int positionCount)
  {
    List<Vector2> positionsAvailable = new List<Vector2>();

    // Horizontal part of the cross
    int horizontalLength = Mathf.Min(3, positionCount); // Center + 1 unit to each side
    for (int i = 0; i < horizontalLength; i++)
    {
      positionsAvailable.Add(startPosition + new Vector2(i * distance - distance / 2, 0)); // Centered horizontally
    }

    // Vertical part of the cross
    int verticalLength = positionCount - horizontalLength; // Remaining units for vertical
    int halfVerticalLength = verticalLength / 2; // Center the vertical part

    for (int i = -halfVerticalLength; i <= halfVerticalLength; i++)
    {
      // Ensure we do not place more than the available units
      if (positionsAvailable.Count < positionCount)
      {
        positionsAvailable.Add(startPosition + new Vector2(0, i * distance)); // Centered vertically
      }
    }

    return positionsAvailable;
  }

  public static List<Vector2> CalculateSnakeFormation(Vector2 startPosition, float distance, int positionCount)
  {
    List<Vector2> positionsAvailable = new List<Vector2>();
    for (int i = 0; i < positionCount; i++)
    {
      float xOffset = (i % 2 == 0) ? 0 : distance; // Offset every other unit
      positionsAvailable.Add(startPosition + new Vector2(xOffset, (i * distance) / 2));
    }
    return positionsAvailable;
  }

  public static List<Vector2> CalculateRandomFormation(Vector2 startPosition, float radius, int positionCount)
  {
    List<Vector2> positionsAvailable = new List<Vector2>();
    Random random = new Random();

    while (positionsAvailable.Count < positionCount)
    {
      // Generate a random position within a circle
      float angle = (float)(random.NextDouble() * Math.PI * 2); // Random angle
      float distance = (float)(random.NextDouble() * radius); // Random distance within the radius

      Vector2 randomPosition = startPosition + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * distance;

      // Check for overlap before adding
      if (!positionsAvailable.Any(pos => pos.DistanceTo(randomPosition) < 8f)) // Assuming 8f is the unit's spacing
      {
        positionsAvailable.Add(randomPosition);
      }
    }

    return positionsAvailable;
  }

  public static List<Vector2> CalculateCrescentShapeFormation(Vector2 startPosition, float outerRadius, float innerRadius, int positionCount)
  {
    List<Vector2> positionsAvailable = new List<Vector2>();
    int outerCount = positionCount / 2; // Half for the outer circle
    int innerCount = positionCount - outerCount; // Remaining for the inner circle

    // Create outer circle positions
    for (int i = 0; i < outerCount; i++)
    {
      float angle = i * (360f / outerCount);
      Vector2 position = startPosition + new Vector2(Mathf.Cos(Mathf.DegToRad(angle)), Mathf.Sin(Mathf.DegToRad(angle))) * outerRadius;
      positionsAvailable.Add(position);
    }

    // Create inner circle positions (offset to create the crescent effect)
    for (int i = 0; i < innerCount; i++)
    {
      float angle = i * (360f / innerCount);
      Vector2 position = startPosition + new Vector2(Mathf.Cos(Mathf.DegToRad(angle)), Mathf.Sin(Mathf.DegToRad(angle))) * innerRadius;
      positionsAvailable.Add(position);
    }

    return positionsAvailable;
  }


}
