using Godot;
using System;
using System.ComponentModel.Design;

public partial class Player : CharacterBody3D
{
    [Export] public float Speed = 7.5f;
    [Export] public float MouseSensitivity = 0.002f;
    [Export] public float VerticalLimit = 50.0f;

    private Vector3 _direction = Vector3.Zero;
    [Export] private SpringArm3D _camera;
    private float _rotationX = 0.0f;
    private float gravity = 15.5f;
    private float jumpPower = 5.0f;
    

    public override void _Ready()
    {

        Input.MouseMode = Input.MouseModeEnum.Captured;

    }

    public override void _UnhandledInput(InputEvent @event)
    {

        if (@event is InputEventKey keyEvent && keyEvent.Pressed && keyEvent.Keycode == Key.Escape)
        {
            Input.MouseMode = Input.MouseModeEnum.Visible;
        }


        if (@event is InputEventMouseMotion motion && Input.MouseMode == Input.MouseModeEnum.Captured)
        {

            RotateY(-motion.Relative.X * MouseSensitivity);


            _rotationX = Mathf.Clamp(_rotationX - motion.Relative.Y * MouseSensitivity, Mathf.DegToRad(-VerticalLimit), Mathf.DegToRad(VerticalLimit / 4));
            _camera.Rotation = new Vector3(_rotationX, _camera.Rotation.Y, _camera.Rotation.Z);
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        _direction = Vector3.Zero;

        // input movement
        if (Input.IsActionPressed("forward"))
            _direction.Z -= 1;
        if (Input.IsActionPressed("backward"))
            _direction.Z += 1;
        if (Input.IsActionPressed("left"))
            _direction.X -= 1;
        if (Input.IsActionPressed("right"))
            _direction.X += 1;


            // normalize direction so diagonal movement isn’t faster
            _direction = _direction.Normalized();

        // convert input direction to world space
        Vector3 basisX = Basis.X;
        Vector3 basisZ = Basis.Z;
        basisX.Y = 0;
        basisZ.Y = 0;
        basisX = basisX.Normalized();
        basisZ = basisZ.Normalized();
        Vector3 moveDirection = basisX * _direction.X + basisZ * _direction.Z;

        // grab existing velocity
        Vector3 velocity = Velocity;

        // horizontal movement
       
        
        velocity.X = moveDirection.X * Speed;
        velocity.Z = moveDirection.Z * Speed;
        

        // gravity
        if (IsOnFloor())
        {
            if (Input.IsActionPressed("jump"))
            {
                velocity.Y = jumpPower; // or whatever your jump strength is
            }
            else
            {
                velocity.Y = 0;
            }
        }
        else
        {
            velocity.Y -= gravity * (float)delta;
        }

        // apply final velocity
        Velocity = velocity;
        MoveAndSlide();
    }
}
    