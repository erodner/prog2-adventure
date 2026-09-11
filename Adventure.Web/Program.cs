using Adventure.Daten;
using Adventure.Kern;
using Adventure.Web.Components;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Die eine Stelle, an der entschieden wird, woher die Level kommen.
builder.Services.AddSingleton<ILevelQuelle>(_ =>
    new TextdateiLevelQuelle(Path.Combine(AppContext.BaseDirectory, "levels")));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
