using Godot;
using System;
using System.Linq;

public partial class MainGameScreen : Node2D
{
	public static MainGameScreen gameInstance;
	double resourceCount = 0;
	
	// Temporary variables
	double resourcesPerSecond = 0;
	int tempBuildingsOwned = 0;
	Label tempBuildingLabel;
	
	Label resourceText;

	float moneyPerClick = 1;

	[Export]
	BuildingUpgrade[] allBuildings;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (gameInstance == null)
			gameInstance = this;

		resourceText = GetNode<Label>("ResourceLabel");
		resourceText.Text = "Money: $" + resourceCount;

		_RecalculateMoneyPerSecond();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		resourceCount += delta * resourcesPerSecond;
		_SetMoneyText();
	}
	
	public void _on_big_button_pressed()
	{
		resourceCount += moneyPerClick;
	}
	
	public void _on_quit_button_pressed()
	{
		GetTree().Quit();
	}

	private void _SetMoneyText()
	{
		if (resourceText != null)
			resourceText.Text = "Money: $" + resourceCount.ToString("0.#");
	}

	public double _GetCurrentMoney()
	{
		return resourceCount;
	}

	public bool _SpendMoney(double amount)
	{
		if (amount <= resourceCount)
		{
			resourceCount -= amount;
			return true;
		}
		else
			return false;
	}

	public void _GiveMoney(double amount)
	{
		resourceCount += amount;
	}

	public void _RecalculateMoneyPerSecond()
	{
		resourcesPerSecond = 0.0;
		for (int i = 0; i < allBuildings.Length; i++)
		{
			resourcesPerSecond += allBuildings[i].totalResourcesPerSecond;
		}
	}
}
