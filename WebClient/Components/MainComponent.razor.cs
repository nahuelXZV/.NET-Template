using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using System.Text.Json;
using WebClient.Configs;
using WebClient.Extensions;
using WebClient.Services;

namespace WebClient.Components;

public partial class MainComponent
{
    [Inject] public IAppServices AppServices { get; set; }
    [Inject] private IOptions<AdminConfig> Configs { get; set; }
    [Parameter] public string ComponentTypeName { get; set; }
    [Parameter] public IDictionary<string, object> Parameters { get; set; }
    private IDictionary<string, object> _parameters;
    private Type _componentType => Type.GetType(ComponentTypeName);

    private AdminConfig _adminConfig;

    protected override void OnInitialized()
    {
        _adminConfig = Configs.Value;
    }

    protected override void OnParametersSet()
    {
        _parameters = new Dictionary<string, object>();

        var componentParameters = _componentType.GetProperties()
            .Where(p => p.GetCustomAttributes(true).Any(attr => attr is ParameterAttribute));

        foreach (var parameter in componentParameters)
        {
            var parameterName = parameter.Name;

            dynamic parameterValue = Parameters[parameterName];

            if (parameterValue is JsonElement)
            {
                JsonElement json = parameterValue;
                var parameterType = parameter.PropertyType;

                parameterValue = json.GetRawText().Deserialize(parameterType);
            }

            _parameters.Add(parameterName, parameterValue);
        }
    }

}
