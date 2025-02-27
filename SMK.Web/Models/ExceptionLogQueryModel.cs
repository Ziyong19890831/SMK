using SMK.Data.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMK.Web.Models
{
    public class ExceptionLogQueryModel : PagedRequest
    {
        public string Id { get; set; }

        public string Category { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime StopTime { get; set; }
    }
}