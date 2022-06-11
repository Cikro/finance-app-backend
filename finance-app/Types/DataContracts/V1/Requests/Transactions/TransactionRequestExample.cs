using System;
using System.Diagnostics.CodeAnalysis;
using finance_app.Types.DataContracts.V1.Dtos;
using finance_app.Types.DataContracts.V1.Dtos.Enums;
using finance_app.Types.DataContracts.V1.Requests.Accounts;
using finance_app.Types.DataContracts.V1.Requests.Transactions;
using Swashbuckle.AspNetCore.Filters;



namespace CarfaxCanada.Dealer.Api.ContractExamples
{
    /// <summary>Provider for swagger examples used with GetAccountsRequest</summary>
    /// <remarks>See documentation for Swagger.AspNetCore.Filters, Manual Annotations, for details to use this example.</remarks>
    public class GetTransactionsRequestExample : IExamplesProvider<GetTransactionsRequest>
    {
        /// <summary>Returns the example data for the GetTransactionsRequest structure</summary>
        /// <returns>GetTransactionsRequest</returns>
        public GetTransactionsRequest GetExamples()
        {
            return new GetTransactionsRequest{
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
    public class UpdateTransactionRequestExample : IExamplesProvider<UpdateTransactionRequest>
    {
        /// <summary>Returns the example data for the CreateAccountRequest structure</summary>
        /// <returns>UpdateTransactionRequest</returns>
        public UpdateTransactionRequest GetExamples()
        {
            return new UpdateTransactionRequest{
                Notes = "A test note from Swagger"
            };
        }
    }
}