using Exercise.Infrastructure;
using Exercise.Model;
using Microsoft.AspNetCore.Mvc;

namespace Exercise.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DataController : ControllerBase
    {
        private readonly ILogger<DataController> _logger;
        private readonly IGenericRepository<ModelType1> _modelType1Repository;
        private readonly IGenericRepository<ModelType2> _modelType2Repository;

        public DataController(ILogger<DataController> logger
            , IGenericRepository<ModelType1> modelType1Repository
            , IGenericRepository<ModelType2> modelType2Repository)
        {
            _logger = logger;
            _modelType1Repository = modelType1Repository;
            _modelType2Repository = modelType2Repository;
        }

        [HttpPost]
        [Route("ModelType1")]
        public async Task<IActionResult> ModelType1(ModelType1 modelType1)
        {
            _logger.LogDebug("ModelType1");
            var result = await _modelType1Repository.SaveAsync(modelType1);
            modelType1.TagFromServer = Guid.NewGuid().ToString();
            return Ok(result);
        }

        [HttpPost]
        [Route("ModelType2")]
        public async Task<IActionResult> ModelType2(ModelType2 modelType2)
        {
            _logger.LogDebug("ModelType2");
            var result = await _modelType2Repository.SaveAsync(modelType2);
            modelType2.TagFromServer = Guid.NewGuid().ToString();
            return Ok(result);
        }

        [HttpGet]
        [Route("Hello")]
        public async Task<IActionResult> Hello(string name)
        {
            // Note: For testing connectivity to the controller

            await Task.CompletedTask;
            return Ok($"Hello, {name}");
        }
    }
}