// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PTMK.Controllers;
using PTMK.Data;
using PTMK.Services;

var services = new ServiceCollection();
services.AddTransient<PTMKService>();
services.AddTransient<PersonsRepository>();
services.AddDbContext<PTMKDbContext>(ServiceLifetime.Transient);
services.BuildServiceProvider().GetService<PTMKService>()?.HandleConsole();