using BiblioScope.View;
using BiblioScope.ViewModel;
using Firebase.Auth;
using Firebase.Auth.Providers;
using Firebase.Auth.Repository;
using Google.Cloud.Firestore.V1;
using Microsoft.Extensions.Logging;

namespace BiblioScope
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton(new FirebaseAuthClient(new FirebaseAuthConfig()
            {
                ApiKey = "AIzaSyB3bTHLIYEHSN3e2QhVMTYpScfJ65S6GlY",
                AuthDomain = "biblioscope.firebaseapp.com",
                Providers = new FirebaseAuthProvider[]
                {
                    new EmailProvider(),
                },
                UserRepository = new FileUserRepository("BiblioScope") //user login persistence
            }));
            
            builder.Services.AddSingleton<SignInPage>();
            builder.Services.AddSingleton<SignInViewModel>();
            builder.Services.AddSingleton<SignUpPage>();
            builder.Services.AddSingleton<SignUpViewModel>();
            
            builder.Services.AddTransient<LibraryViewModel>();
            builder.Services.AddSingleton<UserLibraryPage>();
            
            builder.Services.AddTransient<LibraryBookDetailPage>();
            
            return builder.Build();
        }
    }
}
