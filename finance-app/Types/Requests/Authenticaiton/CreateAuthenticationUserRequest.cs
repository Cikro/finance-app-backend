using AutoMapper;
using finance_app.Types.Repositories.Authentication;
using finance_app.Types.Repositories.FinanceApp;
using finance_app.Types.Services.V1.ResponseMessages;
using finance_app.Types.Services.V1.ResponseMessages.ActionMessages;
using finance_app.Types.Services.V1.ResponseMessages.ReasonMessages;
using finance_app.Types.Services.V1.ResponseMessages.ResourcesMessages;
using finance_app.Types.Services.V1.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace finance_app.Types.Requests.Authenticaiton {


    public class CreateAuthenticationUserRequest : Request {

        /// <summary>
        /// The user who is logging in
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The user's password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// The user's email
        /// </summary>
        public string Email { get; set; }

        private readonly AuthenticationContext _authenticationDbContext;
        private readonly IPasswordService _passwordService;

        public CreateAuthenticationUserRequest(
                    AuthenticationContext authenticationDbContext,
                    IPasswordService passwordService) {

            _authenticationDbContext = authenticationDbContext;
            _passwordService = passwordService;
        }

        public CreateAuthenticationUserRequest() 
        { 
        }

        public override T GetResponse<T>(IMapper mapper) {
            return mapper.Map<T>(this);
        }

        public async override Task ProcessRequest() {
            var authUser = _authenticationDbContext.Users
                .Where(x => x.UserName == Username)
                .FirstOrDefault();

            if (authUser != null) {
                Status = DataContracts.V1.Responses.ApiResponseCodesEnum.DuplicateResource;
                ResponseMessage = new ErrorResponseMessage(
                        new CreatingActionMessage(Username),
                        new ResourceWithPropertyMessage(authUser, "Username", Username),
                        new PropertyAlreadyExistsReason());
                return;
            }

            await _authenticationDbContext.Users.AddAsync(new AuthenticationUser(_passwordService, Username, Password, Email));
            await _authenticationDbContext.SaveChangesAsync();   
        }

    }
}
