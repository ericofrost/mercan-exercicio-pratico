// namespace ER.Application.UnitTests.Validators;
//
// public class LoginValidatorTests
// {
//     private readonly LoginValidator _validator = new();
//
//     [Fact]
//     public void ValidRequest_ShouldNotHaveValidationErrors()
//     {
//         var request = new LoginRequestBuilder().Build();
//
//         var result = _validator.TestValidate(request);
//
//         result.ShouldNotHaveAnyValidationErrors();
//     }
//
//     [Fact]
//     public void EmptyEmail_ShouldHaveValidationError()
//     {
//         var request = new LoginRequestBuilder().WithEmail("").Build();
//
//         var result = _validator.TestValidate(request);
//
//         result.ShouldHaveValidationErrorFor(r => r.Email);
//     }
//
//     [Fact]
//     public void EmptyTenant_ShouldHaveValidationError()
//     {
//         var request = new LoginRequestBuilder().WithTenantId(Guid.Empty).Build();
//
//         var result = _validator.TestValidate(request);
//
//         result.ShouldHaveValidationErrorFor(r => r.TenantId);
//     }
//
//     [Fact]
//     public void EmptyPassword_ShouldHaveValidationError()
//     {
//         var request = new LoginRequestBuilder().WithPassword("").Build();
//
//         var result = _validator.TestValidate(request);
//
//         result.ShouldHaveValidationErrorFor(r => r.Password);
//     }
// }
