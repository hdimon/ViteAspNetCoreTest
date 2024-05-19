using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using ViteAspNetCoreTest.Common.ActionFilters;

namespace ViteAspNetCoreTest.Configuration;

public class ConfigureSwaggerOptions : IConfigureNamedOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
    {
        _provider = provider;
    }

    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, CreateVersionInfo(description));
        }

        /*options.AddSecurityDefinition(ApiKeyDefaults.SchemeName, new OpenApiSecurityScheme
        {
            Description = "ApiKey must appear in query parameter",
            Type = SecuritySchemeType.ApiKey,
            Name = ApiKeyDefaults.DefaultApiKeyArgName,
            In = ParameterLocation.Query,
            Scheme = ApiKeyDefaults.SchemeName
        });

        var key = new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = ApiKeyDefaults.SchemeName
            },
            In = ParameterLocation.Query
        };

        var requirement = new OpenApiSecurityRequirement
        {
            { key, new List<string>() }
        };*/

        /*options.AddSecurityDefinition(name: ApiKeyDefaults.BearerSchemeName, securityScheme: new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Description = "Enter the Bearer Authorization string as following: Bearer {ApiKey}",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = ApiKeyDefaults.BearerSchemeName
        });

        var key = new OpenApiSecurityScheme
        {
            Name = ApiKeyDefaults.BearerSchemeName,
            In = ParameterLocation.Header,
            Reference = new OpenApiReference
            {
                Id = ApiKeyDefaults.BearerSchemeName,
                Type = ReferenceType.SecurityScheme
            }
        };

        var requirement = new OpenApiSecurityRequirement
        {
            { key, new List<string>() }
        };

        options.AddSecurityRequirement(requirement);*/

        options.OperationFilter<ReApplyOptionalRouteParameterOperationFilter>();
    }

    public void Configure(string? name, SwaggerGenOptions options)
    {
        Configure(options);
    }

    private OpenApiInfo CreateVersionInfo(ApiVersionDescription description)
    {
        var info = new OpenApiInfo
        {
            Title = "ViteAspNetCoreTest API",
            Version = description.ApiVersion.ToString()
        };

        if (description.IsDeprecated)
        {
            info.Description += " This API version has been deprecated.";
        }

        return info;
    }
}