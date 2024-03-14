using FinanceFolio;
using FinanceFolio.Data;
using Firebase.Auth;
using Firebase.Auth.Providers;
using FirebaseAdmin;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ApiDependencies
{

    public static class DependencyExtensionMethods
    {
        public static IServiceCollection AddApiDependencies(this IServiceCollection service)
        {
            service.AddSessionServices();
            service.AddFirebaseServices();
            service.AddAuthenticationServices();
            return service;
        }

        private static IServiceCollection AddSessionServices(this IServiceCollection service)
        {
            service.AddDistributedMemoryCache(); // Example registration for DistributedMemoryCache
            service.AddSession();
            return service;
        }

        private static IServiceCollection AddFirebaseServices(this IServiceCollection service)
        {
            service.AddSingleton(FirebaseApp.Create());
            service.AddSingleton<FirebaseAuthClient>();
            service.AddSingleton<FirebaseAuthConfig>();
            service.AddSingleton<Authentication>();

            service.AddSingleton(new FirebaseAuthClient(new FirebaseAuthConfig
            {
                ApiKey = "AIzaSyAEaM8Ccz1irTF3rjrqJGU65YPozgwb--E",
                AuthDomain = $"authfinancefolio.firebaseapp.com",
                Providers = new FirebaseAuthProvider[]
                {
                    new EmailProvider(),
                    new GoogleProvider()
                }
            }));
            service.AddSingleton<IAuthentication, Authentication>(); 
            
            return service;
        }

        private static IServiceCollection AddAuthenticationServices(this IServiceCollection service)
        {
            service.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = $"https://securetoken.google.com/authfinancefolio";
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = $"https://securetoken.google.com/authfinancefolio",
                        ValidateAudience = true,
                        ValidAudience = "authfinancefolio",
                        ValidateLifetime = true
                    };
                });
            service.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            return service;
        }
    }
}