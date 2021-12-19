using cryminals.Exceptions;
using cryminals.Models.InputModels;
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
        private readonly IAuthService _authService;
        private readonly IAccountRepository _accountRepository;

        public RobberyRepository(IConfiguration config, ICheckInputs checkInputs, IAuthService authService)
        {
            conn = new MySqlConnection(config.GetConnectionString("Default"));
            _checkInputs = checkInputs;
            _authService = authService;
        }

        public void Dispose()
        {
            conn?.Close();
            conn?.Dispose();
        }

        public async Task<string> startRobbery(StartRobberyInputModel data)
        {
            // Verificar robbery, status do personagem e calcular as odds.
            try
            {
                if (_checkInputs.checkToken(data.Token) && _checkInputs.checkInt(data.RobberyId)) // valida input do token e do id da robbery
                {
                    for (int i = 0; i < data.Participants.Length; i++)
                    {
                        if (!_checkInputs.checkInt(data.Participants[i])) throw new InvalidInputException("Invalid Input"); // valida input dos ids dos participantes
                    }
                    var account = _authService.retrieveTokenData(data.Token);
                    var robbery = await getRobbery(data.RobberyId);
                    if (data.Participants.Length >= robbery.MinParticipants && data.Participants.Length <= robbery.MaxParticipants) // valida se o numero de participantes está dentro dos limites da robbery
                    {
                        if (await _authService.checkOwnership(account.address, data.Participants)) // verifica se os personagens são possuídos pelo caller (deveria ser feito pela blockchain)
                        {
                            return "Roubo iniciado!";
                        }
                        else throw new Exception("Character ownership verification failed");
                    }
                    else throw new Exception("Number of participants is not permitted");
                }
                else throw new InvalidInputException("Invalid input");
                return "Roubo não iniciado!";
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }

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
