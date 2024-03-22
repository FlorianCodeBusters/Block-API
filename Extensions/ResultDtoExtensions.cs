namespace Blocks_api.Extensions
{
    using FluentResults;
    using Blocks_api.Dtos;
    public static class ResultDtoExtensions
    {
        public static ResultDto<TResponse> ToResultDto<TResponse>(this Result<TResponse> result)
        {
            var errorMessages = result.Errors?.Select(error => error.Message);

            return new ResultDto<TResponse>(result.IsSuccess, result.ValueOrDefault, errorMessages);
        }
    }
}
