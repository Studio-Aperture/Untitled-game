using Godot;
using System;

public partial class Nibbler : CharacterBody3D
{
    [Export] private Area3D detectionRadius;
    [Export] private float turnSpeed = 10f;
    private CharacterBody3D target;
    private float speed = 8.5f;
    private float gravity = 15.5f;
    private float verticalVelocity = 0.0f;


    public override void _Ready()
    {
        detectionRadius = GetNode<Area3D>("radius");  // node named "radius"
        detectionRadius.BodyEntered += OnBodyEntered;
        detectionRadius.BodyExited += OnBodyExited;

        // Don't assign target here, wait until detection
    }

    private void OnBodyEntered(Node body)
    {
        if (body.Name == "player")  // match exactly your node's name
        {
            GD.Print("Player detected!");
            target = (CharacterBody3D)body;
        }
    }

    private void OnBodyExited(Node body)
    {
        if (body == target)
        {
            GD.Print("Player lost!");
            target = null;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (target != null)
        {
            Vector3 direction = (target.GlobalTransform.Origin - GlobalTransform.Origin).Normalized();
            direction.Y = 0;  // Only rotate horizontally

            float targetRotationY = Mathf.Atan2(direction.X, direction.Z);
            Vector3 currentRotation = Rotation;
            currentRotation.Y = Mathf.LerpAngle(currentRotation.Y, targetRotationY, turnSpeed * (float)delta);
            Rotation = currentRotation;

            // Gravity
            if (IsOnFloor())
            {
                verticalVelocity = 0.0f;
                speed = 8.5f;
            }
            else
            {
                speed = 10f;
                verticalVelocity -= gravity * (float)delta;
            }

            // Final velocity
            Vector3 velocity = direction * speed;
            velocity.Y = verticalVelocity;

            Velocity = velocity;
            MoveAndSlide();
        } 
    }
}