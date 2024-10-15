using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens.Saml2;
using Rsk.Saml;
using Rsk.Saml.Generators;

namespace IDP
{
    
        public class CustomNameIdGenerator : SamlNameIdGenerator
    {
        private readonly ILogger<ISamlNameIdGenerator> _logger;
        public CustomNameIdGenerator(ILogger<ISamlNameIdGenerator> logger) : base(logger)
        {
            _logger=logger;
           
        }

        public override async Task<Saml2Subject> GenerateNameId(string subjectId, IList<Claim> userClaims, string defaultFormat, string requestedFormat = null)
        {
            var responseFormat = GetResponseFormat(requestedFormat, defaultFormat);
            if (responseFormat == SamlConstants.NameIdentifierFormats.Transient)
            {
                //var nameIdValue = userClaims.FirstOrDefault(x => x.Type == "SURID")?.Value;
                String nameIdValue = GetValueFromClaims(userClaims,"WDID");
                _logger.LogInformation("nameIdValue: {nameIdValue}", nameIdValue);
                if(nameIdValue.Equals(""))   
                {
                    nameIdValue = GetValueFromClaims(userClaims,"SURID");
                }             
                return new Saml2Subject(new Saml2NameIdentifier(nameIdValue, new Uri(SamlConstants.NameIdentifierFormats.Transient)));
            }

            return await base.GenerateNameId(subjectId, userClaims, defaultFormat, requestedFormat);
        }

        private static String GetValueFromClaims(IEnumerable<Claim> myClaims, string sClaimId)
        {
            String ClaimVal = "";
            if (myClaims is not null)
            {
                var cClaimV = myClaims.FirstOrDefault(x => x.Type == sClaimId);
                if (cClaimV is not null)
                {
                    ClaimVal = cClaimV.Value;
                }
            }
            return ClaimVal;
        }
    }
    
}