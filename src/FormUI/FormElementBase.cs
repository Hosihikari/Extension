using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Hosihikari.FormUI;

public abstract class FormElementBase
{

    private string? serializedData;

    [JsonIgnore]
    public string SerializedData
    {
        get
        {
            if (IsSerialized is false)
            {
                serializedData = Serialize();
                IsSerialized = true;
            }
            return serializedData;
        }
    }

    [JsonIgnore]
    [MemberNotNullWhen(true, nameof(serializedData))]
    public bool IsSerialized { get; internal set; }

    protected abstract string Serialize();

#nullable enable
    internal FormBase? Form { get; set; }

    public event EventHandler<PropertyChangedEventArgs>? PropertyChanged;


    private bool onHandlingPropertyChanged = false;
    public void OnPropertyChanged(string propertyName = "")
    {
        if (onHandlingPropertyChanged is false)
        {
            onHandlingPropertyChanged = true;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            if (Form is not null)
                Form.IsSerialized = false;

            onHandlingPropertyChanged = false;
        }

    }
}
