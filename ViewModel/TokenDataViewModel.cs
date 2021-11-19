using back_end.InputModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.ViewModel
{
    public class TokenDataInputModel
    {
        public int accountId { get; set; }
        public SignatureInputModel signatureReq { get; set; }
    }
}
