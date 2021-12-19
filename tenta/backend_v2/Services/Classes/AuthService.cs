using cryminals.Exceptions;
using cryminals.Models.InputModels;
using cryminals.Models.ViewModels;
using cryminals.Repositories.Interfaces;
using cryminals.Services.Interfaces;
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

namespace cryminals.Services.Classes
{
    public class AuthService : IAuthService
    {

        private readonly string SECRET_KEY = "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk";

        private readonly ICheckInputs _checkInputs;
        private readonly IAuthRepository _authRepo;

        public AuthService(ICheckInputs checkInputs, IAuthRepository authRepo)
        {
            _checkInputs = checkInputs;
            _authRepo = authRepo;
        }

        public async Task<bool> checkOwnership(string address, int[] ids)
        {
            try
            {
                return await _authRepo.checkOwnership(address, ids);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        public string generateToken(SignatureInputModel data)
        {
            try
            {
                if (_checkInputs.checkAddress(data.Address) && _checkInputs.checkHexHash(data.Signature) && _checkInputs.checkInt(data.Message, 1) && validateSignature(data))
                {
                    IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
                    IJsonSerializer serializer = new JsonNetSerializer();
                    IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                    IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

                    var payload = new Dictionary<string, object>
                    {
                        { "address", data.Address },
                        { "signature", data.Signature },
                        { "message", data.Message },
                        { "iat", DateTimeOffset.Now.ToUnixTimeSeconds() },
                        { "exp", DateTimeOffset.Now.ToUnixTimeSeconds() + 86400 }
                    };

                    return encoder.Encode(payload, SECRET_KEY);
                }
                else throw new InvalidInputException("unknown");
            }
            catch (InvalidInputException err)
            {
                throw new InvalidInputException(err.Message);
            }
            catch (Exception)
            {
                throw new TokenGenerationException();
            }

        }

        public TokenDataViewModel retrieveTokenData(string token)
        {
            try
            {
                if (_checkInputs.checkToken(token))
                {
                    var json = JwtBuilder.Create()
                     .WithAlgorithm(new HMACSHA256Algorithm())
                     .WithSecret(SECRET_KEY)
                     .MustVerifySignature()
                     .Decode(token);

                    var tokenData = JsonConvert.DeserializeObject<TokenDataViewModel>(json);
                    if (tokenData.exp > DateTimeOffset.Now.ToUnixTimeSeconds())
                    {
                        return tokenData;
                    }
                    else throw new TokenExpiredException("Token expired.");

                }
                else throw new InvalidInputException("token");
            }
            catch (TokenExpiredException)
            {
                throw new TokenExpiredException("Token expired.");
            }
            catch (SignatureVerificationException)
            {
                throw new SignatureVerificationException("Invalid Token.");
            }
            catch (Exception err)
            {
                throw new InvalidInputException(err.Message);
            }
        }


        public bool validateSignature(SignatureInputModel signature)
        {
            try
            {
                if (_checkInputs.checkAddress(signature.Address) && _checkInputs.checkHexHash(signature.Signature) && _checkInputs.checkInt(signature.Message, 1))
                {
                    var signer = new EthereumMessageSigner();
                    var resultantAddr = signer.EncodeUTF8AndEcRecover(signature.Message.ToString(), signature.Signature);
                    return resultantAddr.Equals(signature.Address);
                }
                else
                {
                    throw new InvalidInputException("unknown");
                }
            }
            catch (InvalidInputException err)
            {
                throw new InvalidInputException(err.Message);
            }
            catch (Exception)
            {
                throw new SignatureValidationException();
            }
        }
    }
}
