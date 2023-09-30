using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using Procurados.Data;
using Procurados.Models;

internal class Program
{
    private static void Main(string[] args)
    {
        string connectionString = "User Id=RM94054;Password=270786;Data Source=oracle.fiap.com.br:1521/ORCL;Connection Timeout=300;";

        using (OracleConnection connection = new OracleConnection(connectionString))
        {
            try
            {
                connection.Open();

                Console.WriteLine("Conex�o bem-sucedida!");

                string sqlQuery = "SELECT * FROM t_users";
                OracleCommand command = new OracleCommand(sqlQuery, connection);

                using (OracleDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                       
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
            }
        }


        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddDbContext<IdwallContext>(options => options.UseOracle(connectionString).EnableSensitiveDataLogging(true));

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthorization();

        app.MapControllers();

        app.Run();


        var host = CreateHostBuilder(args).Build();

        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            try
            {
                var httpClient = services.GetRequiredService<HttpClient>();

                var idwall = services.GetRequiredService<IdwallConfig>();

                idwall.SetHttpClient(httpClient);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        host.Run();
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
                Host.CreateDefaultBuilder(args)
                    .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder.UseStartup<IdwallConfig>();
                    });
}





  