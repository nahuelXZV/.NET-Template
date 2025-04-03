﻿

using Domain.Entities.Segurity;

namespace Domain.DTOs.Segurity;

public class PerfilDTO
{
    public long Id { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }

    public List<PerfilAcceso> ListaAccesos { get; set; }
}
