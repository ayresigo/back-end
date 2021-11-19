using back_end.InputModel;
using back_end.Repositories;
using back_end.ViewModel;
using Nethereum.Signer;
using Nethereum.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Services
{
    public class LoginService : ILoginService
    {
        private readonly IUserAccountRepo _userAccountRepo;

        public LoginService(IUserAccountRepo userAccountRepository)
        {
            _userAccountRepo = userAccountRepository;
        }
        public Task<bool> checkSignature(SignatureInputModel request)
        {
            // Null Checks
            var addrValidator = new AddressUtil();
            if (request == null || string.IsNullOrEmpty(request.Message) || string.IsNullOrEmpty(request.Signature)
                || addrValidator.IsAnEmptyAddress(request.Address)
                || !addrValidator.IsValidEthereumAddressHexFormat(request.Address))
                return Task.FromResult(false);

            // Check for validity
            var signer = new EthereumMessageSigner();

            var resultantAddr = signer.EncodeUTF8AndEcRecover(request.Message, request.Signature);
            return Task.FromResult(!addrValidator.IsAnEmptyAddress(resultantAddr) && resultantAddr.Equals(request.Address));
        }

        public async Task<UserAccountViewModel> getAccount(string address)
        {
            var userAccount = await _userAccountRepo.getAccount(address);
            if (userAccount == null) return null;

            return new UserAccountViewModel
            {
                Id = userAccount.Id,
                Address = userAccount.Address,
                Username = userAccount.Username,
                Avatar = userAccount.Avatar,
                Money = userAccount.Money,
                Respect = userAccount.Respect,
                TotalPower = userAccount.TotalPower,
                Status = userAccount.Status
            };
        }
    }
}
