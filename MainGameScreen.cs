using Godot;
using System;

public partial class MainGameScreen : Node2D
{
	
	double resourceCount = 0;
	
	// Temporary variables
	double resourcesPerSecond = 0;
	int tempBuildingsOwned = 0;
	Label tempBuildingLabel;
	
	Label resourceText;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		resourceText = GetNode<Label>("ResourceLabel");
		resourceText.Text = "Money: $" + resourceCount;
		
		tempBuildingLabel = GetNode<Label>("TempBuildingLabel");
		tempBuildingLabel.Text = "Temp Buildings Owned: " + tempBuildingsOwned;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		resourceCount += delta * resourcesPerSecond;
		_SetMoneyText();
	}
	
	public void _on_big_button_pressed()
	{
		resourceCount++;
		//GD.Print(resourceCount);
	}
	
	public void _on_quit_button_pressed()
	{
		GetTree().Quit();
	}
	
	public void _on_temp_building_button_pressed()
	{
		if (resourceCount >= 10)
		{
			resourceCount -= 10;
			tempBuildingsOwned += 1;
			resourcesPerSecond += 0.1;
			
			tempBuildingLabel.Text = "Temp Buildings Owned: " + tempBuildingsOwned;
		}
	}

	private void _SetMoneyText()
	{
		if (resourceText != null)
			resourceText.Text = "Money: $" + resourceCount.ToString("0.#");
	}
}
