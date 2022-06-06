using BlogProject.Shared.Entities.Concrete;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Data.SqlTypes;

namespace BlogProject.MvcUI.Filters
{
    public class MvcExceptionFilter : IExceptionFilter
    {
        private readonly IHostEnvironment _hostEnvironment;
        private readonly IModelMetadataProvider _metaDataProvider;
        private readonly ILogger _logger;

        public MvcExceptionFilter(IHostEnvironment hostEnvironment, IModelMetadataProvider modelMetadataProvider,ILogger<MvcExceptionFilter> logger)
        {
            _hostEnvironment = hostEnvironment;
            _metaDataProvider = modelMetadataProvider;
            _logger = logger;
        }
        

        public void OnException(ExceptionContext context)
        {
            if (_hostEnvironment.IsDevelopment())
            {
                context.ExceptionHandled = true;
                var mvcErrorModel = new MvcErrorModel();
                ViewResult result;
                switch (context.Exception)
                {
                    case SqlNullValueException:
                        mvcErrorModel.Message = $"Beklenmedik bir veritabanı hatası oluştu.";
                        mvcErrorModel.Detail = context.Exception.Message;
                        result = new ViewResult { ViewName = "Error" };
                        result.StatusCode = 500;
                        _logger.LogError(context.Exception, context.Exception.Message);
                        break;
                    case NullReferenceException:
                        mvcErrorModel.Message = $"Beklenmedik bir null veri hatası oluştu.";
                        mvcErrorModel.Detail = context.Exception.Message;
                        result = new ViewResult { ViewName = "Error" };
                        result.StatusCode = 403;
                        _logger.LogError(context.Exception, context.Exception.Message);
                        break;
                    default:
                        mvcErrorModel.Message = $"Beklenmedik bir hata oluştu.";
                        result = new ViewResult { ViewName = "Error" };
                        result.StatusCode = 500;
                        _logger.LogError(context.Exception, context.Exception.Message);
                        break;

                }
                result = new ViewResult { ViewName = "Error" };
                
                result.ViewData = new ViewDataDictionary(_metaDataProvider, context.ModelState);
                result.ViewData.Add("MvcErrorModel", mvcErrorModel);
                context.Result = result;

            }
                
        }
    }
}
