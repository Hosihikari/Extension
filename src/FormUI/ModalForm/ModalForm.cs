using Hosihikari.Minecraft;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hosihikari.FormUI;

public class ModalForm : FormBase
{
    private string title = string.Empty;

    private string content = string.Empty;

    private string confirmButton = string.Empty;

    private string cancelButton = string.Empty;

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

    [JsonPropertyName("button1")]
    public string ConfirmButton
    {
        get => confirmButton;
        set
        {
            confirmButton = value;
            OnPropertyChanged(nameof(ConfirmButton));
        }
    }

    [JsonPropertyName("button2")]
    public string CancelButton
    {
        get => cancelButton;
        set
        {
            cancelButton = value;
            OnPropertyChanged(nameof(CancelButton));
        }
    }

    [JsonPropertyName("type")]
    public string Type { get; private set; } = "modal";


    protected override string Serialize() => JsonSerializer.Serialize(this);

    [Flags]
    public enum Chosen { Cancel, Confirm }

#nullable enable
    public event Action<Player, Chosen>? Callback;
#nullable disable

    internal void InvokeCallback(Player player, Chosen chosen)
    => Callback?.Invoke(player, chosen);
}
