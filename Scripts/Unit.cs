using Godot;
using System;

public partial class Unit : Node2D
{
  [Export] Texture2D selectedTexture;
  [Export] Texture2D shadowTexture;
  [Export] float speed = 100f;
  Vector2 movePosition = Vector2.Zero;
  Sprite2D shadow;
  Sprite2D sprite;

  public override void _Ready()
  {
    shadow = GetNode<Sprite2D>("shadow");
    sprite = GetNode<Sprite2D>("Sprite");
    shadow.Texture = shadowTexture;
  }

  public override void _Process(double delta)
  {
    if (movePosition == Vector2.Zero) return;
    Vector2 moveDir = (movePosition - Position).Normalized();
    if (Position.DistanceTo(movePosition) < 1f)
    {
      movePosition = Vector2.Zero;
    }
    else
    {
      Position += moveDir * speed * (float)delta;
    }
  }

  public void SetMovePosition(Vector2 position) => movePosition = position;
  public void SetSelected(bool selected)
  {
    shadow.Texture = selected ? selectedTexture : shadowTexture;
  }
}
