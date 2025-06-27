using Godot;
using System;

public partial class UI : Control
{
    [Export] public TextureProgressBar HealthFill;
    [Export] public float MaxHealth = 100f;
    [Export] public float RegenRate = 0.5f; // HP per second

    private float currentHealth;

    public override void _Ready()
    {
        currentHealth = MaxHealth;
        UpdateHealthBar();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Input.IsActionPressed("jump"))
            RemoveHealth(100); // For testing
        else
            RegenHealth((float)delta);
    }

    public void RemoveHealth(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth - amount, 0, MaxHealth);
        UpdateHealthBar();
    }

    private void RegenHealth(float delta)
    {
        currentHealth = Mathf.Clamp(currentHealth + RegenRate * delta, 0, MaxHealth);
        UpdateHealthBar();
        GD.Print(currentHealth);
    }

    private void UpdateHealthBar()
    {
        HealthFill.MaxValue = MaxHealth;
        HealthFill.Value = currentHealth;
    }
}
