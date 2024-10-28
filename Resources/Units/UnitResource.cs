using Godot;

[GlobalClass]
public partial class UnitResource : Resource
{
  [Export]
  public Texture2D texture;
  [Export]
  public float speed = 50f;
  [Export]
  public int damage = 10;
  [Export]
  public int health = 100;
  [Export]
  public int maxHealth = 100;
  [Export]
  public int attackRange = 1;
  [Export]
  public int attackDelay = 1;
  [Export]
  public int defense = 1;
}
