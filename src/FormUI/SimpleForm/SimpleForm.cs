using Hosihikari.Minecraft;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hosihikari.FormUI;

public class SimpleFormCallbackEventArgs : EventArgs
{
    private readonly Player player;
    private readonly int chosen;

    public Player Player => player;

    public int Chosen => chosen;

    public SimpleFormCallbackEventArgs(Player player, int chosen)
    {
        this.player = player;
        this.chosen = chosen;
    }
}

public class SimpleForm : FormBase
{
    private FormElementCollection<SimpleFormElement> elements;

    private string title = string.Empty;

    private string content = string.Empty;

    public SimpleForm()
    {
        elements = new();
        elements.Changed += OnCollectionChanged;
    }

    [JsonPropertyName("title")]
    public string Title
    {
        get => title;
        set
        {
            title = value;
            OnPropertyChanged(nameof(Title));
        }
    }

    [JsonPropertyName("content")]
    public string Content
    {
        get => content;
        set
        {
            content = value;
            OnPropertyChanged(nameof(Content));
        }
    }

    [JsonPropertyName("buttons")]
    internal List<string> SerializedButtons
    {
        get
        {
            var buttons = new List<string>(elements.Count);
            foreach (var element in elements)
            {
                if (element is Button)
                {
                    buttons.Add(element.SerializedData);
                }
            }
            return buttons;
        }
        set => throw new NotSupportedException();
    }

    [JsonPropertyName("type")]
    public string Type { get; private set; } = "form";



    protected override string Serialize() => JsonSerializer.Serialize(this);

    private void OnCollectionChanged(object? sender, EventArgs args) => OnPropertyChanged(nameof(Elements));

    [JsonIgnore]
    public FormElementCollection<SimpleFormElement> Elements
    {
        get => elements;
        set
        {
            elements.Changed -= OnCollectionChanged;
            elements = value;
            elements.Changed += OnCollectionChanged;
            OnPropertyChanged(nameof(Elements));
        }
    }

    public SimpleForm Append(SimpleFormElement element)
    {
        elements.Add(element);
        return this;
    }

    public void Remove(SimpleFormElement element)
        => elements.Remove(element);

#nullable enable
    public event EventHandler<SimpleFormCallbackEventArgs>? Callback;

    internal void InvokeCallback(Player player, int chosen)
        => Callback?.Invoke(this, new(player, chosen));
}
