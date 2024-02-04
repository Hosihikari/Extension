using Hosihikari.Minecraft.Extension.FormUI.Element;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hosihikari.Minecraft.Extension.FormUI;

public sealed class SimpleFormCallbackEventArgs(Player player, int chosen) : EventArgs
{
    public Player Player { get; } = player;

    public int Chosen { get; } = chosen;
}

public sealed class SimpleForm : FormBase
{
    private string _content = string.Empty;
    private FormElementCollection<SimpleFormElement> _elements;

    private string _title = string.Empty;

    public SimpleForm()
    {
        _elements = [];
        _elements.Changed += OnCollectionChanged;
    }

    [JsonPropertyName("title")]
    public string Title
    {
        get => _title;
        set
        {
            _title = value;
            OnPropertyChanged(nameof(Title));
        }
    }

    [JsonPropertyName("content")]
    public string Content
    {
        get => _content;
        set
        {
            _content = value;
            OnPropertyChanged(nameof(Content));
        }
    }

    [JsonPropertyName("buttons")]
    internal List<string> SerializedButtons
    {
        get
        {
            List<string> buttons = new(_elements.Count);
            buttons.AddRange(_elements.OfType<Button>().Select(element => element.SerializedData));
            return buttons;
        }
        set => throw new NotSupportedException();
    }

    [JsonPropertyName("type")] public string Type { get; private set; } = "form";

    [JsonIgnore]
    public FormElementCollection<SimpleFormElement> Elements
    {
        get => _elements;
        set
        {
            _elements.Changed -= OnCollectionChanged;
            _elements = value;
            _elements.Changed += OnCollectionChanged;
            OnPropertyChanged(nameof(Elements));
        }
    }


    protected override string Serialize()
    {
        return JsonSerializer.Serialize(this);
    }

    private void OnCollectionChanged(object? sender, EventArgs args)
    {
        OnPropertyChanged(nameof(Elements));
    }

    public SimpleForm Append(SimpleFormElement element)
    {
        _elements.Add(element);
        return this;
    }

    public void Remove(SimpleFormElement element)
    {
        _elements.Remove(element);
    }

    public event EventHandler<SimpleFormCallbackEventArgs>? Callback;

    internal void InvokeCallback(Player player, int chosen)
    {
        Callback?.Invoke(this, new(player, chosen));
    }
}