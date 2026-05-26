using System.Collections.ObjectModel;
using ControlEquiposElectronicos.DTOs.Usuarios;
using ControlEquiposElectronicos.Services.Interfaces;

namespace ControlEquiposElectronicos.ViewModels;

public class UsuariosViewModel : BaseViewModel
{
    private readonly IUsuarioApiService _usuarioApiService;

    public ObservableCollection<UsuarioListadoDto> Usuarios { get; } = new();

    public UsuariosViewModel(IUsuarioApiService usuarioApiService)
    {
        _usuarioApiService = usuarioApiService;
        Title = "Usuarios";
    }

    public async Task CargarAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;

            var lista = await _usuarioApiService.ObtenerTodosAsync();

            Usuarios.Clear();
            if (lista != null)
            {
                foreach (var usuario in lista)
                    Usuarios.Add(usuario);
            }
        }
        finally
        {
            IsBusy = false;
        }
    }
}
