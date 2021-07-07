using Microsoft.Extensions.Configuration;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Globomantics.CloudGame.Service
{
    public interface IGameApiService
    {
        Task SaveUserScoreAsync(int score);
    }

    public class GameApiService : IGameApiService
    {
        IConfiguration _configuration;

        public GameApiService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SaveUserScoreAsync(int score)
        {
                var connectString = _configuration["AzureSQL:ConnectionString"];
                if(connectString != null)
                {
                    SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectString);
                    var conn = new SqlConnection(builder.ConnectionString);
                    string insertString = @"INSERT INTO Score(Score) VALUES (@UserScore)";
                    SqlCommand cmd = new SqlCommand(insertString, conn);
                    cmd.Parameters.AddWithValue("@UserScore", score);
                    cmd.ExecuteNonQuery();
                    await conn.CloseAsync();
                }
        }
    }
}
