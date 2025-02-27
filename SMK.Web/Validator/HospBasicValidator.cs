using FluentValidation;
using SMK.Web.Models;

namespace SMK.Web.Validator
{
    public class HospBasicViewModelValidator : AbstractValidator<HospBasicViewModel>
    {
        public HospBasicViewModelValidator()
        {
            RuleFor(x => x.HospId).NotEmpty().WithMessage("機構代碼不可為空白。");
            RuleFor(x => x.HospSeqNo).NotEmpty().WithMessage("機構子代碼不可為空白。");
            RuleForEach(x => x.HospContracts).SetValidator(new HospContractValidator());
        }
    }
}
