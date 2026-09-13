using Adventure.Daten;
using Adventure.Kern;
using Adventure.Wasm;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Im Browser gibt es kein Dateisystem – die Level sind fest einprogrammiert.
builder.Services.AddSingleton<ILevelQuelle, EingebauteLevelQuelle>();

await builder.Build().RunAsync();
