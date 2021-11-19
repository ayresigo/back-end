using back_end.InputModel;
using back_end.ViewModel;
using JWT;
using JWT.Algorithms;
using JWT.Exceptions;
using JWT.Serializers;
using Nethereum.Signer;
using Nethereum.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Services
{
    public class AuthService : IAuthService
    {
        // generate jwt
        const string _secret = "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk";


        // validate metamask signature


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

        public Task<TokenViewModel> generateToken(TokenDataInputModel data, string secret = _secret)
        {
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm(); // symmetric
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            //header
            //payload = data
            //signature

            var payload = new Dictionary<string, object>
                {
                    { "accountId", data.accountId },
                    { "address", data.signatureReq.Address },
                    { "message", data.signatureReq.Message },
                    { "signature", data.signatureReq.Signature }
                };

            var token = encoder.Encode(payload, secret);

            return Task.FromResult(new TokenViewModel
            {
                Token = token
            });
        }


        public Task<string> retrieveToken(TokenInputModel token)
        {
            try
            {
                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtAlgorithm algorithm = new HMACSHA256Algorithm(); // symmetric
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);

                var json = decoder.Decode(token.Token, _secret, verify: true);
                return Task.FromResult(json) ;
            }
            catch (TokenExpiredException er)
            {
                return Task.FromResult(er.Message);
            }
            catch (SignatureVerificationException er)
            {
                return Task.FromResult(er.Message);
            }
        }
    }
}
