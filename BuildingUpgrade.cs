using Godot;
using System;

public partial class BuildingUpgrade : Node
{
	[ExportGroup("Labels and Buttons")]
	[Export]
	Label buildingNameLabel;
	[Export]
	Label amountOwnedLabel;
	[Export]
	Label resourcesPerSecondLabel;
	[Export]
	Label totalResourcesPerSecondLabel;
	[Export]
	Button buyOneButton;
	[Export]
	Button sellOneButton;

	[ExportGroup("Money")]
	[Export]
	int baseCost = 1;
	[Export(PropertyHint.Range, "1.0,2.0,0.05")]
	float costMultiplier = 1;
	[Export(PropertyHint.Range, "0.0,1.0,0.05")]
	float sellPenaltyMultiplier = 0.5f;
	[Export]
	float baseMoneyPerSecond = 0.0f;
	float moneyPerSecondMultiplier = 1.0f;

	int amountOwned = 0;
	int costToBuyOne = 0;
	int priceOfSellOne = 0;
	public float totalResourcesPerSecond { get; private set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Buttons
		buyOneButton.Pressed += _HandleBuyOne;

		sellOneButton.Pressed += _HandleSellOne;

		_CalculateCostsAndProfits();
		_RewriteLabels();
	}

	private void _CalculateCostsAndProfits()
	{
		if (amountOwned == 0)
			priceOfSellOne = 0;
		else
			priceOfSellOne = (int)Math.Round(baseCost * Math.Pow(costMultiplier, amountOwned - 1) * sellPenaltyMultiplier);
		costToBuyOne = (int)Math.Round(baseCost * Math.Pow(costMultiplier, amountOwned));

		totalResourcesPerSecond = baseMoneyPerSecond * moneyPerSecondMultiplier * amountOwned;
		if (MainGameScreen.gameInstance != null)
			MainGameScreen.gameInstance._RecalculateMoneyPerSecond();
	}

	private void _RewriteLabels()
	{
		amountOwnedLabel.Text = "Owned: " + amountOwned;
		resourcesPerSecondLabel.Text = "+$" + (baseMoneyPerSecond * moneyPerSecondMultiplier).ToString("0.#") + "/s";
		totalResourcesPerSecondLabel.Text = "+Total: $" + totalResourcesPerSecond.ToString("0.#") + "/s";
		buyOneButton.Text = "1 ($" + costToBuyOne + ")";
		sellOneButton.Text = "1 ($" + priceOfSellOne + ")";
	}

	public void _HandleBuyOne()
	{
		if (MainGameScreen.gameInstance._SpendMoney(costToBuyOne))
		{
			amountOwned += 1;
			_CalculateCostsAndProfits();
			_RewriteLabels();
		}
	}

	public void _HandleSellOne()
	{
		if (amountOwned >= 1)
		{
			amountOwned -= 1;
			MainGameScreen.gameInstance._GiveMoney(priceOfSellOne);
			_CalculateCostsAndProfits();
			_RewriteLabels();
		}
	}
}
