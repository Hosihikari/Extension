using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Hosihikari.Minecraft.Extension.FormUI;

public abstract class FormElementBase
{

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

    protected abstract string Serialize();

    internal FormBase? Form { get; set; }

    public event EventHandler<PropertyChangedEventArgs>? PropertyChanged;


    private bool _onHandlingPropertyChanged;
    public void OnPropertyChanged(string propertyName = "")
    {
        if (_onHandlingPropertyChanged)
        {
            return;
        }

        _onHandlingPropertyChanged = true;

        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        if (Form is not null)
            Form.IsSerialized = false;

        _onHandlingPropertyChanged = false;

    }
}
