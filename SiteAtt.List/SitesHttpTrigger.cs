using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using SiteAtt.List.Abstractions;
using SiteAtt.List.Contracts;
using SiteAtt.List.Models;

namespace SiteAtt.List ;

    public class SitesHttpTrigger
    {
        private readonly ISitesService _sitesService;
        
        public SitesHttpTrigger(ISitesService sitesService)
        {
            _sitesService = sitesService;
        }

        [Function("GetList")]
        [OpenApiOperation(
            operationId: "GetSitesList",
            tags: new[] { "sites"})
        ]
        [OpenApiResponseWithBody(
            statusCode: HttpStatusCode.OK,
            contentType: "application/json",
            bodyType: typeof(SiteListDto),
            Description = "OK response")
        ]
        public async Task<HttpResponseData> GetList([HttpTrigger(AuthorizationLevel.Anonymous,  "get", Route = "sites")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var sites = await _sitesService.GetSitesAsync();

            var dto = ToDto(sites);

            return await CreateResponse(req, dto);
        }
        
        [Function("StoreSites")]
        [OpenApiOperation(
            operationId: "StoreSites",
            tags: new[] { "sites"})
        ]
        [OpenApiRequestBody("application/json", typeof(CreateSitesRequest), Description = "Sites to store. It can be URLs or IP Addresses")]
        [OpenApiResponseWithBody(
            statusCode: HttpStatusCode.OK,
            contentType: "application/json",
            bodyType: typeof(string),
            Description = "OK response")
        ]
        public async Task<HttpResponseData> Post([HttpTrigger(AuthorizationLevel.Anonymous,  "post", Route = "sites")]
        HttpRequestData req,
            FunctionContext executionContext)
        {
            var request = await req.ReadFromJsonAsync<CreateSitesRequest>();
            
            await _sitesService.CreateOrUpdateSitesAsync(request);

            var response = req.CreateResponse(HttpStatusCode.OK);
            return response;
        }
        
        private static SiteListDto ToDto(SitesModel sites)
        {
            var dto = new SiteListDto
            {
                Sites = sites.Sites
            };

            return dto;
        }

        private static async Task< HttpResponseData> CreateResponse(HttpRequestData requestData, SiteListDto dto)
        {
            var response = requestData.CreateResponse(HttpStatusCode.OK);

            await response.WriteAsJsonAsync(dto);

            return response;
        }
    }