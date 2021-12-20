using cryminals.Exceptions;
using cryminals.Repositories.Interfaces;
using cryminals.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cryminals.Repositories.Classes
{
    public class AuthRepository : IAuthRepository
    {
        private readonly MySqlConnection conn;
        private readonly ICheckInputs _checkInputs;

        public AuthRepository(IConfiguration config, ICheckInputs checkInputs)
        {
            conn = new MySqlConnection(config.GetConnectionString("Default"));
            _checkInputs = checkInputs;

        }
        public async Task<bool> checkOwnership(string address, int[] ids)
        {
            List<bool> isOwner = new List<bool>();
            if (_checkInputs.checkAddress(address))
            {
                for (int i = 0; i < ids.Length; i++)
                {
                    if (!_checkInputs.checkInt(ids[i])) throw new InvalidInputException("ids");
                    var query = $"SELECT `fk_owner_address` FROM `characters` WHERE `id` = '{ids[i]}'";

                    await conn.OpenAsync();
                    MySqlCommand sqlCommand = new MySqlCommand(query, conn);
                    MySqlDataReader sqlDataReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync();

                    while (sqlDataReader.Read())
                    {
                        var _address = (string)sqlDataReader["fk_owner_address"];
                        if (_address == address) isOwner.Add(true); else return false;
                    }
                    await conn.CloseAsync();
                }
                if (isOwner.Count == ids.Length)
                    return true;
                else return false;
            }
            else
            {
                throw new InvalidInputException("address");
            }
        }

        public void Dispose()
        {
            conn?.Close();
            conn?.Dispose();
        }
    }
}
