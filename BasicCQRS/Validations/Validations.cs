using BasicCQRS.Data;
using FluentValidation;

public class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
{
    public CreateEmployeeCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
        RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("A valid email is required.");
        RuleFor(x => x.Phone).NotEmpty().WithMessage("Phone number is required.");
        // Add other validation rules as needed
    }
}

public class UpdateEmployeeCommandValidator : AbstractValidator<UpdateEmployeeCommand>
{
    public UpdateEmployeeCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("Valid ID is required.");
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
        // Add other validation rules as needed
    }
}
