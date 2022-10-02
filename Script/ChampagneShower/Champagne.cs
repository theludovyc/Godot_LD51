using Godot;
using System;

public class Champagne : KinematicBody2D
{
	[Export] float champagneHSpeed = 2f;
	[Export] float champagneVSpeed = 2f;
	[Export] float angleOffset = 10f;

	private Vector2 startPos = Vector2.Zero;

    public override void _Ready()
    {
        startPos = Position;
    }

	private float GetVerticalInputValue()
	{
		if (Position.x < 0)
			return (0);
		float input = 0;

		if (Input.IsActionPressed("game_down"))
			input += 1;
		if (Input.IsActionPressed("game_up"))
			input -= 1;
		
		return (input);
	}

	public override void _Process(float delta)
	{
		Move(delta);
	}

	private void Move(float delta)
	{
		Vector2 velocity = Vector2.Zero;
		float vInput = GetVerticalInputValue();

		velocity.x = champagneHSpeed;
		velocity.y = champagneVSpeed * vInput;
		KinematicCollision2D collison = MoveAndCollide(velocity * delta);

		if (collison != null)
			OnCollide();

		RotationDegrees = angleOffset * vInput;
	}

	private void OnCollide()
	{
		Position = startPos;
	}
}
