using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Hosihikari.Minecraft.Extension.FormUI;

public abstract class FormElementBase
{
    private bool _onHandlingPropertyChanged;

    private string? _serializedData;

    [JsonIgnore]
    public string SerializedData
    {
        get
        {
            if (IsSerialized)
            {
                return _serializedData;
            }

            _serializedData = Serialize();
            IsSerialized = true;
            return _serializedData;
        }
    }

    [JsonIgnore]
    [MemberNotNullWhen(true, nameof(_serializedData))]
    public bool IsSerialized { get; internal set; }

    internal FormBase? Form { get; set; }

    protected abstract string Serialize();

    public event EventHandler<PropertyChangedEventArgs>? PropertyChanged;

    public void OnPropertyChanged(string propertyName = "")
    {
        if (_onHandlingPropertyChanged)
        {
            return;
        }

        _onHandlingPropertyChanged = true;

        PropertyChanged?.Invoke(this, new(propertyName));
        if (Form is not null)
        {
            Form.IsSerialized = false;
        }

        _onHandlingPropertyChanged = false;
    }
}