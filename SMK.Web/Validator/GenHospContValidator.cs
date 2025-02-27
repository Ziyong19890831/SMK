using FluentValidation;
using SMK.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace SMK.Web.Validator
{
    public class GenHospContValidator : AbstractValidator<GenHospCont>
    {
        public GenHospContValidator()
        {
            RuleFor(x => x.HospContType).NotNull().NotEmpty();
            RuleFor(x => x.HospContName).NotNull().NotEmpty();
        }
    }
}

