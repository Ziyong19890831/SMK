using FluentValidation;
using SMK.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace SMK.Web.Validator
{

    public class GenPrsnTypeValidator : AbstractValidator<GenPrsnType>
    {
        public GenPrsnTypeValidator()
        {
            RuleFor(x => x.PrsnType).NotNull().NotEmpty();
            RuleFor(x => x.PrsnTypeName).NotNull().NotEmpty();
        }
    }
}
