using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StatusNFE.Models
{
    public class StatusNfe
    {
        public string Casa { get; set; }
        public bool Nfe = false;
        public bool Nfse = false;
        public bool Integracao = false;
        public bool Timer = false;
        public bool Edi = false;
       
    }
}