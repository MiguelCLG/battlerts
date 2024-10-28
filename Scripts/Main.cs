using Godot;
using System;
using System.Collections.Generic;
using static BattleRTS.UnitFormations.UnitFormationUtils;
public partial class Main : Node2D
{
  private Vector2 startMousePosition;
  private Area2D selectionBox;
  string formationType = "square";
  List<Unit> selectedUnits = new();

  public override void _Ready()
  {
    selectionBox = GetNode<Area2D>("%Area2D");
    selectionBox.Visible = false;
  }

  public override void _UnhandledInput(InputEvent @event)
  {
    if (@event is InputEventKey key && key.IsPressed() && !key.IsCommandOrControlPressed())
    {
      switch (key.Keycode)
      {
        case Key.Key1: formationType = "square"; break;
        case Key.Key2: formationType = "circle"; break;
        case Key.Key3: formationType = "wedge"; break;
        case Key.Key4: formationType = "line"; break;
        case Key.Key5: formationType = "staggered"; break;
        case Key.Key6: formationType = "diamond"; break;
        case Key.Key7: formationType = "hex"; break;
        case Key.Key8: formationType = "column"; break;
        case Key.Key9: formationType = "row"; break;
        case Key.Q: formationType = "circular_wedge"; break;
        case Key.W: formationType = "t_shape"; break;
        case Key.E: formationType = "l_shape"; break;
        case Key.R: formationType = "cross"; break;
        case Key.T: formationType = "snake"; break;
        case Key.Y: formationType = "random"; break;
        case Key.U: formationType = "crescent"; break;
        default: formationType = "square"; break;
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
            positionsAvailable = CalculateSquareFormation(GetGlobalMousePosition(), 15f, selectedUnits.Count);
            break;
          case "circle":
            positionsAvailable = CalculateCircleFormation(GetGlobalMousePosition(), 15f, selectedUnits.Count);
            break;
          case "wedge":
            positionsAvailable = CalculateWedgeFormation(GetGlobalMousePosition(), 15f, selectedUnits.Count);
            break;
          case "diamond":
            positionsAvailable = CalculateDiamondFormation(GetGlobalMousePosition(), 15f, selectedUnits.Count);
            break;
          case "line":
            positionsAvailable = CalculateLineFormation(GetGlobalMousePosition(), 15f, selectedUnits.Count);
            break;
          case "staggered":
            positionsAvailable = CalculateStaggeredFormation(GetGlobalMousePosition(), 15f, selectedUnits.Count);
            break;
          case "hex":
            positionsAvailable = CalculateHexagonalFormation(GetGlobalMousePosition(), 15f, selectedUnits.Count);
            break;
          case "column":
            positionsAvailable = CalculateColumnFormation(GetGlobalMousePosition(), 15f, selectedUnits.Count);
            break;
          case "row":
            positionsAvailable = CalculateRowFormation(GetGlobalMousePosition(), 15f, selectedUnits.Count);
            break;
          case "circular_wedge":
            positionsAvailable = CalculateCircularWedgeFormation(GetGlobalMousePosition(), 15f, selectedUnits.Count);
            break;
          case "t_shape":
            positionsAvailable = CalculateTShapeFormation(GetGlobalMousePosition(), 15f, selectedUnits.Count);
            break;
          case "l_shape":
            positionsAvailable = CalculateLShapeFormation(GetGlobalMousePosition(), 15f, selectedUnits.Count);
            break;
          case "cross":
            positionsAvailable = CalculateCrossShapeFormation(GetGlobalMousePosition(), 15f, selectedUnits.Count);
            break;
          case "snake":
            positionsAvailable = CalculateSnakeFormation(GetGlobalMousePosition(), 15f, selectedUnits.Count);
            break;
          case "random":
            positionsAvailable = CalculateRandomFormation(GetGlobalMousePosition(), 15f, selectedUnits.Count);
            break;
          case "crescent":
            positionsAvailable = CalculateCrescentShapeFormation(GetGlobalMousePosition(), 20f, 10f, selectedUnits.Count);
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
