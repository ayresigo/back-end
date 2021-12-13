using cryminals.Exceptions;
using cryminals.Models.ViewModels;
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
    public class RobberyRepository : IRobberyRepository
    {
        private readonly MySqlConnection conn;
        private readonly ICheckInputs _checkInputs;

        public RobberyRepository(IConfiguration config, ICheckInputs checkInputs)
        {
            conn = new MySqlConnection(config.GetConnectionString("Default"));
            _checkInputs = checkInputs;
        }

        public void Dispose()
        {
            conn?.Close();
            conn?.Dispose();
        }

        public async Task<List<RobberyViewModel>> getRobberies(int status = 1)
        {
            try
            {
                if (_checkInputs.checkInt(status))
                {
                    var robberies = new List<RobberyViewModel>();
                    var query = $"SELECT * FROM `robberies` WHERE `status` = {status}";

                    await conn.OpenAsync();
                    MySqlCommand sqlCommand = new MySqlCommand(query, conn);
                    MySqlDataReader sqlDataReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync();

                    while (sqlDataReader.Read())
                    {
                        robberies.Add(new RobberyViewModel
                        {
                            Id = (int)sqlDataReader["Id"],
                            Name = (string)sqlDataReader["Name"],
                            Description = (string)sqlDataReader["Description"],
                            Reward = (int)sqlDataReader["Reward"],
                            Duration = (int)sqlDataReader["Duration"],
                            Stamina = (int)sqlDataReader["Stamina"],
                            Power = (int)sqlDataReader["Power"],
                            MinParticipants = (int)sqlDataReader["MinParticipants"],
                            MaxParticipants = (int)sqlDataReader["MaxParticipants"],
                            AmbushRisk = (int)sqlDataReader["AmbushRisk"],
                            PrisonRisk = (int)sqlDataReader["PrisonRisk"],
                            DeathRisk = (int)sqlDataReader["DeathRisk"],
                            Background = (string)sqlDataReader["Bg"]
                        });
                    }

                    await conn.CloseAsync();
                    return robberies;
                }
                else
                {
                    throw new InvalidInputException("status");
                }
            }
            catch (InvalidInputException err)
            {
                throw new InvalidInputException(err.Message);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        public async Task<RobberyViewModel> getRobbery(int id)
        {
            try
            {
                if (_checkInputs.checkInt(id))
                {
                    RobberyViewModel robbery = null;
                    var query = $"SELECT * FROM `robberies` WHERE `id` = '{id}'";

                    await conn.OpenAsync();

                    MySqlCommand sqlCommand = new MySqlCommand(query, conn);
                    MySqlDataReader sqlDataReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync();

                    while (sqlDataReader.Read())
                    {
                        robbery = new RobberyViewModel
                        {
                            Id = (int)sqlDataReader["Id"],
                            Name = (string)sqlDataReader["Name"],
                            Description = (string)sqlDataReader["Description"],
                            Reward = (int)sqlDataReader["Reward"],
                            Duration = (int)sqlDataReader["Duration"],
                            Stamina = (int)sqlDataReader["Stamina"],
                            Power = (int)sqlDataReader["Power"],
                            MinParticipants = (int)sqlDataReader["MinParticipants"],
                            MaxParticipants = (int)sqlDataReader["MaxParticipants"],
                            AmbushRisk = (int)sqlDataReader["AmbushRisk"],
                            PrisonRisk = (int)sqlDataReader["PrisonRisk"],
                            DeathRisk = (int)sqlDataReader["DeathRisk"],
                            Background = (string)sqlDataReader["Bg"]
                        };
                    }

                    await conn.CloseAsync();
                    return robbery;
                }
                else
                {
                    throw new InvalidInputException("id");
                }
            }
            catch (InvalidInputException ex)
            {
                throw new InvalidInputException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
