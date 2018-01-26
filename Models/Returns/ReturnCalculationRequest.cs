using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VSBaseAngular.Models
{

    public class NewReturnCalculationRequest
    {
        [Required]
        public long SiNumber { get; set; }
        public bool IsFraude { get; set; }
        public bool IsError { get; set; }
        public string Reason { get; set; }
        public string Remark { get; set; }
    }


    public class ReturnCalculationRequest {
        [Required]
        public long SiNumber { get; set; }
        public int FederationNumber { get; set; }
        public long ReturnCalculationId { get; set; }
        public bool IsFraude { get; set; }
        public bool IsError { get; set; }
        public string Reason { get; set; }
        public string Remark { get; set; }
        public string Insz { get; set; }
        public string CreatedBy { get; set; }

        public decimal ReturnByOGM { get; set; }
        public decimal ReturnByDeduction { get; set; }

        public DateTime? RegularLetterCreateDate { get; set; }
        public string RegularLetterCreatedBy { get; set; }

        public DateTime? SignedLetterCreateDate { get; set; }
        public string SignedLetterCreatedBy { get; set; }

        public bool ObjectionMade { get; set; }
        public DateTime ObjectionDate { get; set; }
        public bool ReturnStarted { get; set; }    

        public List<ReturnCalculationLine> ReturnLines { get; set; }
    }
}