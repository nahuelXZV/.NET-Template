﻿using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using System.Security.Claims;
using Microsoft.JSInterop;

namespace WebClientMVC.Components;

public partial class MainBaseComponent : ComponentBase
{
    //[Inject] public IJSRuntime JSRuntime { get; set; }
    //[CascadingParameter] public Task<AuthenticationState> AuthenticationState { get; set; }
    //[CascadingParameter] protected AdminConfig AdminConfig { get; set; }
    //[CascadingParameter] public IAppServices AppServices { get; set; }
    //protected ClaimsPrincipal User { get; set; } = new ClaimsPrincipal();
    //public AlertaModal AlertaModal { get; set; } = new();

    //private async Task<ClaimsPrincipal> GetAuth()
    //{
    //    if (AuthenticationState is null) return null;

    //    var authState = await AuthenticationState;
    //    var user = authState?.User;

    //    return user;
    //}

    //protected bool IsAuthenticated()
    //{
    //    return User?.Identity is not null && User.Identity.IsAuthenticated;
    //}

    //protected override async Task OnInitializedAsync()
    //{
    //    User = await GetAuth();
    //}

    //public async Task ShowSuccessMessage(string message)
    //{
    //    await JSRuntime.InvokeVoidAsync("Message.success", message);
    //}

    //public async Task ShowInfoMessage(string message)
    //{
    //    await JSRuntime.InvokeVoidAsync("Message.info", message);
    //}

    //public async Task ShowWarnMessage(string message)
    //{
    //    await JSRuntime.InvokeVoidAsync("Message.warn", message);
    //}

    //public async Task ShowErrorMessage(string message)
    //{
    //    await JSRuntime.InvokeVoidAsync("Message.error", message);
    //}

    //public async Task ShowErrorMessage(Exception ex)
    //{
    //    string message = $"<p>{ex.Message}</p>";
    //    string errorDetails = "";

    //    if (ex != null)
    //    {
    //        string clientDetails = "";
    //        string diagnosticDetails = "";

    //        string details =
    //            @$"
    //                <div class='card full-details collapsed-card'>
    //                    <div class='card-header'>
    //                        <p class='card-title'>Detalles del error</p>
    //                        <button type='button' class='btn btn-tool' data-card-widget='collapse'>
    //                            <i class='fas fa-plus'></i>
    //                        </button>
    //                    </div>
    //                    <div class='card-body'>
    //                        <p><label>Mensaje: </label> {ex.Message}</p>
    //                        <p><label>Diagnostico: </label> {ex.Message}</p>
    //                        </br>
    //                        <p><label>Detalle: </label></p>
    //                        </br>
    //                        {diagnosticDetails}
    //                        <p><label>Pila de Error: </label></p>
    //                        <p>{ex.StackTrace}</p>
    //                    </div>
    //                </div>";

    //        errorDetails += $"<div class='col-12'>{details}</div>";
    //    }

    //    await JSRuntime.InvokeVoidAsync("Message.error", message + errorDetails);
    //}

    //public void MostrarAlertaModal(string message, string? titulo = "")
    //{
    //    AlertaModal.MostrarModal = true;
    //    AlertaModal.Contenido = message;
    //    if (!string.IsNullOrEmpty(titulo))
    //        AlertaModal.Titulo = titulo;
    //}

    //public void ModalCerradoAlertaHandler()
    //{
    //    AlertaModal = new();
    //    StateHasChanged();
    //}
}
