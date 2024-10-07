using MessageBus;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.OpenApi.Models;
using NLog.Web;
using PersonalOffice.Backend.API.Authentication;
using PersonalOffice.Backend.API.Extensions;
using PersonalOffice.Backend.Application;
using PersonalOffice.Backend.Application.Common.Global;
using PersonalOffice.Backend.Application.Common.Mappings;
using PersonalOffice.Backend.Application.CQRS.Question.Queries.GetTopics;
using System.Reflection;

var logger = NLog.LogManager.GetLogger("PersonalOffice.Backend.API.Program");
var builder = WebApplication.CreateBuilder(args);
var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

{
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    builder.Services.AddHttpLogging(options =>
    {
        options.LoggingFields = HttpLoggingFields.RequestBody |
                                HttpLoggingFields.ResponseStatusCode |
                                HttpLoggingFields.Duration |
                                HttpLoggingFields.ResponseBody;
        options.CombineLogs = true;
        options.RequestBodyLogLimit = 200;
    });
    builder.Services.AddHttpLoggingInterceptor<RulesHttpLoggingInterceptor>();

    builder.Configuration.AddJsonFile(Path.Combine(Environment.CurrentDirectory, "Settings", $"appsettings.{env}.json"), optional: true);
    logger.Info($"appsettings.{env}.json");

    builder.Services.AddMessageBus(config =>
    {
        config.HostName = builder.Configuration["RabbitMq:Host"];
        config.Port = System.Convert.ToInt32(builder.Configuration["RabbitMq:Port"]);
        config.UserName = builder.Configuration["RabbitMq:Login"];
        config.Password = builder.Configuration["RabbitMq:Password"];
        config.IsSSL = System.Convert.ToBoolean(builder.Configuration["RabbitMq:IsSSL"]);

        config.ReceivedQueue = new MessageBusCore.Data.ReceivedQueueData
        {
            QueueName = builder.Configuration["RabbitMq:Queue"],
        };
    });
    MicroserviceNames.Backend = builder.Configuration["RabbitMq:Queue"] ?? MicroserviceNames.Backend;

    builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = builder.Configuration.GetConnectionString("Redis");
    });

    builder.Services.AddApplication(); // Добавление слоя приложения (слой Application файл DependencyInjection.cs метод расширения)
    builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = false;
    }); ; // Добавление контроллеров в контейнер сервисов для управления HTTP-операциями


    builder.Services.AddAutoMapper(config =>  // Добавление AutoMapper для всех типов в текущей сборке
    {
        config.AddProfile(new AssemblyMappingsProfile(typeof(PersonalOffice.Backend.Application.DependencyInjection).Assembly));
        config.AddProfile(new AssemblyMappingsProfile(Assembly.GetExecutingAssembly()));

        config.CreateMap<(int userID, string TopicTypeCode), GetTopicsQuery>()
            .ForMember(x => x.UserId, opt => opt.MapFrom(src => src.userID))
            .ForMember(x => x.TopicTypeID, opt => opt.MapFrom(src => new GetTopicsQuery { UserId = src.userID }.TopicTypeCodeToInt(src.TopicTypeCode)));
    });

    builder.Services.AddRouting(options => options.LowercaseUrls = true); // Установка всех эндпоинтов в нижний регистр

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = PersonalOfficeAuthenticationSchemeOptions.NAME;
        options.DefaultChallengeScheme = PersonalOfficeAuthenticationSchemeOptions.NAME;
        options.DefaultSignInScheme = PersonalOfficeAuthenticationSchemeOptions.NAME;
        options.DefaultSignOutScheme = PersonalOfficeAuthenticationSchemeOptions.NAME;
    })
    .AddScheme<PersonalOfficeAuthenticationSchemeOptions, PersonalOfficeAuthenticationHandler>(PersonalOfficeAuthenticationSchemeOptions.NAME, options => { });

    //builder.Services.AddAuthorization(options =>
    //{
    //    options.AddPolicy("OnlyForURlico", policy =>
    //    {
    //        policy.RequireClaim("Status", "False");
    //    });
    //});

    builder.Services.AddEndpointsApiExplorer();// Добавление эндпоинтов API Explorer для Swagger
    builder.Services.AddSwaggerGen(options =>
    {
        options.IncludeXmlComments(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PersonalOffice.Backend.API.xml"));
        options.IncludeXmlComments(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PersonalOffice.Backend.Application.xml"));
        options.IncludeXmlComments(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PersonalOffice.Backend.Domain.xml"));

        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "Personal Office REST API",
            Description = "API для Личного кабинета \"СОЛИД БРОКЕР\"",
            Contact = new OpenApiContact
            {
                Name = "АО \"ИФК СОЛИД\"",
                Url = new Uri("https://solidbroker.ru/")
            },
        });
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "Авторизация на основе JWT Bearer. Токен можно получить в методе Login указав учетные данные, id необязательное поле.",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Scheme = "Bearer"
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id= "Bearer",
                    },
                    Scheme = PersonalOfficeAuthenticationSchemeOptions.NAME,
                    Name = "Bearer",
                    In = ParameterLocation.Header,
                },
                new List<string>()
            }
        });
    }); // Добавление генератора Swagger для автоматического создания документации
}

builder.Services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

{
    var app = builder.Build();
    app.UseCors("AllowAll");

    if (app.Environment.IsDevelopment() || env == Env.Test)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseDeveloperExceptionPage();
    }

    app.UseHttpLogging();
    app.UseExceptionMiddleware();

    app.UseAuthentication();
    app.UseAuthorization();

    //------------------кеширование статических файлов---------------------------------//
    //app.UseDefaultFiles();
    //app.UseStaticFiles(new StaticFileOptions()
    //{
    //    OnPrepareResponse = ctx =>
    //    {
    //        ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=600");
    //    }
    //});
    //--------------------------------------------------------------------------------//

    app.MapControllers().RequireAuthorization();


    app.Run();
}

