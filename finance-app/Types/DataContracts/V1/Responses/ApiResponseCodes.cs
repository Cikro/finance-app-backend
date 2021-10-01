namespace finance_app.Types.DataContracts.V1.Responses {
    public enum ApiResponseCodesEnum {
        Success = 200,
        BadRequest = 400,
        InternalError = 500,

        // A duplicate resouce already exists in the system
        DuplicateResource = -1,

        // The Resource was not found in the system.
        ResourceNotFound = -2

    }
}