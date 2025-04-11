using System.Net;

namespace TestApi.Infrastructure.Models
{
    public class ResponseModel<T>
    {
        public T? Result { get; set; }
        public bool Success { get; set; } = true;
        public int StatusCode { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public List<string> Messages { get; set; } = new List<string>();

        public ResponseModel()
        {
            Result = default;
        }

        public ResponseModel(bool success)
        {
            Success = success;
            Result = default;
        }

        public ResponseModel(bool success, T result)
        {
            Success = success;
            Result = result;
        }

        public bool IsSuccess()
        {
            return this.Success;
        }

        public bool IsSuccessWithResult()
        {
            return this.Success && this.Result != null;
        }

        public void AddError(string error)
        {
            this.Errors.Add(error);
        }

        public void AddMessage(string message)
        {
            this.Messages.Add(message);
        }

        public void BadRequest()
        {
            this.Success = false;
            this.StatusCode = (int)HttpStatusCode.BadRequest;
        }

        public void BadRequest(string message)
        {
            this.Success = false;
            this.StatusCode = (int)HttpStatusCode.BadRequest;

            AddError(message);
        }

        public void Ok()
        {
            this.StatusCode = (int)HttpStatusCode.OK;
        }

        public void Ok(string message)
        {
            this.StatusCode = (int)HttpStatusCode.OK;
            AddMessage(message);
        }

        public void Ok(T result)
        {
            this.StatusCode = (int)HttpStatusCode.OK;
            this.Result = result;
        }

        public void Ok(T result, string message)
        {
            this.StatusCode = (int)HttpStatusCode.OK;
            this.Result = result;
            AddMessage(message);
        }

        public void NotFound()
        {
            this.StatusCode = (int)HttpStatusCode.NotFound;
        }

        public void InternalServerError()
        {
            this.StatusCode = (int)HttpStatusCode.InternalServerError;
        }

        public void InternalServerError(string error)
        {
            this.StatusCode = (int)HttpStatusCode.InternalServerError;
            AddError(error);
        }

        public void UnAuthorized()
        {
            this.StatusCode = (int)HttpStatusCode.Unauthorized;
        }

        public void UnAuthorized(string error)
        {
            this.StatusCode = (int)HttpStatusCode.Unauthorized;
            AddError(error);
        }
    }
}
