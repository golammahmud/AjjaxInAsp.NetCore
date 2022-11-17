using AuthInAsp.NetCore.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);



var connectionString = builder.Configuration.GetConnectionString("DBConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(connectionString));


// Add services to the container.
builder.Services.AddRazorPages()
    .AddRazorPagesOptions(op =>
{
    op.Conventions.AuthorizeFolder("/");
    op.Conventions.AllowAnonymousToPage("/Account/Login");
    op.Conventions.AllowAnonymousToPage("/Account/Register");
}
    )
    .AddMvcOptions(options =>
    {
        var policy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build();
        options.Filters.Add(new AuthorizeFilter(policy));
    });


builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 5;
    options.Password.RequiredUniqueChars = 2;
}).AddEntityFrameworkStores<ApplicationDbContext>();

//builder.Services.AddMvc(options =>
//{
//    var policy=new AuthorizationPolicyBuilder()
//    .RequireAuthenticatedUser()
//    .Build();

//    options.Filters.Add(new AuthorizeFilter(policy));
//}).AddXmlSerializerFormatters();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
