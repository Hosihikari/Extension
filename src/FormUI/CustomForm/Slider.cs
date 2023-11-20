using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hosihikari.FormUI;

public class Slider : CustomFormElement
{

    [JsonIgnore]
    public override ElementType FormElementType => ElementType.Slider;

    protected override string Serialize() => JsonSerializer.Serialize(this);

    private string title = string.Empty;

    private double min = 0;

    private double max = 0;

    private double step = 0;

    private double @default = 0;

    public Slider(string name) : base(name)
    {
    }

    //[JsonPropertyName("type")]
    //public string Type { get; private set; } = "slider";

    [JsonPropertyName("text")]
    public string Title
    {
        get => title;
        set
        {
            title = value;
            OnPropertyChanged(nameof(Title));
        }
    }

    [JsonPropertyName("min")]
    public double Min
    {
        get => min;
        set
        {
            min = value;
            if (min > max)
                (min, max) = (max, min);
            OnPropertyChanged(nameof(Min));
        }
    }

    [JsonPropertyName("max")]
    public double Max
    {
        get => max;
        set
        {
            max = value;
            if (min > max)
                (min, max) = (max, min);
            OnPropertyChanged(nameof(Max));
        }
    }

    [JsonPropertyName("step")]
    public double Step
    {
        get => step;
        set
        {
            step = value > 0 ? value : max - min;
            OnPropertyChanged(nameof(Step));
        }
    }

    [JsonPropertyName("default")]
    public double Default
    {
        get => @default;
        set
        {
            @default = value;
            @default = Math.Max(Math.Min(@default, max), min);
            OnPropertyChanged(nameof(Default));
        }
    }
}
