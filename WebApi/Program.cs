using Microsoft.EntityFrameworkCore;
using TODO.Persistence;
using TODO.Persistence.Interfaces;
using TODO.Application.Repository;
using System.Reflection;
using TODO.WebApi.Middleware;
using System.Text.Json.Serialization;
using TODO.Application.Notes.Commands.UpdateNote;
using FluentValidation.AspNetCore;
using FluentValidation;
using TODO.Application.Notes.Commands.CreateNote;
using TODO.Application.Reminders.Commands.CreateReminder;
using TODO.Application.Reminders.Commands.UpdateReminder;
using TODO.Application.Tags.Commands.CreateTagCommand;
using TODO.Application.Tags.Commands.UpdateTagCommand;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().
    AddJsonOptions(options => {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;

        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;

    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<INoteRepository, NoteRepository>();
builder.Services.AddScoped<IReminderRepository, ReminderRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
builder.Services.AddValidatorsFromAssemblyContaining<CreateNoteCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateNoteCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateReminderCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateReminderCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateTagCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateTagCommandValidator>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddApplication();

builder.Services.AddDbContext<AppDbContext>(options =>
options.UseNpgsql(builder.Configuration.GetConnectionString("TODOcs")));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
