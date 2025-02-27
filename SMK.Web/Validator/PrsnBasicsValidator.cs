using FluentValidation;
using SMK.Data.Entity;
using SMK.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMK.Web.Validator
{
    public class PrsnBasicsValidator : AbstractValidator<PrsnBasic>
    {
        public PrsnBasicsValidator()
        {
            RuleFor(x => x.PrsnId).NotEmpty().WithMessage("身分證字號不可為空白。");
            RuleFor(x => x.PrsnName).NotEmpty().WithMessage("姓名不可為空白。");
            RuleFor(x => x.PrsnBirthday).NotEmpty().WithMessage("生日不可為空白。");
            RuleFor(x => x.PrsnType).NotEmpty().WithMessage("人員類別不可為空白。");
            //RuleFor(x => x.MajorSpecialistNo).NotNull().NotEmpty();允許專科
        }
    }
}
