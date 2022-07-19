using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using finance_app.Types.DataContracts.V1.Dtos;
using finance_app.Types.DataContracts.V1.Dtos.Enums;
using finance_app.Types.DataContracts.V1.Requests.JournalEntries;
using Swashbuckle.AspNetCore.Filters;



namespace finance_app.Types.DataContracts.ContractExamples
{
    /// <summary>Provider for swagger examples used with.</summary>
    /// <remarks>See documentation for Swagger.AspNetCore.Filters, Manual Annotations, for details to use this example.</remarks>
    public class GetRecentJournalEntriesRequestExample : IExamplesProvider<GetRecentJournalEntriesRequest>
    {
        /// <summary>Returns the example data for the GetRecentJournalEntriesRequest structure</summary>
        /// <returns>GetRecentJournalEntriesRequest</returns>
        public GetRecentJournalEntriesRequest GetExamples()
        {
            return new GetRecentJournalEntriesRequest{
                PageInfo = new PaginationInfo() {
                    ItemsPerPage = 5,
                    PageNumber = 1
                },
                IncludeTransactions = true
            };
        }
    }

    /// <summary>Provider for swagger examples.</summary>
    /// <remarks>See documentation for Swagger.AspNetCore.Filters, Manual Annotations, for details to use this example.</remarks>
    [ExcludeFromCodeCoverage]
    public class CreateJournalEntryRequestExample : IExamplesProvider<CreateJournalEntryRequest>
    {
        /// <summary>Returns the example data for the CreateJournalEntryRequest</summary>
        /// <returns>CreateJournalEntryRequest</returns>
        public CreateJournalEntryRequest GetExamples()
        {
            return new CreateJournalEntryRequest {
                Transactions = new List<TransactionDto> {
                    {
                        new TransactionDto {
                            Amount = 100,
                            AccountId = 1,
                            Type = new EnumDto<TransactionTypeDtoEnum> {
                                Value = TransactionTypeDtoEnum.Debit
                            },
                            TransactionDate = DateTime.Parse("2022-07-15"),
                            Notes = "A test Debit"
                    }},
                    {
                        new TransactionDto {
                            Amount = 100,
                            AccountId = 2,
                            Type = new EnumDto<TransactionTypeDtoEnum> {
                                Value = TransactionTypeDtoEnum.Credit
                            },
                            TransactionDate = DateTime.Parse("2022-07-15"),
                            Notes = "A test Credit"
                    }}
                }
            };
        }
    }

    /// <summary>Provider for swagger examples.</summary>
    /// <remarks>See documentation for Swagger.AspNetCore.Filters, Manual Annotations, for details to use this example.</remarks>
    [ExcludeFromCodeCoverage]
    public class CorrectJournalEntryRequestExample : IExamplesProvider<CorrectJournalEntryRequest>
    {
        /// <summary>Returns the example data for the CorrectJournalEntryRequest</summary>
        /// <returns>PostAccountRequest</returns>
        public CorrectJournalEntryRequest GetExamples()
        {
            return new CorrectJournalEntryRequest{
                Transactions = new List<TransactionDto> {
                    {
                        new TransactionDto {
                            Amount = 100,
                            AccountId = 1,
                            Type = new EnumDto<TransactionTypeDtoEnum> {
                                Value = TransactionTypeDtoEnum.Debit
                            },
                            TransactionDate = DateTime.Parse("2022-07-15"),
                            Notes = "A test Debit"
                    }},
                    {
                        new TransactionDto {
                            Amount = 100,
                            AccountId = 2,
                            Type = new EnumDto<TransactionTypeDtoEnum> {
                                Value = TransactionTypeDtoEnum.Credit
                            },
                            TransactionDate = DateTime.Parse("2022-07-15"),
                            Notes = "A test Credit"
                    }}
                }
            };
        }
    }
}