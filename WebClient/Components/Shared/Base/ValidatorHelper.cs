using FluentValidation;

namespace WebClient.Components.Shared.Base;

public class ValidatorHelper<T> where T : class
{
    private readonly IValidator<T> _validator;
    public Dictionary<string, List<string>> ListaErrores { get; private set; } = new();

    public ValidatorHelper(IValidator<T> validator)
    {
        _validator = validator;
    }

    public bool TieneErrores(string campo) => ListaErrores.ContainsKey(campo);
    public List<string> GetErrores(string campo) => TieneErrores(campo) ? ListaErrores[campo] : new List<string>();

    public async Task<bool> Validar(T modelo)
    {
        ListaErrores.Clear();
        var resultado = await _validator.ValidateAsync(modelo);

        if (!resultado.IsValid)
        {
            ListaErrores = resultado.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToList()
                );
            return false;
        }
        return true;
    }
}
