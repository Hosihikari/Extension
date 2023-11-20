global using static Hosihikari.NativeInterop.Unmanaged.Memory;

using Hosihikari.Minecraft;
using Hosihikari.Minecraft.Extension.Events;
using Hosihikari.Minecraft.Extension.Events.Implements.Player;
using Hosihikari.NativeInterop;
using Hosihikari.NativeInterop.Hook.ObjectOriented;
using Hosihikari.NativeInterop.Unmanaged;
using Hosihikari.NativeInterop.Unmanaged.STL;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace Hosihikari.FormUI;

public class FormResponseEventArgs : EventArgsBase
{
    public required Player Player { get; set; }
    public required uint FormId { get; set; }
    public required string Data { get; set; }
}

public unsafe class FormResponseEvent : HookEventBase<FormResponseEventArgs, FormResponseEvent.HookDelegate>
{
    private const string PacketHandlerDispatcherInstanceHandleSymbol =
#if WINDOWS
    "?handle@?$PacketHandlerDispatcherInstance@VModalFormResponsePacket@@$0A@@@UEBAXAEBVNetworkIdentifier@@AEAVNetEventCallback@@AEAV?$shared_ptr@VPacket@@@std@@@Z";
#else
        "";
#endif

    public delegate void HookDelegate(
        void* @this,
        Pointer<NetworkIdentifier> networkIdentifier,
        Pointer<ServerNetworkHandler> handler,
        CxxSharedPtr shared_ptr_packet);

    public FormResponseEvent() : base(PacketHandlerDispatcherInstanceHandleSymbol)
    {
    }

    public override HookDelegate HookedFunc =>
        (@this, _identifier, _handler, shared_ptr) =>
        {
            using var identifier = _identifier.Target;
            using var handler = _handler.Target;
            var player = handler.GetServerPlayer(identifier);
            var packet = (Pointer<Packet>)(nint)shared_ptr.ptr;

            if ((nint)player is not 0)
            {
                string? data = null;

                var id = DAccess<int>(packet, 48);
                if (DAccess<NativeBool>(packet, 81))
                {
                    if (DAccess<NativeBool>(packet, 72))
                    {
                        using var json = DAccessAsReference<Json.Value>(packet, 56).Target;
                        using var _cppstr = json.ToStyledString().GetInstance();
                        data = _cppstr.ToString();
                    }
                }

                if (string.IsNullOrWhiteSpace(data))
                    data = "null";

                if (data.EndsWith('\n'))
                    data = data[..^1];

                var e = new FormResponseEventArgs
                {
                    Player = player.As<Player>().Target,
                    FormId = (uint)id,
                    Data = data
                };

                OnEventBefore(e);
                Original(@this, identifier, handler, shared_ptr);
                OnEventAfter(e);
            }
        };
}

public unsafe static class FormHandler
{

    private static readonly List<uint> formIds = new();

    private static readonly Dictionary<uint, FormBase> forms = new();

    private static readonly System.Random random = new();

    static FormHandler()
    {
        Events.FormResponse.Before += HandleFormPacket;
    }

    private static uint NewFormId()
    {
        uint id;
        do
        {
            id = unchecked((uint)unchecked(unchecked(random.Next() << 16) + random.Next()));
        } while (formIds.Contains(id));

        return id;
    }

    private static void HandleFormPacket(object? sender, FormResponseEventArgs e)
    {
        var player = e.Player;
        var id = e.FormId;
        var data = e.Data;

        if (forms.TryGetValue(id, out var form) is false)
            return;

        if (form is SimpleForm simpleForm)
        {
            var chosen = data is "null" ? -1 : int.Parse(data);
            simpleForm.InvokeCallback(player, chosen);

            if (chosen >= 0)
            {
                var button = (Button)simpleForm.Elements[chosen];
                button.InvokeOnClicked(player);
            }

            forms.Remove(id);
        }
        else if (form is ModalForm modalForm)
        {
            var chosen = data is "true" ? ModalForm.Chosen.Confirm : ModalForm.Chosen.Cancel;
            modalForm.InvokeCallback(player, chosen);

            forms.Remove(id);
        }
        else if (form is CustomForm customForm)
        {
            var doc = JsonDocument.Parse(data);
            int index = 0;
            foreach (var json in doc.RootElement.EnumerateArray())
            {
                var element = customForm.Elements[index].element;
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
                            var options = (element as Dropdown)!.Options;
                            element.Value = options[json.GetInt32()];
                            element.InvokeValueChanged();
                        }
                        break;
                    case CustomFormElement.ElementType.StepSlider:
                        {
                            var options = (element as StepSlider)!.Options;
                            element.Value = options[json.GetInt32()];
                            element.InvokeValueChanged();
                        }
                        break;
                }
                ++index;
            }

            if (customForm.IsNullCallback is false)
            {
                var elements = new Dictionary<string, CustomFormElement>();
                foreach (var (k, v) in customForm.Elements)
                    elements.Add(k, v);

                customForm.InvokeCallback(elements, player);
            }

            forms.Remove(id);
        }
    }

    public unsafe static void SendTo(this FormBase form, Player player)
    {
        var id = NewFormId();
        forms.Add(id, form);
        var bs = new BinaryStream((ulong)sizeof(void*) + StdString.ClassSize);

        using var data = new StdString(form.SerializedData);

        fixed (byte* ptr = data.Data)
        {
            bs.WriteUnsignedVarInt(id);
            bs.WriteString(new() { ptr = ptr, length = data.Length });
        }

        var sharedPtr_packet = MinecraftPackets.CreatePacket(MinecraftPacketIds.ShowModalForm);
        HeapAlloc.Delete(sharedPtr_packet.ctr);
        var packet = Packet.ConstructInstance((nint)sharedPtr_packet.ptr, false, false);
        packet.Read((Reference<ReadOnlyBinaryStream>)(nint)bs).Drop();

        player.SendNetworkPacket(packet);

        var vfptr = CppTypeSystem.GetVTable<Packet.Vftable>(packet)->__UnknownVirtualFunction_0;
        ((delegate* unmanaged<void*, void>)vfptr)(packet);
        HeapAlloc.Delete(packet);
        bs.Dispose();
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
#nullable disable
}
