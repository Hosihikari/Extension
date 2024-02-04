using Hosihikari.Minecraft.Extension.Events;
using Hosihikari.Minecraft.Extension.FormUI.Element;
using Hosihikari.NativeInterop.Unmanaged;
using Hosihikari.NativeInterop.Unmanaged.STL;
using System.Text.Json;

namespace Hosihikari.Minecraft.Extension.FormUI;

public sealed class FormResponseEventArgs : EventArgsBase
{
    internal FormResponseEventArgs(Player player, uint formId, string data)
    {
        Player = player;
        FormId = formId;
        Data = data;
    }

    public Player Player { get; }
    public uint FormId { get; }
    public string Data { get; }
}

public sealed unsafe class FormResponseEvent() : HookEventBase<FormResponseEventArgs, FormResponseEvent.HookDelegate>(
    "?handle@?$PacketHandlerDispatcherInstance@VModalFormResponsePacket@@$0A@@@UEBAXAEBVNetworkIdentifier@@AEAVNetEventCallback@@AEAV?$shared_ptr@VPacket@@@std@@@Z")
{
    // private readonly string PacketHandlerDispatcherInstanceHandleSymbol = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ?
    //     "?handle@?$PacketHandlerDispatcherInstance@VModalFormResponsePacket@@$0A@@@UEBAXAEBVNetworkIdentifier@@AEAVNetEventCallback@@AEAV?$shared_ptr@VPacket@@@std@@@Z" :
    //     throw new NotImplementedException();

    public delegate void HookDelegate(
        void* @this,
        Pointer<NetworkIdentifier> networkIdentifier,
        Pointer<ServerNetworkHandler> handler,
        CxxSharedPtr sharedPtrPacket);

    public override HookDelegate HookedFunc =>
        (@this, networkIdentifier, pointer, sharedPtr) =>
        {
            using NetworkIdentifier identifier = networkIdentifier.Target;
            using ServerNetworkHandler handler = pointer.Target;
            Pointer<ServerPlayer> player = handler._getServerPlayer(identifier, default);
            Pointer<Packet> packet = (Pointer<Packet>)(nint)sharedPtr.ptr;

            if (player == nint.Zero)
            {
                return;
            }

            string? data = null;

            int id = Memory.DAccess<int>(packet, 48);
            if (Memory.DAccess<NativeBoolean>(packet, 81))
            {
                if (Memory.DAccess<NativeBoolean>(packet, 72))
                {
                    using Json.Value json = Memory.DAccessAsReference<Json.Value>(packet, 56).Target;
                    using StdString cppstr = json.ToStyledString();
                    data = cppstr.ToString();
                }
            }

            if (string.IsNullOrWhiteSpace(data))
            {
                data = "null";
            }

            if (data.EndsWith('\n'))
            {
                data = data[..^1];
            }

            FormResponseEventArgs e = new(player.As<Player>().Target, (uint)id, data);

            OnEventBefore(e);
            Original(@this, identifier, handler, sharedPtr);
            OnEventAfter(e);
        };
}

public static unsafe class FormHandler
{
    private static readonly List<uint> s_formIds = [];

    private static readonly Dictionary<uint, FormBase> s_forms = new();

    private static readonly System.Random s_random = new();

    static FormHandler()
    {
        //Events.FormResponse.Before += HandleFormPacket;
    }

    private static uint NewFormId()
    {
        uint id;
        do
        {
            id = unchecked((uint)unchecked((s_random.Next() << 16) + s_random.Next()));
        } while (s_formIds.Contains(id));

        return id;
    }

    private static void HandleFormPacket(object? sender, FormResponseEventArgs e)
    {
        Player player = e.Player;
        uint id = e.FormId;
        string data = e.Data;

        if (s_forms.TryGetValue(id, out FormBase? form) is false)
        {
            return;
        }

        switch (form)
        {
            case SimpleForm simpleForm:
                {
                    int chosen = data is "null" ? -1 : int.Parse(data);
                    simpleForm.InvokeCallback(player, chosen);

                    if (chosen >= 0)
                    {
                        Button button = (Button)simpleForm.Elements[chosen];
                        button.InvokeOnClicked(player);
                    }

                    s_forms.Remove(id);
                    break;
                }
            case ModalForm modalForm:
                {
                    ModalForm.Chosen chosen = data is "true" ? ModalForm.Chosen.Confirm : ModalForm.Chosen.Cancel;
                    modalForm.InvokeCallback(player, chosen);

                    s_forms.Remove(id);
                    break;
                }
            case CustomForm customForm:
                {
                    JsonDocument doc = JsonDocument.Parse(data);
                    int index = 0;
                    foreach (JsonElement json in doc.RootElement.EnumerateArray())
                    {
                        CustomFormElement element = customForm.Elements[index].element;
                        switch (element.FormElementType)
                        {
                            case CustomFormElement.ElementType.Label:
                                break;

                            case CustomFormElement.ElementType.Input:
                            case CustomFormElement.ElementType.Toggle:
                            case CustomFormElement.ElementType.Slider:
                                element.Value = json.GetRawText();
                                element.InvokeValueChanged();
                                break;

                            case CustomFormElement.ElementType.Dropdown:
                                {
                                    FormElementCollection<string> options = (element as Dropdown)!.Options;
                                    element.Value = options[json.GetInt32()];
                                    element.InvokeValueChanged();
                                }
                                break;
                            case CustomFormElement.ElementType.StepSlider:
                                {
                                    FormElementCollection<string> options = (element as StepSlider)!.Options;
                                    element.Value = options[json.GetInt32()];
                                    element.InvokeValueChanged();
                                }
                                break;
                        }

                        ++index;
                    }

                    if (customForm.IsNullCallback is false)
                    {
                        Dictionary<string, CustomFormElement> elements = new Dictionary<string, CustomFormElement>();
                        foreach ((string k, CustomFormElement v) in customForm.Elements)
                        {
                            elements.Add(k, v);
                        }

                        customForm.InvokeCallback(elements, player);
                    }

                    s_forms.Remove(id);
                    break;
                }
        }
    }

    public static void SendTo(this FormBase form, Player player)
    {
        uint id = NewFormId();
        s_forms.Add(id, form);
        using BinaryStream bs = new(sizeof(void*) + sizeof(StdString));

        using StdString data = new(form.SerializedData);

        fixed (byte* ptr = data.Data)
        {
            bs.WriteUnsignedVarInt(id, default, default);
            bs.WriteString(new() { ptr = ptr, length = data.Length }, default, default);
        }

        CxxSharedPtr sharedPtrPacket = MinecraftPackets.CreatePacket(MinecraftPacketIds.ShowModalForm);
        HeapAlloc.Delete(sharedPtrPacket.ctr);
        Packet packet = Packet.ConstructInstance((nint)sharedPtrPacket.ptr, false, false);
        packet.Read((Reference<ReadOnlyBinaryStream>)(nint)bs).Drop();

        player.SendNetworkPacket(packet);

        CppTypeSystem.GetVTable<Packet.Vftable>(packet)->vfptr_write(packet, default);
        HeapAlloc.Delete(packet);
    }

    //public static bool SendTo(this SimpleForm form, Player player)
    //{
    //    //var id = NewFormId();
    //    //forms.Add(id, form);

    //    var buttons = new List<string>();
    //    var images = new List<string>();

    //    foreach (var ele in form.Elements)
    //    {
    //        var element = ele as Button;
    //        buttons.Add(element!.Text);
    //        images.Add(element!.Image);
    //    }

    //    return player.SendSimpleForm(form.Title, form.Content, buttons, images, new((_, val) => { form.InvokeCallback(player, val); }));
    //}

    //public static bool SendTo(this ModalForm form, Player player)
    //{
    //    return player.SendModalForm(form.Title, form.Content, form.ConfirmButton, form.CancelButton, new((_, val) => { form.InvokeCallback(player, val != 0 ? ModalForm.Chosen.Confirm : ModalForm.Chosen.Cancel); }));
    //}

    //public unsafe static bool SendTo(this CustomForm form, Player player)
    //{
    //    //var id = NewFormId();
    //    //forms.Add(id, form);
    //    //return player.SendRawFormPacket(id, form.SerializedData);
    //    return player.SendCustomForm(form.SerializedData, new((_, val) =>
    //    {
    //        var cppString = val.Get();
    //        var doc = JsonDocument.Parse(cppString.ToString());
    //        cppString.Dispose();

    //        int index = 0;
    //        foreach (var json in doc.RootElement.EnumerateArray())
    //        {
    //            var element = form.Elements[index].element;
    //            switch (element.FormElementType)
    //            {
    //                case CustomFormElement.ElementType.Label:
    //                    break;

    //                case CustomFormElement.ElementType.Input:
    //                case CustomFormElement.ElementType.Toggle:
    //                case CustomFormElement.ElementType.Slider:
    //                    element.Value = json.GetString();
    //                    element.InvokeValueChanged();
    //                    break;

    //                case CustomFormElement.ElementType.Dropdown:
    //                    {
    //                        var options = (element as Dropdown)!.Options;
    //                        element.Value = options[json.GetInt32()];
    //                        element.InvokeValueChanged();
    //                    }
    //                    break;
    //                case CustomFormElement.ElementType.StepSlider:
    //                    {
    //                        var options = (element as StepSlider)!.Options;
    //                        element.Value = options[json.GetInt32()];
    //                        element.InvokeValueChanged();
    //                    }
    //                    break;
    //            }
    //            ++index;
    //        }

    //        if (form.IsNullCallback is false)
    //        {
    //            var elements = new Dictionary<string, CustomFormElement>();
    //            foreach (var (k, v) in form.Elements)
    //                elements.Add(k, v);

    //            form.InvokeCallback(elements, player);
    //        }
    //    }));
    //}
}