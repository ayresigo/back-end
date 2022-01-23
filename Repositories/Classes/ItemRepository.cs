using cryminals.Exceptions;
using cryminals.Models.ViewModels;
 
using cryminals.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cryminals.Repositories.Classes
{
    public interface IItemRepository : IDisposable
    {
        public Task<ItemDBViewModel> getItemDB(int id);
        public Task<List<ItemViewModel>> fetchItems(string token);
    }
    public class ItemRepository : IItemRepository
    {
        private readonly MySqlConnection conn;
        private readonly ICheckInputs _checkInputs;
        private readonly IAuthService _authService;

        public ItemRepository(IConfiguration config, ICheckInputs checkInputs, IAuthService authService)
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

        public async Task<List<ItemViewModel>> fetchItems(string token)
        {
            try
            {
                if (_checkInputs.checkToken(token))
                {
                    var account = _authService.retrieveTokenData(token);
                    var items = new List<ItemViewModel>();
                    var query = $"SELECT * FROM `items` INNER JOIN `items_db`  " +
                    $"ON `items`.`fk_item_db_id` = `items_db`.`item_db_id` " +
                    $"WHERE `items`.`fk_item_owner_address` = '{account.address}'";

                    await conn.OpenAsync();
                    MySqlCommand sqlCommand = new MySqlCommand(query, conn);
                    MySqlDataReader sqlDataReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync();

                    while (sqlDataReader.Read())
                    {
                        var item = new ItemDBViewModel
                        {
                            Id = (int)sqlDataReader["item_db_id"],
                            Name = (string)sqlDataReader["item_db_name"],
                            Description = (string)sqlDataReader["item_db_description"],
                            Rarity = (string)sqlDataReader["item_db_rarity"],
                            Type = (string)sqlDataReader["item_db_type"],
                            Icon = (string)sqlDataReader["item_db_icon"],
                        };

                        items.Add(new ItemViewModel
                        {
                            Id = (int)sqlDataReader["item_id"],
                            Owner = (string)sqlDataReader["fk_item_owner_address"],
                            EquippedBy = (int)sqlDataReader["fk_item_equipped_by_id"],
                            Durability = (int)sqlDataReader["durability"],
                            Item = item,
                        }) ;
                    }

                    await conn.CloseAsync();
                    return items;
                }
                else throw new InvalidInputException("id");
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

        public async Task<ItemDBViewModel> getItemDB(int id)
        {
            try
            {
                if (_checkInputs.checkInt(id))
                {
                    ItemDBViewModel item = null;
                    var query = $"SELECT * FROM `items_db` WHERE `item_db_id` = {id}";

                    await conn.OpenAsync();
                    MySqlCommand sqlCommand = new MySqlCommand(query, conn);
                    MySqlDataReader sqlDataReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync();

                    while (sqlDataReader.Read())
                    {
                        item = new ItemDBViewModel
                        {
                            Id = (int)sqlDataReader["item_db_id"],
                            Name = (string)sqlDataReader["item_db_name"],
                            Description = (string)sqlDataReader["item_db_description"],
                            Rarity = (string)sqlDataReader["item_db_rarity"],
                            Type = (string)sqlDataReader["item_db_type"],
                            Icon = (string)sqlDataReader["item_db_icon"],
                        };
                    }

                    await conn.CloseAsync();
                    return item;
                }
                else throw new InvalidInputException("id");
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
