using Domain.DTOs.Segurity;
using FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using WebClient.Components.Shared.Base;

namespace WebClient.Components.Segurity.Usuario;

public partial class UsuarioCreateComponent
{
    [Inject] public NavigationManager Navigation { get; set; }
    [Inject] public IValidator<UsuarioDTO> Validator { get; set; }
    [Parameter] public UsuarioDTO Usuario { get; set; }
    [Parameter] public List<PerfilDTO> ListaPerfiles { get; set; }

    private ValidatorHelper<UsuarioDTO> _validatorHelper;
    private DotNetObjectReference<UsuarioCreateComponent> _objectHelper;
    private List<long> ListaAccesosSeleccionados { get; set; } = new();
    public bool ModificarContraseña { get; set; } = false;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        _validatorHelper = new ValidatorHelper<UsuarioDTO>(Validator);
        if (Usuario.Id != 0) await ModoEdicion();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender) return;
        await InicializarJSHelper();
    }

    private async Task ModoEdicion()
    {
        //ListaAccesosSeleccionados = Perfil.ListaAccesos.Select(a => a.AccesoId).ToList();
    }

    private async Task InicializarJSHelper()
    {
        try
        {
            _objectHelper = DotNetObjectReference.Create(this);
            await JSRuntime.InvokeVoidAsync("UsuarioCreateComponent.init", _objectHelper);
        }
        catch (Exception)
        {
            await JSRuntime.InvokeVoidAsync("console.warn", $"No se pudo inicializar JSHelper para componente: {nameof(UsuarioCreateComponent)}");
        }
    }

    [JSInvokable]
    public async Task Guardar()
    {
        if (!ModificarContraseña && Usuario.Id != 0) Usuario.Password = "temp";
        if (!await _validatorHelper.Validar(Usuario))
        {
            if (!ModificarContraseña && Usuario.Id != 0) Usuario.Password = string.Empty;
            StateHasChanged(); return;
        }

        try
        {
            if (!ModificarContraseña && Usuario.Id != 0) Usuario.Password = string.Empty;

            if (Usuario.Id != 0)
            {
                var respuesta = await AppServices.UsuarioService.Update(Usuario);
            }
            else
            {
                var respuesta = await AppServices.UsuarioService.Create(Usuario);
            }

            await ShowSuccessMessage("Usuario guardado correctamente");
            await Task.Delay(1000);
            Navigation.NavigateTo($"{AdminConfig.General.WebUrl}Usuario/listado", true);
        }
        catch (Exception ex)
        {
            await ShowErrorMessage(ex.Message);
        }
    }
}
