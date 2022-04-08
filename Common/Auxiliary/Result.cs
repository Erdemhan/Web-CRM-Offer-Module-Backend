using System;

namespace crmweb.Common.Auxiliary
{
    public class Result : IDisposable
    {
        protected Result()
        {
            Success = false;
            Information = "";
        }

        public static Result PrepareFailure(string info)
        {
            return new Result()
            {
                Success = false,
                Information = info
            };
        }

        public static Result PrepareSuccess(string info = "")
        {
            return new Result()
            {
                Success = true,
                Information = info
            };
        }

        public static Result PrepareSuccess(int rowsEffected)
        {
            return new Result()
            {
                Success = true,
                Information = ""
            };
        }
        
        public static Result PrepareResult(Result result)
        {
            return new Result()
            {
                Success = result.Success,
                Information = result.Information
            };
        }

        public static Result PrepareResult(bool success, string info)
        {
            return new Result()
            {
                Success = success,
                Information = info
            };
        }

        public bool Success { get; set; }
        public string Information { get; set; }

        ///////////////////////////////////////////////////////////////////////////////////////////
        //Dispose
        ///////////////////////////////////////////////////////////////////////////////////////////

        // Flag: Has Dispose already been called?
        protected bool Disposed;

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        private void Dispose(bool ADisposing)
        {
            try
            {
                if (Disposed)
                    return;

                if (ADisposing)
                {

                }

                // Free any unmanaged objects here.
                //
                Disposed = true;
            }
            catch
            {
                // ignored
            }
        }

        ~Result()
        {
            Dispose(false);
        }
    }

    public class Result<T> : Result
    {
        private Result()
        {
            Success = false;
            Information = "Tanýmsýz hata";
            Payload = default(T);
        }

        public Result(Result result)
        {
            Success = result.Success;
            Information = result.Information;

            Payload = default(T);
        }
        
        public new static Result<T> PrepareFailure(string info)
        {
            return new Result<T>()
            {
                Success = false,
                Information = info,
                Payload = default
            };
        }
        public static Result<T> PrepareFailure(Result result)
        {
            return new Result<T>()
            {
                Success = false,
                Information = result.Information,
                Payload = default
            };
        }

        public static Result<T> PrepareFailure(string info, T payload)
        {
            return new Result<T>()
            {
                Success = false,
                Information = info,
                Payload = payload
            };
        }

        public static Result<T> PrepareSuccess(T payload)
        {
            return new Result<T>()
            {
                Success = true,
                Information = "",
                Payload = payload
            };
        }

        public new static Result<T> PrepareResult(Result result)
        {
            return new Result<T>()
            {
                Success = result.Success,
                Information = result.Information,
                Payload = default
            };
        }

        public static Result<T> PrepareResult(Result result, T payload)
        {
            return new Result<T>()
            {
                Success = result.Success,
                Information = result.Information,
                Payload = payload
            };
        }

        public static Result<T> PrepareResult(bool success, string info, T payload)
        {
            return new Result<T>()
            {
                Success = success,
                Information = info,
                Payload = payload
            };
        }

        public T Payload { get; set; }

        ///////////////////////////////////////////////////////////////////////////////////////////
        //Dispose
        ///////////////////////////////////////////////////////////////////////////////////////////

        // Protected implementation of Dispose pattern.
        private void Dispose(bool disposing)
        {
            try
            {
                if (Disposed)
                    return;

                if (disposing)
                {
                    if (!typeof(T).IsPrimitive)
                    {
                        var vDisposable = Payload as IDisposable;
                        vDisposable?.Dispose();
                    }
                }

                // Free any unmanaged objects here.
                //
                Disposed = true;
            }
            catch
            {
                // ignored
            }
        }

        ~Result()
        {
            Dispose(false);
        }
    }
}