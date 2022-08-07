using System;
using System.Diagnostics.CodeAnalysis;
using finance_app.Types.DataContracts.V1.Dtos;
using finance_app.Types.DataContracts.V1.Dtos.Enums;
using finance_app.Types.DataContracts.V1.Requests.Accounts;
using Swashbuckle.AspNetCore.Filters;



namespace finance_app.Types.DataContracts.ContractExamples
{
    /// <summary>Provider for swagger examples used with GetAccountsRequest</summary>
    /// <remarks>See documentation for Swagger.AspNetCore.Filters, Manual Annotations, for details to use this example.</remarks>
    public class GetAccountsRequestExample : IExamplesProvider<GetAccountsRequest>
    {
        /// <summary>Returns the example data for the GetAccountsRequest structure</summary>
        /// <returns>GetAccountsRequest</returns>
        public GetAccountsRequest GetExamples()
        {
            return new GetAccountsRequest{
                PageInfo = new PaginationInfo() {
                    ItemsPerPage = 5,
                    PageNumber = 1
                } 
            };
        }
    }

    /// <summary>Provider for swagger examples used with CreateAccountRequest</summary>
    /// <remarks>See documentation for Swagger.AspNetCore.Filters, Manual Annotations, for details to use this example.</remarks>
    [ExcludeFromCodeCoverage]
    public class CreateAccountRequestExample : IExamplesProvider<CreateAccountRequest>
    {
        /// <summary>Returns the example data for the CreateAccountRequest structure</summary>
        /// <returns>CreateAccountRequest</returns>
        public CreateAccountRequest GetExamples()
        {
            return new CreateAccountRequest{
                Name = $"Swagger Account-{new Random().Next(9000,10000)}",
                CurrencyCode = "CAD",
                Description = "Test account from Swagger",
                ParentAccountId = null,
                Type = new EnumDto<AccountTypeDtoEnum>(AccountTypeDtoEnum.Asset) 

            };
        }
    }

    /// <summary>Provider for swagger examples used with CreateAccountRequest</summary>
    /// <remarks>See documentation for Swagger.AspNetCore.Filters, Manual Annotations, for details to use this example.</remarks>
    [ExcludeFromCodeCoverage]
    public class PostAccountRequestExample : IExamplesProvider<PostAccountRequest>
    {
        /// <summary>Returns the example data for the PostAccountRequest structure</summary>
        /// <returns>PostAccountRequest</returns>
        public PostAccountRequest GetExamples()
        {
            return new PostAccountRequest{
                Id = 0,
                Name = $"Swagger Account-{new Random().Next(9000,10000)}",
                Description = "Test account from Swagger",
                Closed = false

            };
        }
    }
}