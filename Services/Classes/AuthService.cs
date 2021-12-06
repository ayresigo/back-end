using back_end.InputModel;
using back_end.Services.Interfaces;
using back_end.ViewModel;
using JWT;
using JWT.Algorithms;
using JWT.Builder;
using JWT.Exceptions;
using JWT.Serializers;
using Nethereum.Signer;
using Nethereum.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Services
{
    public class AuthService : IAuthService
    {
        private readonly ICheckInputs _checkInputs;

        public AuthService(ICheckInputs checkInputs)
        {
            _checkInputs = checkInputs;
        }

        // generate jwt
        const string _secret = "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk";


        // validate metamask signature


        public Task<bool> checkSignature(SignatureInputModel request)
        {
            var addrValidator = new AddressUtil();
            // Null Checks
            if (!_checkInputs.checkAddress(request.Address))
                return Task.FromResult(false);

            // Check for validity
            var signer = new EthereumMessageSigner();

            var resultantAddr = signer.EncodeUTF8AndEcRecover(request.Message, request.Signature);
            return Task.FromResult(!addrValidator.IsAnEmptyAddress(resultantAddr) && resultantAddr.Equals(request.Address));
        }

        public Task<TokenViewModel> generateToken(TokenDataInputModel data, string secret = _secret)
        {
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm(); // symmetric
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            //header
            //payload = data
            //signature

            var exp = 0;
            if (data.exp == 0) exp = 86400;
            else exp = data.exp;

            var payload = new Dictionary<string, object>
                {
                    { "id", data.id },
                    { "address", data.address },
                    { "signature", data.signature },
                    { "iat", DateTimeOffset.Now.ToUnixTimeSeconds() },
                    { "exp", DateTimeOffset.Now.ToUnixTimeSeconds() + exp },
                };

            var token = encoder.Encode(payload, secret);

            return Task.FromResult(new TokenViewModel
            {
                Token = token
            });
        }


        public Task<TokenDataViewModel> retrieveToken(string token)
        {            
            try
            {
                var json = JwtBuilder.Create()
                     .WithAlgorithm(new HMACSHA256Algorithm()) // symmetric
                     .WithSecret(_secret)
                     .MustVerifySignature()
                     .Decode(token);
                return Task.FromResult(JsonConvert.DeserializeObject<TokenDataViewModel>(json));
            }
            catch (TokenExpiredException)
            {
                throw new TokenExpiredException("Token expirado");
            }
            catch (SignatureVerificationException)
            {
                throw new SignatureVerificationException("Verificação da assinatura falhou.");
            }
        }
    }
}
