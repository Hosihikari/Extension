using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hosihikari.Minecraft.Extension.FormUI.Element;

public sealed class Slider(string name) : CustomFormElement(name)
{

    [JsonIgnore]
    public override ElementType FormElementType => ElementType.Slider;

    protected override string Serialize() => JsonSerializer.Serialize(this);

    private string _title = string.Empty;

    private double _min;

    private double _max;

    private double _step;

    private double _default;

    //[JsonPropertyName("type")]
    //public string Type { get; private set; } = "slider";

    [JsonPropertyName("text")]
    public string Title
    {
        get => _title;
        set
        {
            _title = value;
            OnPropertyChanged(nameof(Title));
        }
    }

    [JsonPropertyName("min")]
    public double Min
    {
        get => _min;
        set
        {
            _min = value;
            if (_min > _max)
                (_min, _max) = (_max, _min);
            OnPropertyChanged(nameof(Min));
        }
    }

    [JsonPropertyName("max")]
    public double Max
    {
        get => _max;
        set
        {
            _max = value;
            if (_min > _max)
                (_min, _max) = (_max, _min);
            OnPropertyChanged(nameof(Max));
        }
    }

    [JsonPropertyName("step")]
    public double Step
    {
        get => _step;
        set
        {
            _step = value > 0 ? value : _max - _min;
            OnPropertyChanged(nameof(Step));
        }
    }

    [JsonPropertyName("default")]
    public double Default
    {
        get => _default;
        set
        {
            _default = value;
            _default = Math.Max(Math.Min(_default, _max), _min);
            OnPropertyChanged(nameof(Default));
        }
    }
}
