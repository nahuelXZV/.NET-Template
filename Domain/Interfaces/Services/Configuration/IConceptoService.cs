
using Domain.Entities.Configuration;

namespace Domain.Interfaces.Services.Configuration;

public interface IConceptoService
{
    Task<Concepto> GetByCodigo(string codigo);
}
