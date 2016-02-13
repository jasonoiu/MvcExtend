using System;
using System.Data.Entity;
using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using OAuthService.Auth;
using OAuthService.Providers;
using Owin;

[assembly: OwinStartup(typeof(OAuthService.Startup))]
namespace OAuthService
{
    
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            WebApiConfig.Register(config);
            ConfigureOAuth(app);

            //这一行代码必须放在ConfiureOAuth(app)之后
            app.UseWebApi(config);
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<AuthContext, Migrations.Configuration>());
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true, //允许客户端使用http协议请求
                TokenEndpointPath = new PathString("/token"), //token请求的地址
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30), //token过期时间
                Provider = new SimpleAuthorizationServerProvider(), //提供具体的认证策略
                //refresh token provider
                RefreshTokenProvider = new SimpleRefreshTokenProvider()
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }
}