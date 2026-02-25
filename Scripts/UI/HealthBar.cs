namespace CosmicDoom.Scripts.UI;

using Godot;

public partial class HealthBar : Control {
    [Export] public Color EmptyColor = Colors.Red;
    [Export] public Color HalfwayColor = Colors.Yellow;
    [Export] public Color FullColor = Colors.Green;
    private ColorRect _fill;
    private Label _label;

    private const float BarWidth = 200f;
    private const float Margin = 20f;

    public override void _Ready() {
        _fill = GetNode<ColorRect>("Fill");
        _fill.Color = FullColor;
        _label = GetNode<Label>("Label");
    }

    public void SetHealth(int health, int maxHealth) {
        var ratio = Mathf.Clamp((float)health / maxHealth, 0f, 1f);

        _fill.OffsetRight = Margin + BarWidth * ratio;

        _fill.Color = ratio > 0.5f
            ? FullColor.Lerp(HalfwayColor, (1f - ratio) * 2f)
            : HalfwayColor.Lerp(EmptyColor, (0.5f - ratio) * 2f);

        _label.Text = health.ToString();
    }
}
