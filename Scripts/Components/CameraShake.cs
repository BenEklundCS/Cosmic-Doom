namespace CosmicDoom.Scripts.Components;

using Godot;

public partial class CameraShake : Camera3D {
    private Vector3 _originalPosition;
    private float _shakeIntensity = 0f;
    private float _shakeDuration = 0f;
    private float _shakeElapsed = 0f;

    public override void _Ready() {
        _originalPosition = Position;
    }

    public override void _PhysicsProcess(double delta) {
        if (_shakeElapsed >= _shakeDuration) {
            _shakeIntensity = 0f;
            Position = _originalPosition;
            return;
        }

        _shakeElapsed += (float)delta;

        // Oscillate using sine wave
        var frequency = 10f; // Shake frequency in Hz
        var shakeAmount = Mathf.Sin(_shakeElapsed * frequency * Mathf.Tau) * _shakeIntensity;

        var newPos = _originalPosition;
        newPos.X += ((float)GD.Randf() * 2f - 1f) * shakeAmount;
        newPos.Y += ((float)GD.Randf() * 2f - 1f) * shakeAmount;
        newPos.Z += ((float)GD.Randf() * 2f - 1f) * shakeAmount;

        Position = newPos;
    }

    public void Shake(float intensity, float duration) {
        _shakeIntensity = intensity;
        _shakeDuration = duration;
        _shakeElapsed = 0f;
        _originalPosition = Position;
    }
}