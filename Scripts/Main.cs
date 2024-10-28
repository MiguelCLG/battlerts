using Godot;
using System;
using System.Collections.Generic;

public partial class Main : Node2D
{
  private Vector2 startMousePosition;
  private Area2D selectionBox;
  string formationType = "wedge";
  List<Unit> selectedUnits = new();

  public override void _Ready()
  {
    selectionBox = GetNode<Area2D>("%Area2D");
    selectionBox.Visible = false;
  }

  public override void _UnhandledInput(InputEvent @event)
  {
    if (@event is InputEventKey key && key.IsPressed())
    {
      switch (key.Keycode)
      {
        case Key.Key1: formationType = "square"; break;
        case Key.Key2: formationType = "circle"; break;
        case Key.Key3: formationType = "wedge"; break;
      }
    }
    if (@event is InputEventMouseButton mouseButton && mouseButton.IsPressed() && mouseButton.ButtonIndex == MouseButton.Left)
    {
      selectionBox.Visible = true;
      startMousePosition = GetGlobalMousePosition(); // Store the starting position of the mouse

      // Set the initial position of the selection box to the starting mouse position
      selectionBox.GlobalPosition = startMousePosition;
    }

    if (@event is InputEventMouseMotion mouseMotion && selectionBox.Visible)
    {
      Vector2 currentMousePosition = GetGlobalMousePosition();

      // Calculate the width and height of the selection area
      float width = currentMousePosition.X - startMousePosition.X;
      float height = currentMousePosition.Y - startMousePosition.Y;

      // Update the scale to represent the width and height of the selection area
      selectionBox.Scale = new Vector2(Mathf.Abs(width), Mathf.Abs(height));

      // Adjust the position of the selection box to remain at the top-left corner
      selectionBox.GlobalPosition = new Vector2(
          Mathf.Min(startMousePosition.X, currentMousePosition.X), // Top-left x
          Mathf.Min(startMousePosition.Y, currentMousePosition.Y)  // Top-left y
      );
    }

    if (@event is InputEventMouseButton mouseButtonRelease && mouseButtonRelease.IsReleased() && mouseButtonRelease.ButtonIndex == MouseButton.Left)
    {
      selectionBox.Visible = false;
      foreach (Unit unit in selectedUnits)
      {
        unit.SetSelected(false);
      }
      selectedUnits.Clear();
      // Create a Rect2 representing the selection box
      Rect2 selectionRect = new Rect2(selectionBox.GlobalPosition, selectionBox.Scale);

      // Detect overlapping objects
      CheckOverlappingObjects(selectionRect);

      // Reset selection box scale for next use
      selectionBox.Scale = Vector2.Zero;
    }

    // move units to specified position
    if (@event is InputEventMouseButton rightClick && rightClick.Pressed && rightClick.ButtonIndex == MouseButton.Right)
    {
      if (selectedUnits.Count == 1)
      {
        selectedUnits[0].SetMovePosition(GetGlobalMousePosition());
      }
      else
      {

        List<Vector2> positionsAvailable;

        switch (formationType)
        {
          case "square":
            positionsAvailable = CalculateSquareFormation(GetGlobalMousePosition(), 8f, selectedUnits.Count);
            break;
          case "circle":
            positionsAvailable = CalculateCircleFormation(GetGlobalMousePosition(), 8f, selectedUnits.Count);
            break;
          case "wedge":
            positionsAvailable = CalculateWedgeFormation(GetGlobalMousePosition(), 8f, selectedUnits.Count);
            break;
          default:
            positionsAvailable = new List<Vector2>();
            break;
        }
        foreach (Unit unit in selectedUnits)
        {
          unit.SetMovePosition(positionsAvailable[selectedUnits.IndexOf(unit)]);
        }
      }
    }
  }

  /* private List<Vector2> CalculateValidPositions(Vector2 startPosition, float distance, int positionCount)
  {
    List<Vector2> positionsAvailable = new List<Vector2>();
    for (int i = 0; i < positionCount; i++)
    {
      // Calculate angle in radians
      float angle = i * Mathf.Tau / positionCount; // Use Mathf.Tau (2 * PI) for better accuracy
      Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)); // Create direction vector
      Vector2 position = startPosition + dir * distance; // Calculate the position
      positionsAvailable.Add(position);
    }

    return positionsAvailable;
  }
 */
  private List<Vector2> CalculateSquareFormation(Vector2 startPosition, float distance, int positionCount)
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

  private List<Vector2> CalculateCircleFormation(Vector2 startPosition, float radius, int positionCount)
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

  private List<Vector2> CalculateWedgeFormation(Vector2 startPosition, float distance, int positionCount)
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




  private void CheckOverlappingObjects(Rect2 selectionRect)
  {
    // Iterate over all Node2D objects in the "selectable" group
    foreach (Node2D obj in GetTree().GetNodesInGroup("selectable"))
    {
      // Get the object's global position and calculate its Rect2 based on its size
      Vector2 objGlobalPosition = obj.GlobalPosition;
      Vector2 objSize = obj.Scale; // Adjust this if your objects have a different way to define size

      // Create a Rect2 for the object using its position and size
      Rect2 objectRect = new Rect2(objGlobalPosition - objSize * 0.5f, objSize);

      // Check if the selectionRect overlaps the objectRect
      if (selectionRect.Intersects(objectRect))
      {
        selectedUnits.Add((Unit)obj);
        if (obj is Unit unit)
          unit.SetSelected(true);
      }
    }
  }
}
