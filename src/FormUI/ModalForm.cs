using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hosihikari.Minecraft.Extension.FormUI;

public sealed class ModalForm : FormBase
{
    [Flags]
    public enum Chosen
    {
        Cancel,
        Confirm
    }

    private string _cancelButton = string.Empty;

    private string _confirmButton = string.Empty;

    private string _content = string.Empty;
    private string _title = string.Empty;

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

    [JsonPropertyName("button1")]
    public string ConfirmButton
    {
        get => _confirmButton;
        set
        {
            _confirmButton = value;
            OnPropertyChanged(nameof(ConfirmButton));
        }
    }

    [JsonPropertyName("button2")]
    public string CancelButton
    {
        get => _cancelButton;
        set
        {
            _cancelButton = value;
            OnPropertyChanged(nameof(CancelButton));
        }
    }

    [JsonPropertyName("type")] public string Type { get; private set; } = "modal";


    protected override string Serialize()
    {
        return JsonSerializer.Serialize(this);
    }

    public event Action<Player, Chosen>? Callback;

    internal void InvokeCallback(Player player, Chosen chosen)
    {
        Callback?.Invoke(player, chosen);
    }
}