using System.Collections.Generic;
using System.Linq;
using Godot;
using System;

public class MiniGame : Node
{
	[Signal]
	protected delegate void Win();

	public void SetPauseChildren(bool b)
	{
		List<Node> childs = GetAllChilds(this);

		childs.ForEach((c) => PauseNode(c, b));
	}

	private void PauseNode(Node node, bool isPaused)
	{
		GD.Print("Pause ()" + Name);
		node.SetProcess(!isPaused);
		node.SetPhysicsProcess(!isPaused);

		if (node is AnimatedSprite)
		{
			AnimatedSprite animSprite = (AnimatedSprite)node;
			if (isPaused)
				animSprite.Stop();
			else
				animSprite.Play();
		}
	}

	private List<Node> GetAllChilds(Node node, List<Node> childs = null)
	{
		if (childs == null)
			childs = new List<Node>();

		childs.Add(node);
		foreach (Node child in node.GetChildren())
			childs = GetAllChilds(child, childs);
		return (childs);
	}
}
