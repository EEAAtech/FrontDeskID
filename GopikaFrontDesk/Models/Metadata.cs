using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GopikaFrontDesk
{
    public class GuestsMetadata
    {
        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy hh:mm tt}", ApplyFormatInEditMode = true)]        
        public DateTime CheckIn;

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy hh:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime CheckOut;

        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]        
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string Firstn { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Surname")]
        public string Surn { get; set; }

        [StringLength(300)]
        public string Address { get; set; }

        [Display(Name = "Room No.")]
        public string RoomNo { get; set; }

        
    }
}