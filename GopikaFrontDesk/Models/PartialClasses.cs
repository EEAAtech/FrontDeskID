using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GopikaFrontDesk
{
    [MetadataType(typeof(GuestsMetadata))]
    public partial class Guest
    {
    }

    public class GuestImg
    {
        public Guest gst { get; set; }
        public HttpPostedFileBase UploadedFile { get; set; }
                
    }
}