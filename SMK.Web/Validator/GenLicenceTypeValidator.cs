

using FluentValidation;
using SMK.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMK.Web.Validator
{
    public class GenLicenceTypeValidator : AbstractValidator<GenLicenceType>
    {
        public GenLicenceTypeValidator()
        {
            RuleFor(x => x.LicenceType).NotNull().NotEmpty();
            RuleFor(x => x.LicenceName).NotNull().NotEmpty();
        }
    }
}
